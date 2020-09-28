using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Procedural_Icon : MonoBehaviour
{
    public Material material = null;
    public int circleCount = 1;
    const int circleResolution = 64;
    //Color currColor = new Color();
    public float radius = 0.5f;

    public Color color = new Color(1, 0.5f, 1);

    float xCirc, yCirc;



    Vector3 worldPosition;


    public int randomSeed = 0;

    public Color horLeftColor, horRightColor, verBotColor, verTopColor;

    //public Vector4 c;

    [Range(0,1)]
    public float valence = 0.0f;
    [Range(0,1)]
    public float activation = 0.0f;

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
        
            GLCircle(1f, color);
            GL.PopMatrix();
        }
    }

    private void Update()
    {
        Color valCol = Color.Lerp(horLeftColor, horRightColor, valence);
        Color actCol = Color.Lerp(verBotColor, verTopColor, activation);
        color = Color.Lerp(valCol, actCol, 0.5f);


        float mouseRatioX = Input.mousePosition.x / Screen.width;
        float mouseRatioY = Input.mousePosition.y / Screen.height;

        Vector2 center = new Vector2(xCirc, yCirc) / 2;

        //Debug.Log("center: " + center);

        //Vector3 mousePos = new Vector3(mouseRatioX, mouseRatioY, 0f);

        mouseRatioX = Mathf.Lerp(-1, 1, mouseRatioX);
        mouseRatioY = Mathf.Lerp(-1, 1, mouseRatioY);

        //color = new Color(mouseRatioX, mouseRatioY, 0);

        //Debug.Log(mouseRatioX + " " + mouseRatioY);


        //float lerpMouse;
        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = Camera.main.nearClipPlane;
        //worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        //lerpMouse = Mathf.Lerp(mousePos.x, mousePos.y, 1);


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

            xCirc = x;
            yCirc = y;

            GL.Vertex3(x, y, 0);
            GL.Vertex3(0, 0, 0);
        }

        //Debug.Log("x pos: " + xCirc + "y pos: " + yCirc);
        GL.End();
    }


    /*
    if (valence >= 0f)
    {
        currentValue = valence;
        //currentValue = valence;
        //newColor = Color.Lerp(horLeftColor, horRightColor, valence);

        Color valCol = Color.Lerp(horLeftColor, horRightColor, valence);
        color = valCol * currColor;


    }
    if (activation >= 0f)
    {
        currentValue = activation;
        //currentValue = activation;
        //newColor = Color.Lerp(verBotColor, verTopColor, activation);
        Color actCol = Color.Lerp(horLeftColor, horRightColor, activation);
        color = actCol * currColor;


    }
    */
}
