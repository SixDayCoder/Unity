Shader "Custom/MotionBlur" {

	Properties{
		_MainTex("Base(RGB)", 2D) = "white"{}
		_BlurAmount("BlurAmount", Float) = 1.0
	}

		SubShader{
			/*----------------------------------CGINLCUDE-----------------------------------*/
			CGINCLUDE
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed	_BlurAmount;

			struct v2f {
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			v2f vert(appdata_img v) {
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;

				return o;
			}

			/*
			更新渲染纹理的RGB通道
			在做这件事件的时候,我们仅需要A通道来混合图像而不想写入A通道的值
			*/
			fixed4 fragRGB(v2f o) : SV_Target{
				return fixed4(tex2D(_MainTex, o.uv).rgb, _BlurAmount);//使用_BlurAmount混合RGB
			}

			/*更新渲染纹理的A通道*/
			half4 fragA(v2f o) : SV_Target{
					return tex2D(_MainTex, o.uv);
			}


			ENDCG
			/*----------------------------------CGINLCUDE-----------------------------------*/

			ZTest Always Cull Off ZWrite Off


			Pass {

				Blend SrcAlpha OneMinusSrcAlpha
				ColorMask RGB
				//仅写入颜色
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment fragRGB
				ENDCG
			}

			Pass {

				Blend One Zero
				ColorMask A
				//仅写入透明通道
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment fragA
				ENDCG
			}
		}

}
/*
运动模糊的实现原理
1.累积缓存  物体移动产生多张图像的时候,取多张图像的平均值做运动模糊图像  性能消耗很大
2.速度缓存  存储每个像素当前的运动速度,利用该值决定模糊的方向和大小

这里我们对1进行改进,不在一帧渲染多次,保存上一帧的渲染结果并不断的叠加到当前渲染图像上
*/