using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLColorExample : MonoBehaviour
{
    public Material material = null;
    public int circleCount = 16;
    public int randomSeed = 0;

    const int circleResolution = 64;


    void OnRenderObject()
    {
        material.SetPass(0);

        Random.InitState(randomSeed);
        for (int i = 0; i < circleCount; i++)
        {
            GL.PushMatrix();
            float y = Random.value;
            //X and Y position of circles. X increases with 1, X is a random value between 0 and 1
            GL.MultMatrix(Matrix4x4.Translate(new Vector3(i, y, 0)));
            //calls method GLCircle with radius value of 0.5f
            Color color = Color.HSVToRGB(Random.value, 1, 1);
            GLCircle(0.5f, color);
            GL.PopMatrix();
        }
    }



    void GLCircle(float radius, Color color)
    {
        GL.Begin(GL.TRIANGLE_STRIP);
        GL.Color(color);
        for (int i = 0; i < circleResolution; i = i + 1)
        {
            float t = Mathf.InverseLerp(0, circleResolution - 1, i); // Normalized value of i.         
                                                                     //arc measure?
            float angle = t * Mathf.PI * 2;
            //X and Y 
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;


            GL.Vertex3(x, y, 0);
            GL.Vertex3(0, 0, 0);
        }
        GL.End();
    }
}
