

Shader "Custom/Simple" {

	Properties{
		_Color("Init Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}

	SubShader{
		Pass{
			
			CGPROGRAM

			#include "UnityCG.cginc" 


            #pragma vertex   vert
            #pragma fragment frag

			struct InVertex {
				float4 vertex   : POSITION;//模型坐标
				float3 normal   : NORMAL;//模型法线
				float4 texcoord : TEXCOORD0;//模型的第一套纹理,texcoord0
			};

			struct OutVertex {
				float4 pos   : SV_POSITION;//裁剪空间坐标
				float3 color : COLOR0;//颜色
			};
		

			fixed4 _Color;//关联

			OutVertex vert(InVertex v){
				OutVertex o;

			    o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.normal * 0.5 + fixed3(0.5, 0.5, 0.5);
				//return UnityObjectToClipPos(v);
				//POSITION告诉Unity将模型的坐标填入v, SV_POSITION表示输出的是裁剪空间的坐标
				return o;
			}

			fixed4 frag(OutVertex o) : SV_Target{
				//SV_Target表示输出的颜色存储到一个渲染目标当中，这里输出到默认的帧缓存
				fixed3 c = o.color;
			    c *= _Color.rgb;
				return fixed4(c, 1.0);
				//SV --> system value
				//target --> render target
			}

			ENDCG
			/*
			fixed 精度很低 只有-2.0~2.0的精度 在颜色值上可以用这个 只占11个bit
			需要注意的是,vert是逐个顶点调用的,frag	是逐片元调用的
			因此farg的输入实际上是vert输出的插值后的结果
		    */

		}
	}
}
