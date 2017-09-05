Shader "Custom/EdgeDetection" {
	Properties{
		_MainTex("Main Texture", 2D) = "white"{}
		_EdgeOnly("Edge Only", Range(0,1)) = 1.0  //为0表示在原图基础上渲染,1表示为只渲染描边
		_EdgeColor("Edge Color", Color) = (0, 0, 0, 1)
		_BgColor("Background Color", Color) = (1, 1, 1, 1)
	}

	SubShader{
			Pass{
				ZTest Always Cull Off ZWrite Off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				half4  _MainTex_TexelSize;
				fixed  _EdgeOnly;
				fixed4 _EdgeColor, _BgColor;
				/*
				xxxx_TexelSize是Unity提供的访问xxxx纹理的对应的每个像素的大小。512 x 512 像素大小的纹理,其值为 1/512
				由于卷积要对相邻区域内的纹理采样 需要该变量来计算相邻区域的纹理坐标
				这里使用Sobel边缘检测算子
				{
					-1, -2, -1
					 0,  0,  0
					 1,  2,  1
				}//Gx

				{
					-1,  0,  1
					-2,  0,  2
					 1,  0,  1
				}//Gy
				对每个像素分别进行一次卷积,得到两个方向上的梯度值Gx和Gy,整体梯度 G = sqrt(gx*gx + gy*gy)
				*/

				struct v2f {
					float4 pos : SV_POSITION;
					half2 uv[9] : TEXCOORD0;//存储边缘检测算子
				};

				v2f vert(appdata_img v) {
					v2f o;

					o.pos = UnityObjectToClipPos(v.vertex);
					half2 uv = v.texcoord;

					o.uv[0] = uv + _MainTex_TexelSize.xy * half2(-1, -1);
					o.uv[1] = uv + _MainTex_TexelSize.xy * half2(0, -1);
					o.uv[2] = uv + _MainTex_TexelSize.xy * half2(1, -1);
					o.uv[3] = uv + _MainTex_TexelSize.xy * half2(-1, 0);
					o.uv[4] = uv + _MainTex_TexelSize.xy * half2(0, 0);
					o.uv[5] = uv + _MainTex_TexelSize.xy * half2(1, 0);
					o.uv[6] = uv + _MainTex_TexelSize.xy * half2(-1, 1);
					o.uv[7] = uv + _MainTex_TexelSize.xy * half2(0, 1);
					o.uv[8] = uv + _MainTex_TexelSize.xy * half2(1, 1);
					//根据偏移量计算uv坐标
					return o;
				}


				fixed luminance(fixed4 color) {
					return 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
				}

				half Sobel(v2f o) {
					const half Gx[9] = {
						-1, -2, -1,
						 0,  0,  0,
						 1,  2,  1
					};

					const half Gy[9] = {
						-1,  0, 1,
						-2,  0, 2,
						-1,  0, 1
					};

					half texColor;
					half edgeX = 0, edgeY = 0;

					for (int i = 0; i < 9; ++i) {
						texColor = luminance(tex2D(_MainTex, o.uv[i]));
						edgeX += texColor * Gx[i];
						edgeY += texColor * Gy[i];
					}

					half edge = 1 - abs(edgeX) - abs(edgeY);

					return edge;
				}

				fixed4 frag(v2f o) : SV_Target {
					half edge = Sobel(o);

					fixed4 withEdgeColor = lerp(_EdgeColor, tex2D(_MainTex, o.uv[4]), edge);
					fixed4 onlyEdgeColor = lerp(_EdgeColor, _BgColor, edge);
					return lerp(withEdgeColor, onlyEdgeColor, _EdgeOnly);
				}

				ENDCG

		}
	}
}
