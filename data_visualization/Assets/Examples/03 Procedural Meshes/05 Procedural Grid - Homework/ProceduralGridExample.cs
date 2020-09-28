using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This attribute automatically creates and adds the components "MeshFilter" and "MeshRenderer" to th object
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class ProceduralGridExample : MonoBehaviour
{
    Mesh _mesh;

    private Vector3[] vertices;

    public int xSize, ySize;

    //Awake is called (ONLY ONCE) when the script instance is being loaded.
    void Awake()
    {
        StartCoroutine(Generate());
    }

    
    IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);



        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _mesh.name = "Procedural Grid";

        //The amount of vertices depend on the size of the grid
        //We need a vertex at the corners of EVERY quad - but ADJACENT quads can share the same vertex.
        //So we need ONE MORE VERTEX than we have TILES in EACH DIMENSION
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];

        //NORMALS
        //we need to add tangent vectors to our mesh to properly orient normals. 
        //as we have a flat surface, all tangents point in the same direction, which is incorrect.

        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        //iterates through all X,Y positions and position them next to one another
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;
                yield return wait;
            }
        }

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.tangents = tangents;


        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        _mesh.triangles = triangles;
        //The default normal direction is (0,0,1) which is the exact opposite
        //Normals are defined per vertex, so we need to fill another vector array
        //Alternatively, we can ask the mesh to figure out the normals based on its triangle
        //like this
        _mesh.RecalculateNormals();


    }

    private void OnDrawGizmos()
    {
        //Checks whether the array exists - and jumps out of the method if it does not
        //We need this, as Gizmos are also invoked in while Unity is in EDIT MODE
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

    /* GIZMOS

    Gizmos are visual cues that you can use in the editor. 
    By default they're visible in the scene view and not in the game view, 
    but you can adjust this via their toolbars. 
    The Gizmos utility class allows you to draw icons, lines, and some other things.

    Gizmos can be drawn inside an OnDrawGizmos method, 
    which is automatically invoked by the Unity editor. 
    An alternative method is OnDrawGizmosSelected, 
    which is only invoked for selected objects.

    Q: Why won't the Gizmos move with the object?

    A: Gizmos are drawn directly in world space, not in the object's local space. 
    If you want them to respect your objects transform, 
    you'll have to explicitly apply it by using transform.
    TransformPoint(vertices[i]) instead of just vertices[i].


    NORMALS

    Normal maps are defined in tangent space.
    This is a 3D space that flows around the surface of an object. 
    This approach allows us to apply the same normal map in different places and orientations

    The NORMAL SURFACE represents UPWARD in this space, but which way is right?
    That's defined by the tangent.
    Ideally, the angle between these two vectors is 90 degrees 
    The cross product of them yields the third direction needed to define 3D space 

    So a tangent is a 3D vetor, but Unity actually uses a 4D vector. 
    Its fourth component is always either -1 or 1,
    which is used to control the direction of the third tangent space dimension - FORWARD or BACKWARD
    */
}
