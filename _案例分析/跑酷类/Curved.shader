//#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'
Shader "Custom/Curved" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_QOffset ("Offset", Vector) = (0,0,0,0)
		_Color ("Main Color", Color) = (1,1,1,1)
		_Brightness ("Brightness", Float) = 0.0
		_Dist ("Distance", Float) = 100.0
		_FogColor("FogColor", Color) = (0.87,0.87,0.87,1)
		_FogDensity("FogDensity", Float) = 0.1
		_FogRange("FogRange", Float) = 300
		_Cut("Cut", range(0,1)) = 0

	}
	
	SubShader {
		Tags {"RenderType"="TransparentCutOut"}
		//blend srcalpha oneminussrcalpha
		Cull off

		zwrite on

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert alphatest:_Cut
		struct Input 
		{
			 float2 uv_MainTex;
		};
      
		sampler2D _MainTex;
		float4 _QOffset;
		float _Dist;
		float _Insensitive;
		float _Brightness;
		float4 _Color;
		
		void vert (inout appdata_full v) 
		{
          
			float4 vPos = mul (UNITY_MATRIX_MV, v.vertex);
					    
			float xsmeh=sin(_WorldSpaceCameraPos.z/120);
	 			    
			float zOff = vPos.z/_Dist;

			vPos += _QOffset*zOff*zOff;
	        
			v.vertex = mul(transpose(UNITY_MATRIX_IT_MV), vPos);
			#if SHADER_API_GLES
	    		v.vertex.xyz*= 1.0;
			#endif
		}
      
		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Brightness * _Color;
			float alpha = tex2D (_MainTex, IN.uv_MainTex).a;
			o.Alpha = alpha;  
		}

		ENDCG
	}
	
	Fallback "Diffuse"
}
