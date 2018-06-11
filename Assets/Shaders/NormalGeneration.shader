Shader "Custom/NormalGeneration" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_DeltaScale("DeltaScale", Range(0,1)) = 0.5 
		_HeightScale("HeightScale", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows

		sampler2D _MainTex;
		struct Input {
			float2 uv_MainTex;
		};

		half _DeltaScale;
		half _HeightScale;
		float2 _MainTex_TexelSize;

		

		float GetGrayColor(float3 color)
		{
    		return color.r * 0.2126 + color.g * 0.7152 + color.b * 0.0722;
		}

		float3 GetNormalByGray(float2 uv)
		{   
    		float2 deltaU = float2(_MainTex_TexelSize.x * _DeltaScale, 0);
    		float h1_u = GetGrayColor(tex2D(_MainTex, uv - deltaU).rgb);
    		float h2_u = GetGrayColor(tex2D(_MainTex, uv + deltaU).rgb);
    		// float3 tangent_u = float3(1, 0, (h2_u - h1_u) / deltaU.x);
    		float3 tangent_u = float3(deltaU.x, 0, (h2_u - h1_u) * _HeightScale);

    		float2 deltaV = float2(0, _MainTex_TexelSize.y * _DeltaScale);
    		float h1_v = GetGrayColor(tex2D(_MainTex, uv - deltaV).rgb);
    		float h2_v = GetGrayColor(tex2D(_MainTex, uv + deltaV).rgb);
    		// float3 tangent_v = float3(0, 1, (h2_v - h1_v) / deltaV.y);
    		float3 tangent_v = float3(0, deltaV.y, (h2_v - h1_v) * _HeightScale);

	    	float3 normal = normalize(cross(tangent_v, tangent_u));
			normal.z *= -1;
			normal = normal * 0.5 + 0.5;
			return normal;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);			
			o.Albedo = GetNormalByGray(IN.uv_MainTex);
			o.Alpha = c.a;
		}


		ENDCG
	}
	FallBack "Diffuse"
}

