Shader "Custom/Melt" {
    Properties {
        _MainTex("Main Tex", 2D) = "white" {}
        _Noise("Noise Tex", 2D) = "white" {}
        _MinAlpha("Min Alpha", Range(0, 1)) = 0.0
    }

    SubShader {    
        Pass {
            CGPROGRAM 

            #pragma vertex vert 
            #pragma fragment frag 

            #include "UnityCG.cginc"

            sampler2D _Noise;
            sampler2D _MainTex;
            float _MinAlpha;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f 
            {
                float4 position : SV_POSITION;
                float2 texcoord : TEXCOORD1;
            };

            v2f vert(appdata input) {
                v2f o;
                o.position = mul(UNITY_MATRIX_MVP, input.vertex);
                o.texcoord = input.texcoord;
                return o;
            }

            float4 frag(v2f input) : SV_Target {
                float4 mainColor = tex2D(_MainTex, input.texcoord);
                float4 noiseColor = tex2D(_Noise, input.texcoord);
                float alpha = noiseColor.r - _MinAlpha;
                clip(alpha);
                if(alpha < 0.05)
                    return float4(1,0.5,0,0) * alpha / 0.05;
                else
                    return mainColor / 2;//除以2只是为了暗一点
            }

            ENDCG
        }
    }
}