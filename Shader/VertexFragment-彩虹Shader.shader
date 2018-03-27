// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/VertexFragment" {
	Properties
	{
		_myTexture("Texture", 2D) = "white" {}
	_myColor("Color", Color) = (1,1,1,1)
		_Outline("Outline", Range(0,1)) = 0.04
	}

		SubShader
	{
		Tags{ "Queue" = "Geometry"  "RenderType" = "Opaque" "IgnoreProjector" = "True" }
		// 第一个通道
		Pass
	{
		CGPROGRAM
		// 声明顶点shader函数
#pragma vertex vert

		// 声明片段shader函数
#pragma fragment frag

		// 使用Vertex and Fragment的CG时
		// 会#include "UnityCG.cginc",用到里面的很多函数
#include "UnityCG.cginc"

		sampler2D _myTexture;
	float4    _myColor;

	struct v2f {
		float4 pos:SV_POSITION;
		float3 color :COLOR;
	};
	// appdata_full v是"UnityCG.cginc"里的结构体
	v2f vert(appdata_full v)
	{
		v2f o;
		// UNITY_MATRIX_MVP 当前模型视图投影矩阵
		o.pos = UnityObjectToClipPos(v.vertex);
		// 彩虹色
		o.color = v.vertex * 0.8 + 0.5;
		// 纯色
		//				o.color = v.normal * 0.4 + 0.5;

		return o;
	}

	float4 frag(v2f i) :COLOR
	{
		return float4(i.color,1);
	}


		ENDCG
	}
		// 第二个通道
		Pass{
		Tags{ "LightMode" = "ForwardBase" }

		Cull Front
		Lighting Off
		ZWrite On

		CGPROGRAM

#pragma vertex vert  
#pragma fragment frag  

#pragma multi_compile_fwdbase  

#include "UnityCG.cginc"  

		float _Outline;

	struct a2v
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float4 pos : POSITION;
	};

	v2f vert(a2v v)
	{
		v2f o;

		float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
		float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
		// 轮廓黑线的宽度
		pos = pos + float4(normalize(normal),0) * _Outline;

		o.pos = mul(UNITY_MATRIX_P, pos);

		return o;
	}

	float4 frag(v2f i) : COLOR
	{
		return float4(0, 0, 0, 1);
	}

		ENDCG
	}
	}

		FallBack "Diffuse"
}
