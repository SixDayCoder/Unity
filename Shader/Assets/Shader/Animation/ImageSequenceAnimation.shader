Shader "Custom/ImageSequenceAnimation" {
	Properties{
		_Color("Color Tint", Color) = (1, 1, 1, 1)
		_MainTex("Image Source", 2D) = "white"{}
		_HorizontalAmount("Horizontal Amount", Float) = 8
		_VerticalAmount("Vertical Amount", Float) = 8
		/*
	     水平方向和竖直方向上关键帧图像的个数
		*/
		_Speed("Speed", Range(1, 100)) = 30
	}

	//帧序列动画往往是透明纹理,需要设置Pass的相关状态
	SubShader{
		Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		Pass{
			Tags{"LightMode" = "ForwardBase"}

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			//透明度混合

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			fixed4 _Color;
			float4 _MainTex_ST;
			sampler2D _MainTex;
			float _HorizontalAmount;
			float _VerticalAmount;
			float _Speed;

			struct VertexIn {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct VertexOut {
				float4 pos : SV_POSITION;
				float2 uv  : TEXCOORD0;
			};

			VertexOut vert(VertexIn v) {
				VertexOut o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				return o;
			}

			fixed4 frag(VertexOut o) : SV_Target{

				float time = floor(_Time.y * _Speed);
				float row = floor(time / _HorizontalAmount); // 商做行索引
				float col = time - row * _VerticalAmount; //余数做列索引

				half2 uv = o.uv + half2(col, -row);
				uv.x /= _HorizontalAmount;
				uv.y /= _VerticalAmount;

				fixed4 c = tex2D(_MainTex, uv);
				c.rgb *= _Color;

				return c;
			}

			ENDCG

		}
	}


}

/*
_Time  float4 记录了该场景加载到现在的时间 (t/20, t, 2t, 3t)
_SinTime float4 记录了sin(time)  (t/8, t/4, t/2, t)
_CosTime float4 记录了...
_unity_DeltaTime float4 (deltatime, 1/deltatime,smooth deltatime, 1/smooth deltatime) 
*/