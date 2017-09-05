Shader "Custom/NormalMapTagentSpace" {
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
			Tags{"LightingMode" = "ForwardBase"}

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
			实现在切线空间下计算光照,思路是
			1.在fragment shader中采样Normal Texture获得切空间下的法线
			2.之后将光照、视角信息变换到切空间下
			3.计算光照
			*/

			struct VertexIn {
				float4 vertex : POSITION;
				float3 normal : NORMAL;//需要用法线和切线构造切空间
				float4 tangent : TANGENT;//顶点的切线方向填充到这里,注意是float4,tangent.w决定副切线的方向性
				float4 texcoord : TEXCOORD0;//纹理
			};

			struct VertexOut {
				float4 pos : SV_POSITION;
				float4 uv  : TEXCOORD0;
				float3 lightDir : TEXCOORD1;
				float3 viewDir  : TEXCOORD2;
			};

			VertexOut vert(VertexIn v) {
				VertexOut o;

				o.pos = UnityObjectToClipPos(v.vertex);

				//获取纹理 
				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy   + _MainTex_ST.zw;
				o.uv.zw = v.texcoord.xy * _NormalTex_ST.xy + _NormalTex_ST.zw;

				//计算副切线
				float3 binormal = cross(normalize(v.normal), normalize(v.tangent.xyz)) * v.tangent.w;//使用w分量明确方向性

				//构建从模型空间到切线空间的变换矩阵
				float3x3 transform = float3x3(v.tangent.xyz, binormal, v.normal);

				//将光照、视角从模型空间变换到切线空间
				o.lightDir = mul(transform, ObjSpaceLightDir(v.vertex)).xyz;
				o.viewDir = mul(transform, ObjSpaceViewDir(v.vertex)).xyz;
				//为什么是右乘?变换矩阵怎么求得的 
				//右乘是因为transform的构造方式

				return o;

			}

			fixed4 frag(VertexOut o) : SV_Target {
				fixed3 tangentLightDir = normalize(o.lightDir);
				fixed3 tangentViewDir = normalize(o.viewDir);

				//法线贴图采样
				fixed4 packedNormal = tex2D(_NormalTex, o.uv.zw);
				fixed3 tangentNormal;

				tangentNormal.xy = (packedNormal.xy * 2 - 1) * _BumpScale;
				tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy, tangentNormal.xy)));
				//切空间的法线是归一化的,得到xy后可利用sqrt(1-x^2-y^2)得到z分量

				//纹理贴图采样 
				fixed3 albedo = tex2D(_MainTex, o.uv.xy).rgb * _Color.rgb;

				//diffuse term,half lambert
				fixed3 halfLambert = dot(tangentNormal, tangentLightDir) * 0.5f + 0.5f;
				fixed3 diffuseColor = _LightColor0.rgb * albedo * halfLambert;
				//specular term,blinn phong
				fixed3 halfDir = normalize(tangentLightDir + tangentViewDir);
				fixed3 specularColor = _LightColor0.rgb * _SpecularColor.rgb * pow(saturate(dot(halfDir, tangentViewDir)), _Gloss);

				fixed3 color = diffuseColor + specularColor;
				return fixed4(color, 1.0);
			}

			ENDCG
		}
	}
}
