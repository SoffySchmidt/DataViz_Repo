using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Zoonose2 : MonoBehaviour
{
    public AnimalHost animalHostScript;
    public Animal animalScript;
    // TEXT FILE
    public string dataCsvFileName = "";
    public GameObject textObjectPrefab = null;

    //Vector3 posXYZ;
    public int _yearMin, _yearMax, _deathMin, _deathMax;
    LayerMask spawnedObjectLayer = 8;

    public int circleCount = 16;
    public float repulsionForce = 5;
    public float centerForce = 0.1f;
    public float padding = 0.2f;

    Vector3[] positions;
    float[] radiuses;

    public Material mat;
    //RADIUS
    float circRad;
    float Rad;
    float minRad = float.MaxValue;
    float maxRad = float.MinValue;

    //GAMEOBJECTS
    public Year yearScript;
    public GameObject mainObject;
    GameObject sphereObject;
    public List<GameObject> sphereArray = new List<GameObject>();
    public List<GameObject> mainObjectArray = new List<GameObject>();

    Vector2 original;

    //DICTIONARY
    public Dictionary<int, GameObject> _mainObjectLookUp = new Dictionary<int, GameObject>();
    private IEnumerator coroutine;

    float spawnTime = 5f;
    float curSpawnTime;

    //LISTS
    public List<Virus> _viruses = new List<Virus>();
    List<float> circleRadius = new List<float>();
    int[] id = new int[16];
    int lengthOfLineRenderer = 20;
    //COLOR
    Color lerpedColor;



    void Awake()
    {

        curSpawnTime = spawnTime;
        // Parse.
        string csvFilePath = Application.streamingAssetsPath + "/" + dataCsvFileName;
        string csvContent = File.ReadAllText(csvFilePath);
        Parse(csvContent);

        positions = new Vector3[circleCount];
        radiuses = new float[circleCount];

        // Filter.
        Filter();
        // Mine.
        Mine();
        // Represent.
        Represent();
        // Interact.
        AddInteraction();
        //CheckReference();

        StartCoroutine(SpawnObjects());


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

            // For each field in this row.
            for (int f = 0; f < fieldContents.Length; f++)
            {
                string fieldContent = fieldContents[f];

                switch (f)
                {
                    case 0:
                        //ID
                        int id;
                        bool parseSucceededID = int.TryParse(fieldContent, out id);
                        if (parseSucceededID) virus.id = id;
                        break;
                    case 1:
                        //YEAR
                        int year;
                        bool parseSucceeded = int.TryParse(fieldContent, out year);
                        if (parseSucceeded) virus.year = year;
                        break;
                    case 2:
                        //VIRUS NAME
                        virus.name = fieldContent;
                        break;
                    case 3:
                        //NO. DEATHS
                        int noDeath;
                        bool parseSucceededDeath = int.TryParse(fieldContent, out noDeath);
                        if (parseSucceededDeath) virus.noDeaths = noDeath;
                        break;
                    case 4:
                        virus.animName = fieldContent;
                        break;
                }
            }
            // Add to virus list.
            _viruses.Add(virus);
        }
    }


    void Filter()
    {
        for (int v = _viruses.Count - 1; v >= 0; v--)
        {
            Virus virus = _viruses[v];

            if (virus.year < 1900)
            { // If from before 1900, remove
                _viruses.RemoveAt(v);
            }
        }
    }

    void Mine()
    {
        //YEAR MAX & MIN
        _yearMin = int.MaxValue;
        _yearMax = int.MinValue;
        foreach (Virus virus in _viruses)
        {
            if (virus.year > _yearMax) _yearMax = virus.year;
            else if (virus.year < _yearMin) _yearMin = virus.year;
        }
        //DEATH MAX & MIN
        _deathMin = int.MaxValue;
        _deathMax = int.MinValue;
        foreach (Virus virus in _viruses)
        {
            if (virus.noDeaths > _deathMax) _deathMax = virus.noDeaths;
            else if (virus.noDeaths < _deathMin) _deathMin = virus.noDeaths;
        }

        for (int i = 0; i < _viruses.Count; i++)
        {
            float thisRad = Mathf.Log(_viruses[i].noDeaths) / 2;
            Rad = thisRad;
            if (thisRad < minRad) minRad = thisRad;
            else if (thisRad > maxRad) maxRad = thisRad;
        }

        

    }

    void Represent()
    {
        // Sort by year.
        _viruses.Sort((a, b) => a.year - b.year);

        for (int v = 0; v < _viruses.Count; v++)
        {
            //Debug.Log(_yearMin + " " + _yearMax);

            Virus virus = _viruses[v];   
            id[v] = virus.id;
            circRad = Mathf.Log(virus.noDeaths) / 2;
            radiuses[v] = circRad;

            //posXYZ = new Vector3(Random.Range(30f, 50f), Random.Range(30f, 50f), 0);
            positions[v] = Random.insideUnitSphere * 5;

            //Empty Objects, parents of the spheres
            mainObject = new GameObject(virus.id + " " + virus.name);
            mainObject.transform.SetParent(transform);
            mainObject.transform.localScale = new Vector3(circRad, circRad, circRad);
            mainObject.tag = "Zoonose";

            sphereObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphereObject.name = mainObject.name;
            sphereObject.transform.SetParent(mainObject.transform);
            sphereObject.tag = "Zoonose";

            sphereArray.Add(sphereObject);
            mainObjectArray.Add(mainObject);

            _mainObjectLookUp.Add(virus.id, sphereObject);



            //Debug.Log(virus.id + " " + virus.year);

        }


    }

    IEnumerator SpawnObjects()
    {
        //yield return new WaitForSeconds(1);
        float timePassed = 0;
        while(timePassed < 5)
        {

            for (int c1 = 0; c1 < _viruses.Count; c1++)
            {
                Vector3 pos1 = positions[c1];
                float radius1 = radiuses[c1];
                //Debug.Log(radius1);
                // Add neightbor force.
                for (int c2 = 0; c2 < c1; c2++)
                {

                    Vector3 pos2 = positions[c2];
                    float radius2 = radiuses[c2];

                    Vector3 towards2 = pos2 - pos1;
                    float sqrDistace = towards2.sqrMagnitude;
                    float collisionDistance = radius1 + radius2 + padding;
                    float sqrCollisionDistance = collisionDistance * collisionDistance;
                    if (sqrDistace < sqrCollisionDistance)
                    {
                        float distance = Mathf.Sqrt(sqrDistace);
                        float forceMult = distance / collisionDistance; // normalize.
                        forceMult = 1 - forceMult; // Invert.
                        towards2 /= distance; // Normalize vector.
                        towards2 *= forceMult * repulsionForce;

                        pos1 -= towards2 * Time.deltaTime;
                        pos2 += towards2 * Time.deltaTime;

                        positions[c2] = pos2; // Update!
                    }
                }

                Vector3 center = Vector3.zero;
                Vector3 towardsCenter = center - pos1;
                pos1 += towardsCenter * centerForce * Time.deltaTime;

                positions[c1] = pos1; // Update!

            }
            for (int v = 0; v < _viruses.Count; v++)
            {
                mainObjectArray[v].transform.localPosition = new Vector3(positions[v].x, positions[v].y, 0);
                sphereArray[v].transform.localScale = new Vector3(radiuses[v], radiuses[v], radiuses[v]);
                //sphereArray[v].transform.localPosition 

                //ColorChange(sphereArray[v], minRad, maxRad, radiuses[v]);
            }

           timePassed += Time.deltaTime;

            yield return null;
        }
    }
    void Update()
    {
        
    
        
     

    }

    void Spawn()
    { 
    }
    /*
    void ColorChange(GameObject currSphere, float minRad, float maxRad, float circRad)
    {
        circleRadius.Add(circRad);
        float normalized = Mathf.InverseLerp(minRad, maxRad, circRad);
        lerpedColor = Color.Lerp(Color.yellow, Color.red, normalized);
        //Debug.Log(sphereObject.name + " " + normalized);
        currSphere.GetComponent<MeshRenderer>().material.color = lerpedColor * 1.1f;

    }
    */

    void AddInteraction()
    {


        foreach (Virus virus in _viruses)
        {
 
            GameObject mainObject = _mainObjectLookUp[virus.id];
            GameObject textObject = Instantiate(textObjectPrefab); // Make a copy of the original text object.
            textObject.SetActive(true);


            textObject.transform.SetParent(mainObject.transform);
            textObject.transform.localPosition = mainObject.transform.position;
            textObject.transform.Rotate(0, 0, 0);
            textObject.GetComponent<TextMesh>().text = virus.name;
 

            Collider barCollider = mainObject.GetComponentInChildren<Collider>();
            TextRevealer textRevealer = barCollider.gameObject.AddComponent<TextRevealer>(); // Instantiate (create) a new script and add it to colliders gameobject.

            textRevealer.textObject = textObject;

        }

      

    }
}






