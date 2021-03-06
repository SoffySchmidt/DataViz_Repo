﻿ Shader "Custom/05HorizontalGradient"
{

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


			ToFrag Vert( ToVert v )
			{
				ToFrag o;
				o.vertex = UnityObjectToClipPos( v.vertex );
				o.uv = v.uv; //Copy uv to output that will be forwarded to Frag function
				return o;
			}
			
			half4 Frag( ToFrag i ) : SV_Target
			{
				half4 brightness = 1 - i.uv.x;

				//return half4(brightness, brightness, brightness, 1); //Same as half4(i.uv.x, i.uv.y, 0, 1)

				return half4(brightness.xxx,1); //Alternative
			}

			ENDCG
		}
	}
}