/*
	Copyright © Carl Emil Carlsen 2020
	http://cec.dk
*/

using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimalHost: MonoBehaviour
{
	public string dataCsvFileName = "";
	public GameObject textObjectPrefab = null;
    public GameObject[] AllObjects = new GameObject[5];

    public int count;
    float circRad;
    List <int> circRadArray = new List<int>();

    Vector3 posXYZ;

    List<float> circleRadius = new List<float>();
    Color lerpedColor;

    float minRad = float.MaxValue;
    float maxRad = float.MinValue;
    float Rad;


    public string[] listOfHosts; 

    List<Animal> _animals = new List<Animal>();

	Dictionary<int,GameObject> _mainObjectLookup = new Dictionary<int,GameObject>();

    Dictionary<string, int> _hostAnimNumb = new Dictionary<string, int>();

    Dictionary<string, int>.ValueCollection hostCount;

    GameObject animalObject;

    void Awake()
	{
        AllObjects = new GameObject[5];

        
        listOfHosts = new string[] { "Bats", "Birds", "Mosquitos", "Pigs", "Primates" };
        for(int i = 0; i < listOfHosts.Length; i++)
        {
            _hostAnimNumb.Add(listOfHosts[i], 0);
        }
        // Parse.
        string csvFilePath = Application.streamingAssetsPath + "/" + dataCsvFileName;
		string csvContent = File.ReadAllText( csvFilePath );
		Parse( csvContent );

		// Filter.
		Filter();
		// Mine.
		//Mine();
		// Represent.
		Represent();
		// Interact.
		//AddInteraction();

	}

	void Parse( string csvText )
	{
		// Split by new lines in rows.
		string[] rowContents = csvText.Split( '\n' );

        // For each row.
        for (int a = 1; a < rowContents.Length; a++) {
            string rowContent = rowContents[a];
            string[] fieldContents = rowContent.Split(';');
            Animal animal = new Animal(a);

			// For each field in this row.
			for( int f = 0; f < fieldContents.Length; f++ ) {
				string fieldContent = fieldContents[ f ];
                
                switch ( f )
				{
                    case 0:
                        //ID
                        int id;
                        bool parseSucceededID = int.TryParse(fieldContent, out id);
                        if (parseSucceededID) animal.id = id;
                        break;
                    case 1:
						// ANIMAL RESERVOIR
						animal.animalReservoir = fieldContent;  
                        foreach(string stringAnimal in listOfHosts)
                        if(animal.animalReservoir == stringAnimal)
                        {  
                            if (_hostAnimNumb.ContainsKey(animal.animalReservoir))
                            {
                                    //Add 1 value to the specified key in the dictionary
                                    _hostAnimNumb[animal.animalReservoir] += 1;
                            }
                            else
                            {
                                    _hostAnimNumb.Add(animal.animalReservoir, 1);
                            }
                        }            
						break;
                    case 2:
                        //ANIMAL AMPLIFIERS
                        animal.animalAmplifier = fieldContent;
                        break;    
                }
        
            }
            
            _animals.Add( animal);
            hostCount = _hostAnimNumb.Values;
            
            Debug.Log("circRad: " + circRad);
        }    
        //Debug.Log(" Bats: " + _hostAnimNumb["Bats"] + " Birds: " + _hostAnimNumb["Birds"] + " Mosquitos: " + _hostAnimNumb["Mosquitos"] + " Primates: " + _hostAnimNumb["Primates"] + " Pigs: " + _hostAnimNumb["Pigs"]);
    }

    void Filter()
    {

        foreach (KeyValuePair<string, int> attachStat in _hostAnimNumb)
        {
            //Now you can access the key and value both separately from this attachStat as:
            Debug.Log(attachStat.Value);
            circRadArray.Add(attachStat.Value);
        }
        /*
        for (int i = 0; i < AllObjects.Length; i++)
        {
        
            int thisRad = circRadArray[i];
            Debug.Log(thisRad);

            Rad = thisRad;

            if (thisRad < minRad)
            {
                minRad = thisRad;

            }

            else if (thisRad > maxRad)
            {

                maxRad = thisRad;
            }

        }*/
    }

    void Represent()
    {

        for (int i = 0; i < AllObjects.Length; i++)
        {

            AllObjects[i] = new GameObject(listOfHosts[i]);
            AllObjects[i].transform.SetParent(transform);
            AllObjects[i].transform.localPosition = new Vector3(0, 0, 0);


            //Debug.Log("AllObjects: " + listOfHosts[i]);
        }
        for (int a = 0; a < _animals.Count; a++)
        {
            Animal animal = _animals[a];
            //position = transform.TransformPoint(Random.insideUnitSphere * circRad * 2f);

            float mainX = 1;
            if (_animals[a].animalReservoir == listOfHosts[0])
            {
                //6                
                GameObject childObjects = new GameObject(animal.id + " " + _animals[a].animalReservoir);
                childObjects.transform.SetParent(AllObjects[0].transform);
                childObjects.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
            }
            else if (_animals[a].animalReservoir == listOfHosts[1])
            {
                Debug.Log("Birds: " + _animals[a].id);
                //4            
                mainX += 1;
                GameObject childObjects = new GameObject(animal.id + " " + _animals[a].animalReservoir);
                childObjects.transform.SetParent(AllObjects[1].transform);
                childObjects.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
            }
            else if (_animals[a].animalReservoir == listOfHosts[2])
            {
                Debug.Log("Mosequitos: " + _animals[a].id);
                mainX += 2;

                GameObject childObjects = new GameObject(animal.id + " " + _animals[a].animalReservoir);
                childObjects.transform.SetParent(AllObjects[2].transform);
                childObjects.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
            }
            else if (_animals[a].animalReservoir == listOfHosts[3])
            {
                Debug.Log("Pigs: " + _animals[a].id);
                //circDiam += 1;
                mainX += 3;
                GameObject childObjects = new GameObject(animal.id + " " + _animals[a].animalReservoir);
                childObjects.transform.SetParent(AllObjects[3].transform);
                childObjects.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
            }
            else if (_animals[a].animalReservoir == listOfHosts[4])
            {
                Debug.Log("Primates: " + _animals[a].id);
                //circDiam += 1;
                mainX += 4;
                GameObject childObjects = new GameObject(animal.id + " " + _animals[a].animalReservoir);
                childObjects.transform.SetParent(AllObjects[4].transform);
                childObjects.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
            }
            else
            {
                Debug.Log("Other: " + _animals[a].id);
            }
        }
        for (int i = 0; i < AllObjects.Length; i++)
        {
            circRad = circRadArray[i] * 5f;

            bool validPosition = false;
            int spawnAttempts = 0;
            int maxSpawnAttemptsPerObstacle = 20;
            float obstacleCheckRadius = circRad * 2f;

            posXYZ = new Vector3(Random.Range(30f, 50f), Random.Range(30f, 50f), 0);

            while (!validPosition && spawnAttempts < maxSpawnAttemptsPerObstacle)
            {
                spawnAttempts++;

                validPosition = true;

                Collider[] Colliders = Physics.OverlapSphere(posXYZ, obstacleCheckRadius);


                foreach (Collider collider in Colliders)
                {

                    if (collider.tag == "Animal")
                    {

                        validPosition = false;
                    }
                }
                if (validPosition)

                {
                    animalObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    animalObject.name = AllObjects[i].name;
                    animalObject.transform.SetParent(AllObjects[i].transform);
                    animalObject.tag = "Animal";
                    //Debug.Log("name: " + sphereObject.name + "pos " + sphereObject.transform.position);

                    animalObject.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
                    animalObject.transform.localScale = new Vector3(circRad, circRad, circRad);

                    Debug.Log(animalObject.transform.localScale);

                    ColorChange(minRad, maxRad, circRad);

                }
            }
        }
    }

    void ColorChange(float minRad, float maxRad, float circRad)
    {

        float normalized = Mathf.InverseLerp(minRad, maxRad, circRad);
        lerpedColor = Color.Lerp(Color.blue, Color.green, normalized);
        //Debug.Log(sphereObject.name + " " + normalized);


        animalObject.GetComponent<MeshRenderer>().material.color = lerpedColor;
    }

    /*
    Vector3 prevPoint = position + new Vector3(circRad, 0); //Angle 0. 


    GameObject sphereObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    Vector3 point = position + new Vector3(position.x, position.y, 0);


    sphereObject.AddComponent<SphereCollider>();

    sphereObject.transform.SetParent(AllObjects[i].transform);
    sphereObject.transform.localPosition = new Vector3(position.x, position.y, 0);
    sphereObject.transform.localScale = new Vector3(circRad, circRad, circRad);


    prevPoint = point;


    Debug.Log(prevPoint);
    */








    //Here we add to the dictionary
    // Add an entry to the object lookup, so that we can use person id to find it's associated main object in the scene.
    /*
    for (int j = 0; j < _animals.Count; j++)
    {
        if (_mainObjectLookup.ContainsKey(animal.id))
        {
            continue;
        }
        else
        {
            _mainObjectLookup.Add(animal.id, );
        }

    Debug.Log(_mainObjectLookup[animal.id]);
    }
    */

    /*
    void AddInteraction()
    {


        for (int i = 0; i < AllObjects.Length; i++)
        {
           Animal animal = _animals[i];

            GameObject mainObject = _mainObjectLookup[animal.id];

            GameObject textObject = Instantiate(textObjectPrefab); // Make a copy of the original text object.
            textObject.SetActive(true);


            textObject.transform.SetParent(AllObjects[i].transform);
            textObject.transform.localPosition = AllObjects[i].transform.position;
            textObject.transform.Rotate(0, 0, -45);
            textObject.GetComponent<TextMesh>().text = animal.animalReservoir;

            Collider barCollider = mainObject.GetComponentInChildren<Collider>();
            TextRevealer textRevealer = barCollider.gameObject.AddComponent<TextRevealer>(); // Instantiate (create) a new script and add it to colliders gameobject.
                                                                                             //TextObject of "TextRevealer" script equals textObject declared in this script
            textRevealer.textObject = textObject;

        }

    }
    */


    /*
//SORT DICTIONARY VALUES from MIN to MAX
_hostAnimNumb = _hostAnimNumb.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
foreach(KeyValuePair<string, int> item in _hostAnimNumb.OrderBy(key => key.Value))
{
    //Do something
    Debug.Log("Dictionary: " + _hostAnimNumb.Values.ToList()[0] + " " + _hostAnimNumb.Values.ToList()[1] + " " + _hostAnimNumb.Values.ToList()[2] + " " + _hostAnimNumb.Values.ToList()[3] + " " + _hostAnimNumb.Values.ToList()[4]);
}
*/
    /*
        void Mine()
        {

            _hostMin = int.MaxValue;
            _hostMax = int.MinValue;


            foreach (int s in hostCount)
            {
                if (s > _hostMax) _hostMax = s;
                else if (s < _hostMin) _hostMin = s;

                Debug.Log("HEY" + _hostMax + " "  + _hostMin);
                foreach(Animal animal in _animals)
                {
                    if (animal.animalCount > _hostMax) _hostMax = animal.animalCount;
                    else if (animal.animalCount < _hostMin) _hostMin = animal.animalCount;

                    Debug.Log("hostcount: " + animal.animalCount + "host max: " + _hostMax + " " + _hostMin);
                }

            }



    // Debug.Log(_hostMin + " " + _hostMax);

        }
    */




    /*
    AllObjects[0] = new GameObject("Bats");
        AllObjects[0].transform.SetParent(transform);
            AllObjects[0].transform.localPosition = new Vector3(0, 0, 0);

    AllObjects[1] = new GameObject("Birds");
        AllObjects[1].transform.SetParent(transform);
            AllObjects[1].transform.localPosition = new Vector3(0, 0, 0);

    AllObjects[2] = new GameObject("Mosquitos");
        AllObjects[2].transform.SetParent(transform);
            AllObjects[2].transform.localPosition = new Vector3(0, 0, 0);

    AllObjects[3] = new GameObject("Pigs");
        AllObjects[3].transform.SetParent(transform);
            AllObjects[3].transform.localPosition = new Vector3(0, 0, 0);

    AllObjects[4] = new GameObject("Primates");
        AllObjects[4].transform.SetParent(transform);
            AllObjects[4].transform.localPosition = new Vector3(0, 0, 0);

    */
}