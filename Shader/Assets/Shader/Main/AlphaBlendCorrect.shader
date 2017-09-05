Shader "Custom/AlphaBlendCorrect" {
	Properties{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Main Texture", 2D) = "white"{}
	_AlphaScale("Alpha Scale", Range(0,1)) = 0.5
	}

	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }


		Pass{
			ZWrite On
			ColorMask 0
			/*
			新添加的Pass仅仅将模型的深度信息正确写入到深度缓冲中,从而剔除模型中被自身遮挡的片元
			ColorMask RGB | A | 0
			设置颜色通道的写掩码,为0时意味着不写入任何颜色通道,即不会输出任何颜色
			经过该Pass,得到了逐像素的正确的深度信息
			*/
		}

		Pass{
			Tags{ "LightMode" = "Forwardbase" }
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Lighting.cginc"

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _AlphaScale;

			struct VertexIn {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct VertexOut {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldPos : TECOORD1;
				float3 worldNormal : TEXCOORD2;
			};

			VertexOut vert(VertexIn v) {
				VertexOut o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(v.vertex, unity_ObjectToWorld).xyz;
				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;

				return o;
			}

			fixed4 frag(VertexOut o) : SV_Target{
				fixed3 worldNormal = normalize(o.worldNormal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(o.worldPos));
				fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));

				fixed4 texColor = tex2D(_MainTex, o.uv);
				fixed3 albedo = texColor.rgb * _Color.rgb;

				//ambient term
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				//diffuse term, half lambert
				fixed3 halfLambert = dot(worldNormal, worldLightDir) * 0.5f + 0.5f;
				fixed3 diffuseColor = _LightColor0.rgb * albedo * halfLambert;

				return fixed4(ambient + diffuseColor, texColor.a * _AlphaScale);//这里不同,只有开启了Blend才有意义
			}

			ENDCG
		}
	}
}
/*

*/