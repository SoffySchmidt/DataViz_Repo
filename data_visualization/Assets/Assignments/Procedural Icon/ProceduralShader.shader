Shader "DataVisWorkshop/GLShader"
{
	//where the actual shaders start. Contains vertex- and fragment shader
	SubShader
	{
		
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		
		Pass
		{
		//language for writing shader code
			CGPROGRAM

			//pragma pre-compiler step to tell you the name of the vertex and fragment shader.
			//it could be called whatever, e.g. potato
			#pragma vertex Vert
			#pragma fragment Frag
			
			//helpful for structuring code, positiong of objects, etc.
			//this grabs the CG file inside UnityCG and pastes it into the script
			#include "UnityCG.cginc"
			//Mesh data: vertex position, vertex normal, UVs, tangents, vertex colors
			struct VertexInput
			{
				float4 vertex : POSITION;
				//float4 color : COLOR;
				fixed4 color : COLOR;
			};
			//Fragment 
			struct VertexOutput
			{
				//vertex = clipSpacePos
				float4 vertex : SV_POSITION;
				//float4 color : COLOR;
				fixed4 color : COLOR;
			};

			//v.vertex localspace vertex
			VertexOutput Vert(VertexInput v)
			{
				VertexOutput o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				return o;
			}

			fixed4 Frag(VertexOutput i) : SV_Target
			{
				//float3 clipPos = o.vertex.xyz;
				
				return i.color;
				//RGBA
				//return float4(0,0,1,0);
			}
			ENDCG
		}
	}
}
