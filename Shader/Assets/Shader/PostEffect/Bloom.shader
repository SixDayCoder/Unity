Shader "Custom/Bloom" {
	Properties{
		_MainTex("Main Tex", 2D) = "white"{}
		_Bloom("Bloom Tex", 2D) = "black"{}
		_LuminanceThreshold("Luminance Threshold", Float) = 0.5  //光亮门限
		_BlurSize("Blur Size", Float) = 1.0
	}

		SubShader{
			
			/*--------------------------------------CGINCLUDE-----------------------------------------*/
			CGINCLUDE

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _Bloom;
			half4 _MainTex_TexelSize;
			float _LuminanceThreshold;
			float _BlurSize;

			struct v2f {
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			v2f vertExtractBright(appdata_img v) {
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;

				return o;
			}

			fixed luminance(fixed4 color) {
				return 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
			}

			fixed4 fragExtractBright(v2f o) : SV_Target{
				fixed4 color = tex2D(_MainTex, o.uv);
				fixed val = clamp(luminance(color) - _LuminanceThreshold, 0, 1.0f);

				return color * val;
			}


			struct v2fBloom {
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
			};

			v2fBloom vertBloom(appdata_img v){
				v2fBloom o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = v.texcoord;//主纹理的uv
				o.uv.zw = v.texcoord;//生成的光亮纹理的uv,和主纹理的大小是一致的,坐标保持一致

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0.0)
					o.uv.w = 1.0 - o.uv.w;
				#endif

				return o;

			}

			fixed4 fragBloom(v2fBloom o) : SV_Target{
				return tex2D(_MainTex, o.uv.xy) + tex2D(_Bloom, o.uv.zw);
			}

			ENDCG

				/*--------------------------------------CGINCLUDE-----------------------------------------*/

			ZWrite Off ZTest Always Cull Off

			Pass {
				CGPROGRAM
				#pragma vertex vertExtractBright
				#pragma fragment fragExtractBright
				ENDCG
			}

			UsePass "Custom/GaussianBlur/GAUSSIAN_BLUR_VERTICAL"
			UsePass "Custom/GaussianBlur/GAUSSIAN_BLUR_HORIZONTAL"


			Pass {
				CGPROGRAM
				#pragma vertex vertBloom
				#pragma fragment fragBloom
				ENDCG
			}
		}
}
/*
Bloom效果是指发亮的贴图能够将自己的亮的地方“晕染”出去,实现思路很简单：

提取光照比较亮的地方存到纹理当中(相比原纹理,不亮的地方将颜色设为0)，做高斯模糊,然后和原图融合即可。

因此需要四个Pass
Pass0  -> 提取
Pass1
Pass2  高斯模糊
Pass3  Bloom融合

*/
