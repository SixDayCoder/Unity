Shader "Custom/ScrollBackground" {
	Properties{
		_MainTex("Base Layer(RGB)", 2D) = "white"{}
		_DetailTex("2nd Layer(RGB)", 2D) = "white"{}
		_ScrollX("Base Layer Scroll Speed", Float) = 1.0
		_Scroll2X("2nd Layer Scroll Speed", Float) = 1.0
		_Multiplier("Layer Multiplier", Float) = 1.0 //控制纹理的整体亮度
	}

	SubShader{
		Pass{
			Tags{"LightMode" = "ForwardBase"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _DetailTex;
			float _ScrollX;
			float _Scroll2X;
			float _Multiplier;
			float4 _MainTex_ST;
			float4 _DetailTex_ST;


			struct a2v {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
			};
			
			v2f vert(a2v v) {
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex) + frac(float2(_ScrollX, 0.0) * _Time.y);//在x轴方向实现水平偏移
				o.uv.zw = TRANSFORM_TEX(v.texcoord, _DetailTex) + frac(float2(_Scroll2X, 0.0) * _Time.y);

				return o;
			}

			fixed4 frag(v2f o) : SV_Target{
				fixed4 firstLayer = tex2D(_MainTex, o.uv.xy);
				fixed4 secondLayer = tex2D(_DetailTex, o.uv.zw);

				fixed4 color = lerp(firstLayer, secondLayer, secondLayer.a);
				color.rgb *= _Multiplier;
				return color;
			}
			ENDCG
		}
	
	}
}
