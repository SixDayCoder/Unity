Shader "Custom/NormalMapWorldSpace" {
	Properties{
		_MainTex("Texture", 2D) = "white"{}
		_NormalTex("Normal Texture", 2D) = "bump"{}
		_Color("Color", Color) = (1, 1, 1, 1)
		_BumpScale("Bump Scale", Float) = 1.0
		_SpecularColor("Specular Color", Color) = (1, 1, 1, 1)
		_Gloss("Gloss", Range(8.0, 256)) = 20
	}

	SubShader{
		Pass{
			Tags{ "LightingMode" = "ForwardBase" }

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "Lighting.cginc"

			fixed4 _Color;
			fixed4 _SpecularColor;
			float4 _MainTex_ST;
			float4 _NormalTex_ST;
			float  _Gloss;
			float  _BumpScale;
			sampler2D _MainTex;
			sampler2D _NormalTex;
			/*
			实现在世界空间下计算光照,思路是
			1.vertex shader记录下从切空间到世界空间的变换矩阵
			2.fragment中计算光照
			*/

			struct VertexIn {
				float4 vertex   : POSITION;
				float3 normal   : NORMAL;//需要用法线和切线构造切空间
				float4 tangent  : TANGENT;//顶点的切线方向填充到这里,注意是float4,tangent.w决定副切线的方向性
				float4 texcoord : TEXCOORD0;//纹理
			};

			struct VertexOut {
				float4 pos : SV_POSITION;
				float4 uv  : TEXCOORD0;
				float4 T2W0 : TEXCOORD1;
				float4 T2W1 : TEXCOORD2;
				float4 T2W2 : TEXCOORD3;
				/*
				T2Wi -> tangent to world矩阵的分量 一个寄存器只能存32位所以把矩阵拆开存
				为了充分利用float4  每个分量的w记录世界坐标
				*/
			};

			VertexOut vert(VertexIn v) {
				VertexOut o;

				o.pos = UnityObjectToClipPos(v.vertex);

				//获取纹理 
				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uv.zw = v.texcoord.xy * _NormalTex_ST.xy + _NormalTex_ST.zw;
				
				//计算世界坐标系下的normal tangent binormal  构造变换矩阵
				float3 worldPos = mul(v.vertex, unity_ObjectToWorld).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);//变换为worldNormal
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);//把方向矢量从模型空间转换到世界空间中
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

				//构造矩阵
				o.T2W0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
				o.T2W1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
				o.T2W2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);
	
				return o;

			}

			fixed4 frag(VertexOut o) : SV_Target{
				float3 worldPos = float3(o.T2W0.w, o.T2W1.w, o.T2W2.w);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(worldPos));
				fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));

				//法线贴图采样,获取切线空间的normal
				fixed4 packedNormal = tex2D(_NormalTex, o.uv.zw);
				fixed3 tangentNormal;

				tangentNormal.xy = (packedNormal.xy * 2 - 1) * _BumpScale;
				tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy, tangentNormal.xy)));
				//切空间的法线是归一化的,得到xy后可利用sqrt(1-x^2-y^2)得到z分量

				//切空间变换到世界空间 
				fixed3 worldNormal = normalize( half3(dot(o.T2W0.xyz, tangentNormal),
											          dot(o.T2W1.xyz, tangentNormal),
												      dot(o.T2W2.xyz, tangentNormal)) );

				//纹理贴图采样 
				fixed3 albedo = tex2D(_MainTex, o.uv.xy).rgb * _Color.rgb;

				//diffuse term,half lambert
				fixed3 halfLambert = dot(worldNormal, worldLightDir) * 0.5f + 0.5f;
				fixed3 diffuseColor = _LightColor0.rgb * albedo * halfLambert;
				//specular term,blinn phong
				fixed3 halfDir = normalize(worldLightDir + worldViewDir);
				fixed3 specularColor = _LightColor0.rgb * _SpecularColor.rgb * pow(saturate(dot(halfDir, worldViewDir)), _Gloss);

				fixed3 color = diffuseColor + specularColor;
				return fixed4(color, 1.0);
			}

		ENDCG
		}
	}
}
