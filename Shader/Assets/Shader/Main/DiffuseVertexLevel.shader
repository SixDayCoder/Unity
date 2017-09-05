// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/DiffuseVertexLevel" {
	Properties{
		_DiffuseColor("Diffuse Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}

		SubShader{
			Pass{
				Tags{"LightMode" = "ForwardBase"}
				//只有正确定义了LightMode才能使用Unity内置的一些光照变量

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "Lighting.cginc"

				fixed4 _DiffuseColor;
				
				struct VertexIn {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
				};

				struct VertexOut {
					float4 pos   : SV_POSITION;
					fixed3 color : COLOR;
				};

				VertexOut vert(VertexIn v) {
					VertexOut o;
					o.pos = UnityObjectToClipPos(v.vertex);

					//手动计算漫反射光
					//fixed3 ambient     = UNITY_LIGHTMODE_AMBIENT.xyz;//获取环境光
					fixed3 worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));//模型空间法线变换到世界空间法线
					//?
					//worldNormal  = normal * objectToWorld.inverse().transpose() 
					//若objectToWorld正交 a.transpose() = a.inverse()
					//objectToWorld.inverse().transpose() = objectToWorld.transpose().transpose() = objectToWorld
					fixed3 worldLight  = normalize(_WorldSpaceLightPos0.xyz);//标准化光源的方向
					fixed3 diffuse     = _LightColor0.rgb * _DiffuseColor.rgb * saturate(dot(worldNormal, worldLight));

					o.color = diffuse;
					return o;
				}


				fixed4 frag(VertexOut o) : SV_Target{
					return fixed4(o.color, 1.0);
				}
				ENDCG
		}
	}
}
