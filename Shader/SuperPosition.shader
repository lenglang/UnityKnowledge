Shader "Unlit/SuperPosition" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DecalTex ("Decal (RGBA)", 2D) = "black" {}
		_WaterTex ("Water (RGBA)", 2D) = "black" {}
		_Color ("Color", color) = (1, 1, 1, 0)
		_DecalColor ("Decal Color", color) = (1, 1, 1, 0)
		_WaterColor("Water Color", color) = (1, 1, 1, 0)
		_Stencil ("Stencil ID", float) = 0
	}
	SubShader {
		Stencil {
            Ref [_Stencil]
            Comp NotEqual
            Pass Keep
        }
		Tags { "RenderType"="Opaque" "Queue"="Geometry+2" "IgnoreProjector"="true" }
		LOD 200
		
		Pass {  
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					float2 texcoord1 : TEXCOORD1;
					float3 normal : NORMAL;
				};
					
				struct v2f {
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					half2 dtexcoord : TEXCOORD1;
					half2 wtexcoord : TEXCOORD2;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _DecalTex;
				float4 _DecalTex_ST;

				sampler2D _WaterTex;

				half4 _Color;
				fixed4 _DecalColor;
				fixed4 _WaterColor;
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.dtexcoord = TRANSFORM_TEX(v.texcoord1, _DecalTex);
					float3 nw = mul((float3x3)UNITY_MATRIX_IT_MV, SCALED_NORMAL);
					o.wtexcoord.x = nw.x / 2 + 0.5;
					o.wtexcoord.y = nw.y / 2 + 0.5;
					return o;
				}
				
				fixed4 frag (v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.texcoord);
					fixed4 det = tex2D(_DecalTex, i.texcoord);
					fixed4 wte = tex2D(_WaterTex, i.texcoord);
					col.rgb = lerp(col.rgb, det.rgb * _DecalColor.rgb + wte.rgb * _WaterColor.rgb, det.a * _DecalColor.a + wte.a * _WaterColor.a);
					return col + _Color;
				}
			ENDCG
		}
	} 
}
