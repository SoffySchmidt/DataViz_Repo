using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlaneGen : MonoBehaviour
{
    public float size = 16;
    public int gridSize = 16;

    MeshFilter filter;

    void Awake()
    {
        filter = GetComponent<MeshFilter>();
        filter.mesh = GenerateMesh();
    }

    private Mesh GenerateMesh()
    {
        Mesh m = new Mesh();

        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>(); //flat plane, just stores 2 values (x,z)

        //Generate vertices, normals and uvs
        for (int x = 0; x < gridSize+1; x++)
        {
            for (int y = 0; y < gridSize + 1; y++)
            {
                //Calculates the X and Y position of our grid
                //It starts in the top corner and iterates over the Xs and Ys
                //For each of the points it is going to walk throuhg 
                //First it goes down the Y every step


                //It takes the x and divides it by the gridSize. This gives us an fraction of how far accross the plane we are - multiplied by the size
                //The total will only ever equal the total size we want it to be.
                //we will eventually cross the entire size and equal that
                vertices.Add(new Vector3(-size * 0.5f + size * (x / ((float)gridSize)), 0, -size * 0.5f + size * (y / (float)gridSize)));

                //normal points point upwards (y-axis)
                //normals.Add(new Vector3(0,1,0.5f));

                normals.Add(Vector3.up);


                uvs.Add(new Vector2(x / (float)gridSize, y / (float)gridSize));
            }
        }

        //take four vertex points and split them into two triangles
        //first triangle: (0,1,3)
        //second triangle: (1, 2, 3)

        var triangles = new List<int>();

        //the Vertex Count is always ONE value bigger than the gridSize
        var vertCount = gridSize + 1;

        //For every fourth vertices, we are going to have two triangles 
        //theoretically this is not going to iterate over the right side or the bottom side
        for (int i = 0; i < vertCount * vertCount - vertCount; i++)
        {
            //to avoid triangles to be generated from the bottom to the top
            //we want each squares of triangles to be below and to the right
            if((i+1) % vertCount == 0)
            {
                //if the remainder of the modulus operator is 0, continue function
                continue;
            }

            //e.g. 1 + 1 + 17| 1 + 17 | 1 | 1 | 1 + 1 | 1 + 17 + 1 
            //= 19, 18, 1, 1, 2, 19

            triangles.AddRange(new List<int>(){
                //first triangle e.g. 3, 1, 0
                i + 1 + vertCount, i + vertCount, i,
                //second triangle e.g. 1, 2, 3
                i, i + 1, i + vertCount + 1
            });

        }

        m.SetVertices(vertices);
        m.SetNormals(normals);
        m.SetUVs(0, uvs);
        m.SetTriangles(triangles, 0); 

        //return mesh
        return m;
    }
}
