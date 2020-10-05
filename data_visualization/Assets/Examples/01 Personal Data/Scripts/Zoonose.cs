/*
	Copyright © Carl Emil Carlsen 2020
	http://cec.dk
*/

using System.IO;
using System.Collections.Generic;
using UnityEngine;


public class Zoonose : MonoBehaviour
{
    public string dataCsvFileName = "";
    public GameObject textObjectPrefab = null;
    GameObject mainObject;
    Vector3 posXYZ;
    float circRad;

    GameObject sphereObject;
    List<Collider> listOfSpheres = new List<Collider>();

    List<Virus> _viruses = new List<Virus>();


    float Rad;

    Color lerpedColor;
    //Dictionary<int, GameObject> _mainObjectLookup = new Dictionary<int, GameObject>();
    int _yearMin, _yearMax, _deathMin, _deathMax;
    List<float> circleRadius = new List<float>();

    List<GameObject> itemsToPickFrom = new List<GameObject>();

    float overlapTestBoxSize;

    float minRad = float.MaxValue;
    float maxRad = float.MinValue;

    LayerMask spawnedObjectLayer = 8;


    void Awake()
    {
        // Parse.
        string csvFilePath = Application.streamingAssetsPath + "/" + dataCsvFileName;
        string csvContent = File.ReadAllText(csvFilePath);
        Parse(csvContent);

        // Filter.
        Filter();

        // Mine.
        Mine();

        // Represent.
        Represent();

        // Interact.
        //AddInteraction();


        //float val = Mathf.Log(50, 5);
        //Debug.Log(val);
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

        for (int i = 0; i < _viruses.Count; i++)
        {
           

                float thisRad = Mathf.Log(_viruses[i].noDeaths) / 2;

                Rad = thisRad;

                if (thisRad < minRad)
                {
                    minRad = thisRad;

                }

                else if (thisRad > maxRad)
                {

                    maxRad = thisRad;
                }
            
        }
        
        
    }


    void Represent()
    {
        // Sort by year.
        _viruses.Sort((a, b) => a.year - b.year);

        // Create elements ...
        for (int v = 0; v < _viruses.Count; v++)
        {

            Virus virus = _viruses[v];
            circRad = Mathf.Log(virus.noDeaths) / 2;
            posXYZ = new Vector3(Random.Range(30f, 50f), Random.Range(30f, 50f), 0);
        
            //Empty Objects, parents of the spheres
            mainObject = new GameObject(virus.id + " " + virus.name);
            mainObject.transform.SetParent(transform);
            mainObject.transform.localScale = new Vector3(circRad, circRad, circRad);



            bool validPosition = false;
            int spawnAttempts = 0;
            int maxSpawnAttemptsPerObstacle = 20;
            float obstacleCheckRadius = circRad * 2f;

            
            while (!validPosition && spawnAttempts < maxSpawnAttemptsPerObstacle)
            {
                spawnAttempts++;

                validPosition = true;

                Collider[] Colliders = Physics.OverlapSphere(posXYZ, obstacleCheckRadius);
             

                foreach (Collider collider in Colliders)
                {

                    if (collider.tag == "Zoonose")
                    {

                        validPosition = false;
                    }
                }
                if (validPosition)

                {
                    sphereObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphereObject.name = virus.name;
                    sphereObject.transform.SetParent(mainObject.transform);
                    sphereObject.tag = "Zoonose";
                    //Debug.Log("name: " + sphereObject.name + "pos " + sphereObject.transform.position);

                    sphereObject.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
                    sphereObject.transform.localScale = new Vector3(circRad, circRad, circRad);
                    //Debug.Log(sphereObject.name + " " + posXYZ);

                    ColorChange(minRad, maxRad, circRad);

                }
            }


        }


        void ColorChange(float minRad, float maxRad, float circRad)
        {
            

            circleRadius.Add(circRad);

            float normalized = Mathf.InverseLerp(minRad, maxRad, circRad);
            lerpedColor = Color.Lerp(Color.yellow, Color.red, normalized);
            //Debug.Log(sphereObject.name + " " + normalized);


            sphereObject.GetComponent<MeshRenderer>().material.color = lerpedColor * 1.1f;
        }
       

    }
}
    