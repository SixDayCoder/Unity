Shader "Custom/SimpleTexture" {
	Properties {
		_MainTex("MainTex", 2D) = "white"{}
		_Color ("Color", Color) = (1,1,1,1)
		_SpecularColor("SpecularColor", Color) = (1, 1, 1, 1)
		_Glossiness("Smoothness", Range(0,1)) = 0.5
	}
	SubShader{
			Pass{
				Tags{"LightMode" = "ForwardBase"}

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "Lighting.cginc"
				#include "UnityCG.cginc"


				fixed4 _Color;
				fixed4 _SpecularColor;
				float  _Glossiness;
				sampler2D _MainTex;
				
				float4 _MainTex_ST;
				/*
				该变量不是任意的,ST是缩放scale和平移translation的缩写
				_MainTex_ST.xy存放的是缩放值
				_MainTex_ST.zw存放的是偏移值
				*/

				struct VertexIn {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 texcoord : TEXCOORD0;
					/*
					TEXCOORD是纹理坐标,会存放到相应的寄存器中
					因为寄存器对齐等原因,使用4个byte存储,和平台有关系
					*/
				};

				struct VertexOut {
					float4 pos : SV_POSITION;
					float3 worldNormal : TEXCOORD0;
					float3 worldPos : TEXCOORD1;
					float2 uv : TEXCOORD2;
					/*
					*/
				};

				VertexOut vert(VertexIn v) {
					VertexOut o;

					o.pos = UnityObjectToClipPos(v.vertex);
					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					o.worldPos =  mul(v.vertex.xyz, unity_ObjectToWorld).xyz;
					o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					//先缩放在平移
					//或者o.uv = TRANSFORM_TEX(v.texcoord,_MainTex)

					return o;
				}

				fixed4 frag(VertexOut o) : SV_Target{


					fixed3 worldNormal = normalize(o.worldNormal);
					fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(o.worldPos));

					//use texture sample the diffuse color
					fixed3 albedo = tex2D(_MainTex, o.uv).rgb * _Color.rgb;
					
					//half lambert
					fixed3 halfLambert = dot(worldNormal, worldLightDir) * 0.5f + 0.5f;
					fixed3 diffuseColor = _LightColor0.rgb * albedo * halfLambert;
					//BlinnPhong
					fixed3 viewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));
					fixed3 halfDir = normalize(worldLightDir + viewDir);
					fixed3 specularColor = _LightColor0.rgb * _SpecularColor.rgb * pow(saturate(dot(halfDir, viewDir)), _Glossiness);

					return fixed4(specularColor + diffuseColor, 1.0);
				}

			ENDCG
		}
	}

	FallBack "Specular"
}
