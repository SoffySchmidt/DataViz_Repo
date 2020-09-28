using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This attribute automatically creates and adds the components "MeshFilter" and "MeshRenderer" to th object
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class ProceduralObjectExample : MonoBehaviour
{
    Mesh _mesh;

    private Vector3[] vertices;

    public int xSize, ySize, zSize;

    void Awake()
    {
       Generate();
    }


    void Generate()
    {
  

        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _mesh.name = "Procedural Grid";

        vertices = new Vector3[(xSize + 1) * (ySize + 1) * (zSize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];     
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                for (int z = 0; i <= zSize; z++, i++)
                {
                    vertices[i] = new Vector3(x, y, z);
                    uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                    tangents[i] = tangent;

                    Debug.Log("i: " + i + "x: " + x + "y: " + y + "z: " + z);
                }
  
            }
        }
     

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.tangents = tangents;


        int[] triangles = new int[xSize * ySize * zSize * 10];

        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 10, vi++)
            {
                for (int z = 0; z < zSize; z++, ti += 10, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                    triangles[ti + 5] = vi + xSize + 2;

                  

                }

            }
        }
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();


    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

}
