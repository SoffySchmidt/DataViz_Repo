/*
	Copyright © Carl Emil Carlsen 2020
	http://cec.dk
*/

using UnityEngine;

public class GLPrimitivesExample : MonoBehaviour
{
	public Material material;


	void OnRenderObject()
	{
		material.SetPass( 0 );

		GL.wireframe = true;

		// Draw a triangle.
		GL.Begin( GL.TRIANGLES  );
		GL.Vertex3( 0, 0, 0 );
		GL.Vertex3( 0, 1, 0 );
		GL.Vertex3( 1, 1, 0 );
		GL.End();

		// Draw a quad.
		GL.Begin( GL.QUADS );
		GL.Vertex3( 2, 0, 0 );
		GL.Vertex3( 2, 1, 0 );
		GL.Vertex3( 3, 1, 0 );
		GL.Vertex3( 3, 0, 0 );
		GL.End();

		// Draw a continous line.
		GL.Begin( GL.LINE_STRIP );
		GL.Vertex3( 5 + 0.5f, -2, 0 );
		GL.Vertex3( 5 - 0.5f, -1, 0 );
		GL.Vertex3( 5 + 0.5f, 0, 0 );
		GL.Vertex3( 5 - 0.5f, 1, 0 );
		GL.Vertex3( 5 + 0.5f, 2, 0 );
		GL.End();

		// Draw multiple seperated lines in one call.
		GL.Begin( GL.LINES );
		GL.Vertex3( 7 + 0.5f, -2, 0 );
		GL.Vertex3( 7 - 0.5f, -1, 0 );
		GL.Vertex3( 7 + 0.5f, 0, 0 );
		GL.Vertex3( 7 - 0.5f, 1, 0 );
		GL.Vertex3( 7 + 0.5f, 2, 0 );
		GL.End();

		// Draw multiple triangles using jagged positions.
		GL.Begin( GL.TRIANGLE_STRIP );
		GL.Vertex3( 9 + 0.5f, -2, 0 );
		GL.Vertex3( 9 - 0.5f, -1, 0 );
		GL.Vertex3( 9 + 0.5f, 0, 0 );
		GL.Vertex3( 9 - 0.5f, 1, 0 );
		GL.Vertex3( 9 + 0.5f, 2, 0 );
		GL.End();

		GL.wireframe = false;
	}
}