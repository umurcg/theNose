// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader ".myShaders/Diffuse"{
	Properties{
		_Color("Main Color", Color) = (1,0,0,1)
		_MainTexture("Main Texture", 2D) = "white"{}
	}

	SubShader{
		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		struct appdata {
			float4 vertices : POSITION;
			float2 texcoor: TEXCOORD0;
			float3 normal:NORMAL;
			//İnformation provided by unity
};
		struct v2f {
			float4 pos : SV_POSITION;
			float2 texcoor: TEXCOORD0;
			float3 normal:NORMAL;
			//I will transform appdata to these variables
		
		};
		fixed4 _Color;
		sampler2D _MainTexture;
		float4 _LightColor0;
		
		
		v2f vert(appdata IN) {
			v2f OUT;
			OUT.pos = mul(UNITY_MATRIX_MVP, IN.vertices);
			OUT.texcoor = IN.texcoor;
			OUT.normal = mul(float4(IN.normal, 0.0), unity_ObjectToWorld).xyz; //takes normal of in value and makes it according to world coordinates
			
			return OUT;
			//i am transfering some how
		}
		fixed4 frag(v2f IN):COLOR
		{
			fixed4 texColor = tex2D(_MainTexture, IN.texcoor);
		float3 normalDirection = normalize(IN.normal);
		float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
			float3 diffuse = _LightColor0.rgb*max(0.0, dot(normalDirection, lightDirection));
			return _Color*texColor*float4(diffuse, 1);
		   // return texColor;
			
			//return _Color;
			//know i am mathing those information with input of user.

		}
		ENDCG
	}
	}
}