 Shader "Custom/09HorizontalGradientPalette"
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
				int stepCount = 10;
				half brightness = floor(i.uv.x * stepCount) / stepCount; //"Rund ned", 0.5 bliver 0 og 1.5 bliver 1.0
				return half4(brightness.xxx,1); 
			}

			ENDCG
		}
	}
}