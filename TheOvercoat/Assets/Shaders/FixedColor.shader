// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader ".myShaders/Fixed Unlit"{
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
			//İnformation provided by unity
};
		struct v2f {
			float4 pos : SV_POSITION;
			float2 texcoor: TEXCOORD0;
			//I will transform appdata to these variables
		
		};
		fixed4 _Color;
		sampler2D _MainTexture;
		v2f vert(appdata IN) {
			v2f OUT;
			OUT.pos = UnityObjectToClipPos(IN.vertices);
			OUT.texcoor = IN.texcoor;
			return OUT;
			//i am transfering some how
		}
		fixed4 frag(v2f IN):COLOR
		{
			fixed4 texColor = tex2D(_MainTexture, IN.texcoor);
		    return texColor;
			//return _Color;
			//know i am mathing those information with input of user.

		}
		ENDCG
	}
	}
}