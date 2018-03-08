Shader "Pigeon Coop Shaders/Diffuse Color Mask" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Mask ("Mask (RGB)", 2D) = "white" {}
        _Color ("Main Color", COLOR) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Unlit 
		#include "UnityCG.cginc" 
 
		half4 LightingUnlit (SurfaceOutput s, half3 lightDir, half atten) {
			half4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

		sampler2D _MainTex;
		sampler2D _Mask;
		float4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 m = tex2D (_Mask, IN.uv_MainTex);

			if(m.r > 0.5)
				o.Albedo = c.rgb;
			else
				o.Albedo = c.rgb * _Color;

			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
