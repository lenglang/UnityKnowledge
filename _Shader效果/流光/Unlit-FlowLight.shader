// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Unlit/FlowLight" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_FlowTex ("FlowTex", 2D) = "black" {}
	_FlowSpeed ("FlowSpeed", vector) = (0, 0, 0, 0)
}

SubShader {
	Tags { "Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent"  }
	LOD 100
	
	Pass {  
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
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
				half2 flowUV   : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _FlowTex;
			float4 _FlowTex_ST;

			half4 _FlowSpeed;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.flowUV = TRANSFORM_TEX(v.texcoord, _FlowTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				fixed4 fcol = tex2D(_FlowTex, i.flowUV + half2(_Time.x * _FlowSpeed.x, _Time.x * _FlowSpeed.y));
				col.rgb += fcol.rgb;
				return col;
			}
		ENDCG
	}
}

}
