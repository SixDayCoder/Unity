// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SpecularVertexLevel" {

	Properties {
		_SpecularColor ("SpecularColor", Color) = (1,1,1,1)
		_DiffuseColor("DiffuseColor", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(8.0 ,256.0)) = 20.0
	}

	SubShader{
			Pass{
				
				Tags{"LightMode" = "ForwardBase"}

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "Lighting.cginc"

				struct VertexIn {
					float4 vertex   : POSITION;
					fixed3 normal   : NORMAL;
				};

				struct VertexOut {
					float4 pos   : SV_POSITION;
					fixed3 color : COLOR;
				};

				fixed3 _SpecularColor;
				fixed3 _DiffuseColor;
				float  _Glossiness;

				VertexOut vert(VertexIn v) {
					VertexOut o;
					o.pos = UnityObjectToClipPos(v.vertex);
					
					fixed3 worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
					fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);

					//diffuse term,half lambert
					fixed3 halfLambert = dot(worldNormal, worldLightDir) * 0.5f + 0.5f;
					fixed3 diffuseColor = _LightColor0 * _DiffuseColor.rgb * halfLambert;
					//specular term
					//reflect(worldLightComeInDir,normal)
					float4 worldPosition = mul(v.vertex, unity_ObjectToWorld);
					fixed3 reflectDir    = normalize(reflect(-worldLightDir, worldNormal));
					fixed3 viewDir       = normalize(_WorldSpaceCameraPos.xyz - worldPosition.xyz);
					fixed3 specularColor = _LightColor0.rgb * _SpecularColor.rgb * pow(saturate(dot(reflectDir, viewDir)), _Glossiness);
					
					o.color = diffuseColor + specularColor;
					return o;

				}

				fixed4 frag(VertexOut o) : SV_Target{
					return fixed4(o.color, 1.0);
				}


				ENDCG
			}
		}
}
