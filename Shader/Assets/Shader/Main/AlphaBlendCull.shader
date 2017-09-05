Shader "Custom/AlphaBlendCull" {
	Properties{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Main Texture", 2D) = "white"{}
	_AlphaScale("Alpha Scale", Range(0,1)) = 0.5
	}

	/*
	两次渲染,先渲染背面
	再渲染正面
	*/
	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }


		Pass{
			Tags{ "LightMode" = "Forwardbase" }
			Cull Front 
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

		Pass{
			Tags{ "LightMode" = "Forwardbase" }
			Cull Back
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
