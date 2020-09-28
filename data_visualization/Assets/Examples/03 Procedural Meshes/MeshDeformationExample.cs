using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformationExample : MonoBehaviour
{
    Mesh _mesh;
    public Material material;
    public Mesh originalMesh;
    Vector3[] _vertices;
    Vector3[] _deformedVertices;

    public float waveCount = 2;  //Number of waves per Unity unit
    //Multiply value to determine the wave's "size on the y and x" axis. 
    //By default the Sin() wave would go from -1 to 1
    public float waveAmount = 0.5f;
    public float waveSpeed = 1f;
    public float twistAmount = 0f;
    float waveAngleOffset;


    private void Awake()
    {
       _vertices = originalMesh.vertices;
       int[] triangleIndicies = originalMesh.triangles;

        _mesh = new Mesh();                  //create mesh
        _mesh.vertices = _vertices;           //set mesh's vertices to the bunny's vertices
        _mesh.triangles = triangleIndicies;  //set mesh' triangulation to the bunny's triangulated mesh 

       
        _mesh.RecalculateNormals(); //In order for the mesh to receive and cast light, we need to recalculate normals
        _mesh.RecalculateBounds();

        _deformedVertices = new Vector3[_vertices.Length];
        
    }

    private void Update()
    {
        waveAngleOffset += Time.deltaTime * waveCount;

        for (int v = 0; v < _vertices.Length; v++)
        {
            Vector3 vertexPosition = _vertices[v];
            //Manipulate! 

            //Push up and down with a sin wave
            float angle = vertexPosition.x * Mathf.PI * 2 * waveCount + waveAngleOffset;
            //Husk: Evighedscirklen (-1, 1)
            vertexPosition.y += Mathf.Sin(angle) * waveAmount;

            //Twist. 
            //Man kan have en rotation, som man kan gange med en position 
            //Og så vil rotation foregår ved den position

            //Rotate position around (0,0,0) with rotation (quaternion) 
            vertexPosition = Quaternion.Euler(0,vertexPosition.y * twistAmount,0) * vertexPosition;

            _deformedVertices[v] = vertexPosition;

        }
        _mesh.vertices = _deformedVertices;
        _mesh.RecalculateNormals();

        Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, material, gameObject.layer);
    }


}
