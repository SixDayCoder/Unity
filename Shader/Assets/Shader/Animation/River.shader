Shader "Custom/River" {
	Properties{
		_MainTex("Main Tex", 2D) = "white"{}   //河流纹理
		_Color("Color Tint", Color) = (1, 1, 1, 1) //控制纹理显示的颜色
		_Magnitude("Distortion Magnitude", Float) = 1 //水流波动的幅度
		_Frequency("Distortion Frequency", Float) = 1 //波动的频率
		_InvWaveLength("Distortion Inverse Wave Length", Float) = 10 //波长的倒数
		_Speed("Speed", Float) = 0.5 //纹理移动速度
	}
	SubShader{
		Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "DisableBatching" = "True"}
		/*
		使用DisableBatching标签设置该Pass是否启用批处理.批处理会合并所有相关的模型,这些模型的模型空间就会失去
		在本例中要对模型空间的顶点做处理,所以取消该操作
		*/
		Pass{
			Tags{"LightMode" = "ForwardBase"}

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Cull off  //让水流每个面都能显示

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _Magnitude, _Frequency, _InvWaveLength, _Speed;

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

				float4 offset;

				offset.yzw = float3(0, 0, 0);//只在水平方向偏移

				float4	bias = v.vertex.x * _InvWaveLength + v.vertex.y * _InvWaveLength + v.vertex.z * _InvWaveLength;
				offset.x = sin(_Frequency * _Time.y + bias) * _Magnitude;
				/*
				为了保证不同的位置有不同的位移,添加了模型空间下的位置分量
				*/
				o.pos = UnityObjectToClipPos(v.vertex + offset);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv += float2(0.0, _Time.y * _Speed);   //纹理动画的水平方向的偏移

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
