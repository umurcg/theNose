Shader "CookbookShaders/Transparent" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

	}
	SubShader {
		//For transparency we should add some tags
		Tags { 
		"Queue"="Transparent" //This force computer to draw this material after frawing solid materials. We use it for transparenct
		"IgnoreProjector"="True"  // I dont know
		"RenderType"="Transparent" //It is type of render obviously
		}

		//This make sure back of geometry wont be drawn
		Cull Back
		
		LOD 200
		

		CGPROGRAM
		// Tell the shader is transparent and needs to blended with that was drawn on the screen before
		//#pragma surface surf Standard fullforwardshadows
		#pragma surface surf Standard alpha
	
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	//FallBack "Diffuse"

}
