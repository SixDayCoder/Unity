Shader "Custom/BlinnPhong" {
	Properties{
		_SpecularColor("SpecularColor", Color) = (1,1,1,1)
		_DiffuseColor("DiffuseColor", Color) = (1,1,1,1)
		_Glossiness("Smoothness", Range(8.0 ,256.0)) = 20.0
	}

	SubShader{
		Pass{

			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Lighting.cginc"

			struct VertexIn {
				float4 vertex   : POSITION;
				fixed3 normal : NORMAL;
			};

			struct VertexOut {
				float4 pos         : SV_POSITION;
				float3 worldPos    : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
			};

			fixed3 _SpecularColor;
			fixed3 _DiffuseColor;
			float  _Glossiness;

			VertexOut vert(VertexIn v) {
				VertexOut o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(v.vertex, unity_ObjectToWorld);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				return o;

			}

			fixed4 frag(VertexOut o) : SV_Target{

				fixed3 worldNormal = normalize(o.worldNormal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(o.worldPos));

				//diffuse term,half lambert
				fixed3 halfLambert = dot(worldNormal, worldLightDir) * 0.5f + 0.5f;
				fixed3 diffuseColor = _LightColor0.rgb * _DiffuseColor.rgb * halfLambert;
				//specular term
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));
				fixed3 halfDir = normalize(worldLightDir + viewDir);
				fixed3 specularColor = _LightColor0.rgb * _SpecularColor.rgb * pow(saturate(dot(halfDir, viewDir)), _Glossiness);
				
				fixed3 color  = diffuseColor + specularColor;

				return fixed4(color, 1.0);
			}


			ENDCG
		}
	}
}
/*
传统的Phong高光反射模型
specularColor = lightColor * specularColor * pow( max(dot(viewDir,reflectDir),0), glossiness)
改进后的BlinnPhong模型不再使用反射向量,性能更好些,效果也差不多
而是引入了一个新的向量 h = normalize(viewDir + lightDir)
specularColor = lightColor * specularColor * pow( max(dot(h,normal),0), glossiness)
*/