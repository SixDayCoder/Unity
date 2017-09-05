Shader "Custom/AlphaTest" {
	Properties{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Texture", 2D) = "whiet"{}
		_Cutoff("Alpha Cut off", Range(0,1)) = 0.5
	}

	SubShader{
		Tags{ "Queue" = "AlphaTest"  "IgnoreProjector" = "True"  "RenderType" = "TransparentCutout" }
			/*
			透明度测试 某片元透明度不满足条件时被完全抹去
			RenderType可以把Shader归入到提前定义的组,在这里这个组的名字是TransparentCutout
			IgnoreProjector --> 忽略投影器
			*/
		Pass{
			Tags{"LightMode" = "ForwardBase"}


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Lighting.cginc"

			fixed4 _Color;
			float _Cutoff;
			float4 _MainTex_ST;
			sampler2D _MainTex;

			struct VertexIn {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct VertexOut {
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldPos    : TEXCOORD1;
				float2 uv : TEXCOORD2;
			};
			
			VertexOut vert(VertexIn v) {
				VertexOut o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(v.vertex, unity_ObjectToWorld).xyz;
				o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				return o;
			}

			fixed4 frag(VertexOut o) : SV_Target	 {
				fixed worldNormal = normalize(o.worldNormal);
				fixed worldLightDir = normalize(UnityWorldSpaceLightDir(o.worldPos));

				fixed4 texColor = tex2D(_MainTex, o.uv);
				//Alpha test
				//clip(texColor.a - _Cutoff); //这里相当于texColor.a < _Cutoff
				//clip(x) 如果x的某个分量小于0 即被舍弃
				if (texColor.a - _Cutoff < 0.0)
					discard;

				//diffuse term,half lambert
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				fixed3 halfLambert = dot(worldNormal, worldLightDir) * 0.5f + 0.5f;
				fixed3 albedo = _Color * texColor.rgb;
				fixed3 diffuseColor = _LightColor0.rgb * albedo * halfLambert;

				return fixed4(diffuseColor + ambient, 1.0);
			}

			ENDCG
		}
	}
}
/*
透明度测试是不关闭zwrite的,它表现的是,如果一个片元通过了透明度检测,那么就存在,否则完全舍弃
表现出来就是,某个物体要不全透明要不就全不透明
*/

/*
Unity的渲染顺序
Background  1000 在任何队列之前被渲染
Geometry    2000 默认的渲染队列 不透明物体
AlphaTest   2450 不透明物体渲染完之后,透明度检测
Transparent 3000 使用透明度混合的物体,从后往前排序后渲染，关闭了zwrite
Overlay     4000 实现叠加效果  最后被渲染
*/