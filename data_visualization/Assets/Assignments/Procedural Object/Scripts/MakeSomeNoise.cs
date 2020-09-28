using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSomeNoise : MonoBehaviour
{
    public float power = 3;
    public float scale = 1;
    public float timeScale = 1;

    private float offsetX, offsetY;
    private MeshFilter mf;

    public float waveCount = 2;
    public float waveAmount = 0.5f;
    public float waveSpeed = 1f;
    public float twistAmount = 0f;
    float waveAngleOffset;

    Vector3[] _deformedVertices;
    Vector3[] vertices;
    Vector3 vertexPosition;

    // Start is called before the first frame update
    void Awake()
    {
        mf = GetComponent<MeshFilter>();
        MakeNoise();


        _deformedVertices = new Vector3[vertices.Length];
    }

    // Update is called once per frame
    void Update()
    {
        MakeNoise();

        //"Animate" offsetX and offSetY numbers over time
        //Every frame the terrain will be moving
        offsetX += Time.deltaTime * timeScale;
        offsetY += Time.deltaTime * timeScale;

       
    }

    void MakeNoise()
    {
        //waveAngleOffset += Time.deltaTime * waveCount;
        //reads and sets vertices equal to the mesh's vertex values

        vertices = mf.mesh.vertices;

        for (int i = 0; i < vertices.Length-1; i++)
        {
            //The vertices' Y coordinates are updated every frame by the values calculated in CalculateHeight * power
            //Causing the mesh's Y coordinates to move up and down each frame 
            //vertices[i].x and vertices[i].z go from -64 to 64. 
            vertices[i].y = CalculateHeight(vertices[i].x, vertices[i].z) * power;

            vertexPosition = vertices[i];
        }


        mf.mesh.vertices = vertices;         //updates mesh's vertex values with the values generated and received in the for-loop
        mf.mesh.RecalculateNormals();
        
    }

    //PERLIN NOISE
    //float Value between 0.0 and 1.0. (Return value might be slightly below 0.0 or beyond 1.0.) 
    // The noise does not contain a completely random value at each point but rather consists of "waves" 
    // whose values gradually increase and decrease across the pattern

   //Returns X and Y values between 0.0 and 1.0
    float CalculateHeight(float x, float y)
    {
        float xCoord = x * scale + offsetX;
        float yCoord = y * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
