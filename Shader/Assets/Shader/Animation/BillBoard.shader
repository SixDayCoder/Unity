Shader "Custom/BillBoard" 
{
	Properties{
		_MainTex("Main Tex", 2D) = "white"{}
		_Color("Color Tint", Color) = (1, 1, 1, 1)
		_VerticalBillboarding("Vertical Restraints", Range(0,1)) = 1
		//调整是固定法线方向还是固定up方向
	}

	SubShader{
		Tags{"Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" "DisableBathing" = "True"}

		Pass{
			Tags{"LightMode" = "ForwardBase"}
			ZWrite Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _VerticalBillboarding;

			struct a2v {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};


			v2f vert(a2v v) {
				v2f o;

				float3 anchor = float3(0, 0, 0);//选择模型的原点作为锚点
				float3 view = mul(float4(_WorldSpaceCameraPos, 1), unity_WorldToObject);//摄像机坐标变换到模型空间

				//计算正交向量基
				float3 normal = view - anchor;
				/*
				if(_VerticalBillboarind = 1  固定normal为view的方向  否则固定up的方向为(0,1,0)这意味着normal的y必须为0(x,0,y)保证垂直
				*/
				normal.y = normal.y * _VerticalBillboarding;
				normal = normalize(normal);

				float3 up = abs(normal.y) > 0.999 ? float3(0, 0, 1) : float3(0, 1, 0);
				float3 right = normalize(cross(up, normal));
				up = normalize(cross(normal, right));

				//得到正交基后,根据原始的位置相对于锚点的偏移量和三个正交基,计算得到新的顶点位置
				float3 anchorOff = v.vertex.xyz - anchor;
				float3 localPos = anchor + right * anchorOff.x + up * anchorOff.y + normal * anchorOff.z;

				o.pos = UnityObjectToClipPos(float4(localPos, 1));
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				return o;
			}

			fixed4 frag(v2f o) : SV_Target{
				fixed4 c = tex2D(_MainTex, o.uv);
				c.rgb *= _Color.rgb;

				return c;
			}

			ENDCG
		}
	}
}
/*
始终朝向相机的四边形
广告牌技术 : 根据视角方向来旋转一个被纹理着色的多边形,重点是构建旋转矩阵
使用表面法线normal up right来构造旋转矩阵，除此之外指定锚点anchor在旋转过程中固定不变 确定多边形在空间中的位置

构造过程如下 
首先通过初试计算获得object的normal和up,这两者是不一定垂直的,但是两者其中之一必定是固定的
假设法线方向是固定的 ：

right = up x normal
up(real) = normal x right
所以需要有一个参数来指定固定的是normal还是up
*/