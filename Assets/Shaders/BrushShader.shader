﻿Shader "Sprites/Brush"
{
	Properties
	{
		[PreRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_TintColor ("Tint Color", Color) = (1, 1, 1, 1)
		_Flow ("Flow", Range(0, 1)) = 1
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}
	SubShader
	{
		Tags 
		{
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest Off


		// Additive
		// Blend One One

		// Soft Additive
		// Blend OneMinusDstColor One

		// Transparent
		// Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcColor
		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha One


		// Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			static const float COLOR_MIN_STEP = 1.0f / 256.0f;

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _Flow;
			float4 _TintColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.uv);

				float flow = _Flow;
				// if(flow < COLOR_MIN_STEP)
				// 	flow = COLOR_MIN_STEP;

				half4 final = _TintColor;
				
				final.a = col.r * flow;
				
				return final;
			}
			ENDCG
		}
	}
}
