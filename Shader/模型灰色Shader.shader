// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Gray_Egg" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Indentity ("Indentity", float) = 1
	_Power("Power", float) = 1
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Indentity;
			fixed _Power;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				fixed d = Luminance (col.rgb);
				fixed4 mi = fixed4 (d, d, d, 1);
				return (col * (1 - _Indentity) + mi * _Indentity) * _Power;
			}
		ENDCG
	}
}

}
