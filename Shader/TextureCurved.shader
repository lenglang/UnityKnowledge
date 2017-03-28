// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/TextureCurved" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_QOffset("Offset", Vector) = (0,0,0,0)
	_Color("Main Color", Color) = (1,1,1,1)
	_Brightness("Brightness", Float) = 0.0
	_Dist("Distance", Float) = 100.0
	_FogColor("FogColor", Color) = (0.87,0.87,0.87,1)
	_FogDensity("FogDensity", Float) = 0.1
	_FogRange("FogRange", Float) = 300
	_Cut("Cut", range(0,1)) = 0
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100
	cull off
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _QOffset;
			float _Dist;
			float _Insensitive;
			float _Brightness;
			float4 _Color;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				float4 vPos = mul(UNITY_MATRIX_MV, v.vertex);

				float xsmeh = sin(_WorldSpaceCameraPos.z / 120);

				float zOff = vPos.z / _Dist;

				vPos += _QOffset*zOff*zOff;

				v.vertex = mul(transpose(UNITY_MATRIX_IT_MV), vPos);

#if SHADER_API_GLES
				v.vertex.xyz *= 1.0;
#endif

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				UNITY_APPLY_FOG(i.fogCoord, col);
				UNITY_OPAQUE_ALPHA(col.a);
				return col;
			}
		ENDCG
	}
}

}
