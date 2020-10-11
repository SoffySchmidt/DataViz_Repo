/*
	Copyright © Carl Emil Carlsen 2020
	http://cec.dk
*/

using System.IO;
using System.Collections.Generic;
using UnityEngine;


public class Zoonose : MonoBehaviour
{
    // TEXT FILE
    public string dataCsvFileName = "";
    public GameObject textObjectPrefab = null;

    Vector3 posXYZ;
    int _yearMin, _yearMax, _deathMin, _deathMax;
    LayerMask spawnedObjectLayer = 8;

    //RADIUS
    float circRad;
    float Rad;
    float minRad = float.MaxValue;
    float maxRad = float.MinValue;

    //GAMEOBJECTS
    GameObject mainObject;
    GameObject sphereObject;

    //LISTS
    List<Virus> _viruses = new List<Virus>();
    List<float> circleRadius = new List<float>();

    //COLOR
    Color lerpedColor;


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
            Virus virus = _viruses[v];
            circRad = Mathf.Log(virus.noDeaths) / 2;
            posXYZ = new Vector3(Random.Range(30f, 50f), Random.Range(30f, 50f), 0);
        
            //Empty Objects, parents of the spheres
            mainObject = new GameObject(virus.id + " " + virus.name);
            mainObject.transform.SetParent(transform);
            mainObject.transform.localScale = new Vector3(circRad, circRad, circRad);

            //BOOLS and INTS for positioning method
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
                    if (collider.tag == "Zoonose") validPosition = false;             
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
    