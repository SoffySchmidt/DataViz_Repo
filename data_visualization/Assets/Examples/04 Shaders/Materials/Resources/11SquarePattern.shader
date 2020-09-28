 Shader "Custom/11SquarePatterns"
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
				half brightness = floor(fmod(i.uv.x * _TileCount, 2.0)) + floor(fmod(i.uv.y * _TileCount, 2.0));

				//x: 0,1,0,1, etc.
				//y: 0,1,0,1, etc.
				//forskyder gradient med et offset på 1. 

				return half4(brightness.xxx,1); 
			}

			ENDCG
		}
	}
}