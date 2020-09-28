 Shader "Custom/ShaderPattern_2"
{
	 Properties{
		_TileCount("Tile Count", Range(0,100)) = 10

		
		_Color("Color", Color) = (0, 0, 0, 1) //the base color
		_SecondaryColor("Secondary Color", Color) = (1,1,1,1) //the color to blend to
		_Blend("Blend Value", Range(0,1)) = 0.5 //0 is the first color, 1 the second

	 }

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"

			#pragma vertex Vert
			#pragma fragment Frag


			half hash12(half2 p)
			{
				half3 p3 = frac(half3(p.xyx) * .1031);
				p3 += dot(p3, p3.yzx + 33.33);
				return frac((p3.x + p3.y) * p3.z);
			}

			

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

			float _Blend;

			half3 _Color;
			half3 _SecondaryColor;
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
				
				
				const int stepCount = _TileCount;;

				i.uv.x += floor(_Time.y * 10);
				half2 pos = floor(i.uv * stepCount); //Convert to integer steps 
				half result = hash12(pos);

				half brightness = floor(fmod(pos * _TileCount, 2.0));
				half2 col = lerp(_Color, _SecondaryColor, _Blend);

				return half4(result, col, 1);
			}

			ENDCG
		}
	}
}