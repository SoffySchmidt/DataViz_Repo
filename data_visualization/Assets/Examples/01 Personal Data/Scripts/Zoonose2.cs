using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Zoonose2 : MonoBehaviour
{
    public string dataCsvFileName = "";
    public GameObject textObjectPrefab = null;
    GameObject mainObject;
    Vector3 posXYZ;
    float circRad;

    GameObject circleObject;
    List<Collider> listOfSpheres = new List<Collider>();
    //Lists: Dynamically sized array, where you do not need to define its size
    //More flexibility and functionality than an array
    List<Virus> _viruses = new List<Virus>();

    //Dictionaries are similiar to Lists
    //They take two generic terms. In this case an integer and a gameobject 
    //as the two values
    Dictionary<int, GameObject> _mainObjectLookup = new Dictionary<int, GameObject>();
    int _yearMin, _yearMax, _deathMin, _deathMax;

    public Material material = null;
    public int circleCount = 16;
    public int randomSeed = 16;

    float x, y;
    Mesh _mesh;

    const int circleResolution = 64;


    void Awake()
    {

        // Parse.
        string csvFilePath = Application.streamingAssetsPath + "/" + dataCsvFileName;
        string csvContent = File.ReadAllText(csvFilePath);
        Parse(csvContent);

        //Debug.Log(dataCsvFileName);
        // Filter.
        Filter();

        // Mine.
        Mine();

        // Represent.
        Represent();

        // Interact.
        //AddInteraction();


       
    }


    void Parse(string csvText)
    {
        // Split by new lines in rows.
        string[] rowContents = csvText.Split('\n');

        // For each row.
        for (int r = 1; r < rowContents.Length; r++)
        {
            string rowContent = rowContents[r];
            string[] fieldContents = rowContent.Split(';');
            Virus virus = new Virus(r);

            //Debug.Log(rowContent);

            // For each field in this row.
            for (int f = 0; f < fieldContents.Length; f++)
            {
                string fieldContent = fieldContents[f];

                switch (f)
                {
                    case 0:
                        int id;
                        bool parseSucceededID = int.TryParse(fieldContent, out id);
                        if (parseSucceededID) virus.id = id;
                        break;
                    case 1:
                        //Year
                        int year;
                        bool parseSucceeded = int.TryParse(fieldContent, out year);
                        if (parseSucceeded) virus.year = year;
                        break;
                    case 2:
                        // First name.
                        virus.name = fieldContent;
                        break;
                    case 3:
                        //No. Deaths
                        int noDeath;
                        bool parseSucceededDeath = int.TryParse(fieldContent, out noDeath);
                        if (parseSucceededDeath) virus.noDeaths = noDeath;
                        break;
                }
            }


            //Debug.Log(virus.year + " " + virus.noDeaths);

            // Add to person list.
            _viruses.Add(virus);
        }
    }


    void Filter()
    {
        for (int v = _viruses.Count - 1; v >= 0; v--)
        {
            Virus virus = _viruses[v];

            if (virus.year < 1900 || virus.year > 2020)
            { // If too young OR (||) too old
                _viruses.RemoveAt(v);
            }
        }
    }


    void Mine()
    {
        //Year
        _yearMin = int.MaxValue;
        _yearMax = int.MinValue;
        foreach (Virus virus in _viruses)
        {
            if (virus.year > _yearMax) _yearMax = virus.year;
            else if (virus.year < _yearMin) _yearMin = virus.year;
        }
        //Deaths
        _deathMin = int.MaxValue;
        _deathMax = int.MinValue;
        foreach (Virus virus in _viruses)
        {
            if (virus.noDeaths > _deathMax) _deathMax = virus.noDeaths;
            else if (virus.noDeaths < _deathMin) _deathMin = virus.noDeaths;
        }
    }


    void Represent()
    {
        // Sort by year.
        _viruses.Sort((a, b) => a.year - b.year);


        List<Vector3> vertices = new List<Vector3>();
        List<int> triangleIndices = new List<int>();
        List<Color> colors = new List<Color>();

        for (int v = 0; v < _viruses.Count; v++)
        {
            Virus virus = _viruses[v];

         

            //random.insideUnitCircle provides random point inside circle
            Vector2 position = Random.insideUnitCircle * 50;
            circRad = Mathf.Log(virus.noDeaths) / 2;
            float radius = circRad;
            Color color = Color.HSVToRGB(Random.value, 1, 1);


            mainObject = new GameObject(virus.id + " " + virus.name);
            mainObject.transform.SetParent(transform);
            mainObject.transform.localPosition = position;
            mainObject.transform.localScale = new Vector2(radius,radius);



            AddCircle(position, radius, color, vertices, triangleIndices, colors, virus.name);
        }

        _mesh = new Mesh();
        //_mesh.SetIndexBufferParams(triangleIndices.Count, UnityEngine.Rendering.IndexFormat.UInt32); //Increase max index count
        _mesh.SetVertices(vertices);
        //mesh constitutes of triangles
        _mesh.SetIndices(triangleIndices, MeshTopology.Triangles, 0);
        _mesh.SetColors(colors);


    }
 



    void AddCircle(Vector2 position, float radius, Color color, List<Vector3> vertices, List<int> triangleIndices, List<Color> colors, string virusName)
    {
        //Add vector2 position offset 
        Vector2 prevPoint = position + new Vector2(radius, 0); //Angle 0. 
        for (int i = 1; i < circleResolution; i = i + 1)
        {

            float t = Mathf.InverseLerp(0, circleResolution - 1, i); // Normalized value of i.                                                                         //arc measure?

            //we are using radians, not degrees
            // 1 degree = 0.017 radian
            float angle = t * Mathf.PI * 2; //approx. 0.1
            //Debug.Log(angle);
            x = Mathf.Cos(angle) * radius;
            y = Mathf.Sin(angle) * radius;
            Vector2 point = position + new Vector2(x, y);


            triangleIndices.Add(vertices.Count); //first time, index value is 0.
            vertices.Add(position); // Add vertex at circle's center point
            colors.Add(Color.white);

            triangleIndices.Add(vertices.Count);
            vertices.Add(point); // Add vertex at current point in circle
            colors.Add(color);


            triangleIndices.Add(vertices.Count);
            vertices.Add(prevPoint); // Add vertex at previous point in circle
            colors.Add(color);


            //for every point moved forward, we remember the previous 
            prevPoint = point;
        }


        circleObject = new GameObject();
        circleObject.transform.SetParent(mainObject.transform);
        circleObject.transform.position = new Vector3(x, y, 0);
        circleObject.name = virusName;
        circleObject.tag = "Zoonose";

    }

    void Update()
    {
        Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, material, gameObject.layer);
    }
}
/*

 // Sort by year.
 _viruses.Sort((a, b) => a.year - b.year);


 List<Collider> positions = new List<Collider>();
 // Create elements ...
 for (int v = 0; v < _viruses.Count; v++)
 {

     Virus virus = _viruses[v];
     circRad = Mathf.Log(virus.noDeaths) / 2;
     posXYZ = new Vector3(Random.Range(0, 20f), Random.Range(0, 20f), 0);

     //Empty Objects, parents of the spheres
     mainObject = new GameObject(virus.id + " " + virus.name);
     mainObject.transform.SetParent(transform);
     mainObject.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
     mainObject.transform.localScale = new Vector3(circRad, circRad, circRad);


     float radius = circRad;
     //Debug.Log(radius);
     bool validPosition = false;
     int spawnAttempts = 0;
     int maxSpawnAttemptsPerObstacle = 20;

     while (!validPosition && spawnAttempts < maxSpawnAttemptsPerObstacle)
     {
         spawnAttempts++;
         validPosition = true;

         //sphereObject.GetComponent<SphereCollider>().radius = circRad;

         Collider[] hitColliders = Physics.OverlapSphere(posXYZ, radius, 0);
         foreach (Collider otherCollider in hitColliders)
         {
             Debug.Log(otherCollider);
             //now we ask each of thoose gentle colliders if they sens something is within their bounds
             if (otherCollider.bounds.Intersects(sphereObject.GetComponent<SphereCollider>().bounds))

                 Debug.Log("intersects!" + otherCollider + " " + sphereObject);
             validPosition = false;

         }


     }

     {
         if (validPosition)

             sphereObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
         sphereObject.transform.SetParent(mainObject.transform);
         sphereObject.tag = "Zoonose";
         sphereObject.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
         sphereObject.transform.localScale = new Vector3(circRad, circRad, circRad);

     }
     //CheckSphere(posXYZ, circRad);

 }


}
*/
