Shader "Custom/Sakura0" {
	Properties{
		_MainTex("Base(RGB)", 2D) = "white"{} //OnRenderImage的sourceTexture
		_Brightness("Brightness", Float) = 1
		_Saturation("Saturation", Float) = 1
		_Contrast("Contrast", Float) = 1
	}

		SubShader{
			Pass{
				ZTest Always Cull Off ZWrite Off
				
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				float4 _MainTex_ST;	
				half _Brightness, _Saturation, _Contrast;

				struct v2f {
					float4 pos :  SV_POSITION;
					half2 uv : TEXCOORD0;
				};
				
				v2f vert(appdata_img v) {
					v2f o;

					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.texcoord;

					return o;

				}

				fixed4 frag(v2f o) : SV_Target{
					fixed4 renderTex = tex2D(_MainTex, o.uv);
					
					//brightness
					fixed3 color = renderTex.rgb * _Brightness;

					//saturation
					fixed luminance = 0.2125 * renderTex.r + 0.7154 * renderTex.g + 0.0721 * renderTex.b;
					fixed3 luminanceeColor = fixed3(luminance, luminance, luminance);
					color = lerp(luminanceeColor, color, _Saturation);

					//contrast
					fixed3 avgColor = fixed3(0.5, 0.5, 0.5);

					color = lerp(avgColor, color, _Contrast);

					return fixed4(color, renderTex.a);

				}
				ENDCG
			}
		}
		Fallback Off
}
