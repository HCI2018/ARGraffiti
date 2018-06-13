Shader "Sprites/Brush"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
//		[PreRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_TintColor ("Tint Color", Color) = (1, 1, 1, 1)
		_Flow ("Flow", Range(0, 1)) = 1
//		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}
	SubShader
	{
		Tags 
		{
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			// "PreviewType"="Plane"
			//"CanUseSpriteAtlas"="True"
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
			#pragma multi_compile_instancing

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			static const float COLOR_MIN_STEP = 1.0f / 256.0f;

			sampler2D _MainTex;
			float4 _MainTex_ST;

			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(float4, _TintColor);
				UNITY_DEFINE_INSTANCED_PROP(float, _Flow);
			UNITY_INSTANCING_BUFFER_END(Props)

			
			v2f vert (appdata v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);


				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);

				half4 col = tex2D(_MainTex, i.uv);

				float flow = UNITY_ACCESS_INSTANCED_PROP(Props, _Flow);
				// if(flow < COLOR_MIN_STEP)
				// 	flow = COLOR_MIN_STEP;

				half4 final = UNITY_ACCESS_INSTANCED_PROP(Props, _TintColor);
				
				final.a = col.r * flow;
				
				return final;
			}
			ENDCG
		}
	}
}
