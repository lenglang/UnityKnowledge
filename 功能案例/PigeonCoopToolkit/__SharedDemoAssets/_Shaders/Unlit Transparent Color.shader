Shader "Pigeon Coop Shaders/Unlit Transparent Color" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,0)
	}
	SubShader {

		Tags {"Queue" = "Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		Pass {
				Color [_Color]
			}
	} 
	FallBack "Diffuse"
}
