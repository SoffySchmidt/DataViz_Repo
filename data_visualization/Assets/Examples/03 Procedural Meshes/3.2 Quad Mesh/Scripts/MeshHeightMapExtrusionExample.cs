using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshHeightMapExtrusionExample : MonoBehaviour
{
    public Material material;
    public Texture2D heightmapTexture;
    public Vector2Int resolution = new Vector2Int(32,32);
    public Vector2 size = new Vector2(10, 10);
    public float heightMax = 2010;
    public float heightMin = 456;


    Mesh _mesh;
    // Start is called before the first frame update
    void Awake()
    {
        _mesh = new Mesh();

        int vertexCount = resolution.x * resolution.y;
        int quadCount = (resolution.x - 1) * (resolution.y - 1);
        int quadIndexCount = quadCount * 4;

        Vector3[] vertices = new Vector3[vertexCount];
        int[] quadIndices = new int[quadIndexCount];

        int v = 0; //vertex index
        int i = 0; //QuadIndex index;

        for (int nz = 0; nz < resolution.y; nz++)
        {
            //By normalizing the value, you can reuse this variable for positioning the material 
            //which you might want to place differently instead of on the total size of the mesh

            float tz = Mathf.InverseLerp(0, resolution.y - 1, nz); //Normalized position z (0.0 to 1.0)
            float z = Mathf.Lerp(0, size.y, tz);                   //Scale to size.y
            
            //Making resolution independent of size

            for (int nx = 0; nx < resolution.x; nx++)
            {
                float tx = Mathf.InverseLerp(0, resolution.x - 1, nx); //Normalized position z (0.0 to 1.0)
                float x = Mathf.Lerp(0, size.x, tx); //Scale to size.x
                //Debug.Log (nx + " -> " + tx + " -> " + x);

                Color heightMapColor = heightmapTexture.GetPixelBilinear(tx, tz); // Read interpolated color (between pixels) from texture (normalized values)

                float y = Mathf.Lerp(heightMin, heightMax, heightMapColor.r); // Random.value;

                //float y = heightMapColor.r * heightMax;

                Vector3 position = new Vector3(x, y, z);

                vertices[v] = position;

                //Add quads if we are inside right (x) and depth (z) edge of grid. 
                if (nx < resolution.x - 1 && nz < resolution.y - 1)
                {
                    //i+0
                    quadIndices[i++] = v;                       //Current vertex
                    //i+1   
                    quadIndices[i++] = v + resolution.x;        //Vertex in next row
                    //i+2
                    quadIndices[i++] = v + resolution.x + 1;    //Vertex in new row plus 1    
                    //i+3
                    quadIndices[i++] = v + 1;                   //
                    //i += 4;
                }
                v++; //next vertex index
            }
        }

        _mesh.SetVertices(vertices);
        _mesh.SetIndexBufferParams(quadIndices.Length, UnityEngine.Rendering.IndexFormat.UInt32); //enable large mesh support
        _mesh.SetIndices(quadIndices, MeshTopology.Quads, 0);
        _mesh.RecalculateNormals(); //Let Unity compute the normals of the surface (slow, but ok in Awake).
       
    }

    // Update is called once per frame
    void Update()
    {
        Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, material, gameObject.layer);
    }
}
