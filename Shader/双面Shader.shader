Shader "Nature/Vegitation Vertex Lit" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,0)
		_SpecColor("Spec Color", Color) = (1,1,1,1)
		_Emission("Emmisive Color", Color) = (0,0,0,0)
		_Shininess("Shininess", Range(0.01, 1)) = 0.7
		_FrontTex("Front (RGB)", 2D) = "white" { }
	_BackTex("Back (RGB)", 2D) = "white" { }
	}
		SubShader{
		Material{
		Diffuse[_Color]
		Ambient[_Color]
		Shininess[_Shininess]
		Specular[_SpecColor]
		Emission[_Emission]
	}
		Lighting On
		SeparateSpecular On
		Blend SrcAlpha OneMinusSrcAlpha
		Pass{
		Cull Front
		SetTexture[_BackTex]{
		Combine Primary * Texture
	}
	}
		Pass{
		Cull Back
		SetTexture[_FrontTex]{
		Combine Primary * Texture
	} } }}