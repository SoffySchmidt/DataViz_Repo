/*
	Copyright © Carl Emil Carlsen 2020
	http://cec.dk
*/

using UnityEngine;

public class GLTranslationRotationScaleExample : MonoBehaviour
{
	public Material material = null;

    public int rectCount = 16;
    public float rectAngle = -45;
    public float rectScale = 2;
    public float rectTranslateXFactor = 1;

    //Matrices are quicker for the PC to execute
	void OnRenderObject()
	{
		material.SetPass( 0 );

        for (int i = 0; i < rectCount; i++)
        {
            //Remember current transformation state.
            GL.PushMatrix();

            //Matrix4x4: 4 columns x 4 rows = 16

            //Transform
            Matrix4x4 transformation = Matrix4x4.Translate(Vector3.right * i * rectTranslateXFactor);
            //Rotate 
            transformation *= Matrix4x4.Rotate(Quaternion.Euler(0, 0, rectAngle));
            //Scale
            transformation *= Matrix4x4.Scale(Vector3.one * rectScale);
            //Translate to center of rect
            transformation *= Matrix4x4.Translate(new Vector3(0.5f, 1, 0));
         

            GL.MultMatrix(transformation);

            //Draw.
            GLRect(0.9f, 2);

            //Reapply last transformation state
            GL.PopMatrix();
        }
	
	}

	void GLRect(float width, float height)
	{
		GL.Begin( GL.QUADS );
		GL.Vertex3( 0, 0, 0 );
		GL.Vertex3( 0, height, 0 );
		GL.Vertex3( width, height, 0 );
		GL.Vertex3( width, 0, 0 );
		GL.End();
	}
}