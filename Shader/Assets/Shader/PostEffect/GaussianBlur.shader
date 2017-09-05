Shader "Custom/GaussianBlur" {
	Properties{
		_MainTex("Base", 2D) = "white"{}
		_BlurSize("Blur Size", Float) = 1.0
	}

		SubShader{

			/*---------------------------CGINCLUDE------------------------------------*/

			CGINCLUDE
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			half4 _MainTex_TexelSize;
			float _BlurSize;

			struct v2f {
				float4 pos : SV_POSITION;
				half2 uv[5] : TEXCOORD;
				/*
				将要使用的高斯卷积核是5x5的,但是将其拆分为了一维,所以UV[5]即可存储
				*/
			};

			v2f vertBlurVertical(appdata_img v) {
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);

				half2 uv = v.texcoord;

				o.uv[0] = uv;//中心点,即实际采样点
				o.uv[1] = uv + float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
				o.uv[2] = uv - float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
				o.uv[3] = uv + float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;
				o.uv[4] = uv - float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;
				/*
				a.y + _MainTex_TexelSize.y * 2.0
			    a.y + _MainTex_TexelSize.y * 1.0        
				采样点a
				a.y - _MainTex_TexelSize.y * 1.0
				a.y - _MainTex_TexelSize.y * 2.0
				*/

				return o;
			}

			v2f vertBlurHorizontal(appdata_img v) {
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);

				half2 uv = v.texcoord;

				o.uv[0] = uv;//中心点,即实际采样点
				o.uv[1] = uv + float2(0.0, _MainTex_TexelSize.x * 1.0) * _BlurSize;
				o.uv[2] = uv - float2(0.0, _MainTex_TexelSize.x * 1.0) * _BlurSize;
				o.uv[3] = uv + float2(0.0, _MainTex_TexelSize.x * 2.0) * _BlurSize;
				o.uv[4] = uv - float2(0.0, _MainTex_TexelSize.x * 2.0) * _BlurSize;
				/*
				a.x - _MainTex_TexelSize.x * 2.0 | a.x - _MainTex_TexelSize.x * 1.0 |  a  | a.x + _MainTex_TexelSize.x * 1.0  | a.x + _MainTex_TexelSize.x * 2.0
				*/
				return o;
			}

			//pass共用的片元着色器
			fixed4 frag(v2f o) : SV_Target{

				float weight[3] = {0.4026, 0.2242, 0.0545};

				fixed3 sum = tex2D(_MainTex, o.uv[0]) * weight[0];

				for (int i = 1; i <= 2; ++i) {
					sum += tex2D(_MainTex, o.uv[i]).rgb * weight[i];
					sum += tex2D(_MainTex, o.uv[i * 2]).rgb * weight[i];
				}

				return fixed4(sum, 1.0);
			}
			ENDCG


			/*---------------------------CGINCLUDE------------------------------------*/

				ZTest Always Cull Off ZWrite Off

				Pass {
					//给出name属性后可以在别的shader的pass中引用
					NAME "GAUSSIAN_BLUR_VERTICAL"

					CGPROGRAM
					#pragma vertex vertBlurVertical
					#pragma fragment frag
					ENDCG
				}

				Pass {
					NAME "GAUSSIAN_BLUR_HORIZONTAL"

					CGPROGRAM
					#pragma vertex vertBlurHorizontal
					#pragma fragment frag
					ENDCG

				}

		}
}
