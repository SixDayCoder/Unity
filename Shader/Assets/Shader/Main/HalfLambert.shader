Shader "Custom/HalfLambert" {
	Properties{
		_DiffuseColor("Diffuse Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}

		SubShader{
		Pass{
			Tags{ "LightMode" = "ForwardBase" }
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
				float4 pos         : SV_POSITION;
				fixed3 worldNormal : TEXCOORD0;
			};

			VertexOut vert(VertexIn v) {
				VertexOut o;
				o.pos         = UnityObjectToClipPos(v.vertex);
				o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);	
				return o;
			}


			fixed4 frag(VertexOut o) : SV_Target{
				fixed3 normal       = normalize(o.worldNormal);
				fixed3 worldLight   = normalize(_WorldSpaceLightPos0.xyz);//标准化光源的方向
				fixed3 halfLambert = dot(normal, worldLight) * 0.5f + 0.5f;
				fixed3 diffuse      = _LightColor0.rgb * _DiffuseColor.rgb * halfLambert;
				return fixed4(diffuse, 1.0);
			}
			ENDCG
		}
	}
}
/*
为什么要使用半Lambert漫反射模型?
传统的Lambert漫反射模型
color = lightColor * diffuseColor * max(0, dot(normal,lightDir))
有个问题，那就是背对我们的所有点的color结果都是(0,0,0),没有任何明暗变化,背光区域看起来像是平面。

改进的半Lambert算法改进了公式
color = lightColor * diffuseColor * ( A * dot(normal, lightDir) + B)
通常A = 0.5 B = 0.5
这样把dot(normal,lightDir)的值域从[-1,1]映射到了[0,1],原先所有背对我们的结果都是0,现在有了变化
这个模型无任何物理依据,仅仅是视觉效果不同
*/