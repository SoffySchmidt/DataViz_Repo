 Shader "Custom/10Checkers"
{
	 Properties{
		 _TileCount("Tile Count", Range(0,100)) = 10
	 }

	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex Vert
			#pragma fragment Frag
		
			struct ToVert
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0; //Receive UV set 0 
			};
			
			struct ToFrag
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0; 
			};

			int _TileCount;

			ToFrag Vert( ToVert v )
			{
				ToFrag o;
				o.vertex = UnityObjectToClipPos( v.vertex );
				o.uv = v.uv; //Copy uv to output that will be forwarded to Frag function
				return o;
			}
			
			half4 Frag( ToFrag i ) : SV_Target
			{
				int gradientCount = _TileCount;
				float offset = floor(fmod(i.uv.y * gradientCount, 2.0));				//i.uv.y runs from 0 - 1, scale it up, run up to 2. 
																						//Create and alternating offset along the v (y) axis that is either 0.0 or 1.0
				half gradientValue = fmod(i.uv.x * gradientCount + offset, 2.0);		//Repeat when we hit 2.0 (x modulo 2.0)
				half brightness = floor(gradientValue);									//"Rund ned", 0.5 bliver 0 og 1.5 bliver 1.0

				//x: 0,1,0,1, etc.
				//y: 0,1,0,1, etc.
				//forskyder gradient med et offset på 1. 

				return half4(brightness.xxx,1); 
			}

			ENDCG
		}
	}
}