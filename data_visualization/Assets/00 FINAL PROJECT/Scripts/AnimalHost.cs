/*
	Copyright © Carl Emil Carlsen 2020
	http://cec.dk
*/

using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class AnimalHost : MonoBehaviour
{
    //TEXT FILES
    public string dataCsvFileName = "";
    public GameObject textObjectPrefab = null;

    //RADIUS
    
    List<int> circRadArray = new List<int>();
    Vector3 posXYZ;
    //float minRad = float.MaxValue;
    //float maxRad = float.MinValue;

    public GameObject HaloPrefab;

    float[] radiuses;
    public int circleCount = 5;

    Vector3[] positions;
    //COLOR
    Color lerpedColor;
    public Color[] colors = new Color[5];

    //GAMEOBJECTS
    public GameObject animalObject;
    public GameObject childObjects;
    public GameObject[] AllObjects = new GameObject[5];
    public List<GameObject> animalList = new List<GameObject>();

    TextRevealer textRevealer;
    //LISTS
    public string[] listOfHosts;
    public List<Animal> _animals = new List<Animal>();

    public List<int> batArray = new List<int>();
    public List<int> birdsArray = new List<int>();
    public List<int> mosquitoesArray = new List<int>();
    public List<int> primatesArray = new List<int>();
    public List<int> pigsArray = new List<int>();

    Material mat01;


    //DICTIONARIES

    //Dictionary<int,GameObject> _mainObjectLookup = new Dictionary<int,GameObject>();
    Dictionary<string, int> _hostAnimNumb = new Dictionary<string, int>();

    public Dictionary<int, GameObject> _mainAnimLookUp = new Dictionary<int, GameObject>();


    void Awake()
    {

        mat01 = Resources.Load("mat01", typeof(Material)) as Material;

        colors[0] = Color.blue;
        colors[1] = Color.red;
        colors[2] = Color.green;
        colors[3] = Color.yellow;
        colors[4] = Color.cyan;

        AllObjects = new GameObject[5];
        radiuses = new float[circleCount];

        positions = new Vector3[circleCount];

        listOfHosts = new string[] { "Bats", "Birds", "Mosquitoes", "Pigs", "Primates" };
        for (int i = 0; i < listOfHosts.Length; i++)
        {
            _hostAnimNumb.Add(listOfHosts[i], 0);
        }
        // Parse.
        string csvFilePath = Application.streamingAssetsPath + "/" + dataCsvFileName;
        string csvContent = File.ReadAllText(csvFilePath);
        Parse(csvContent);

        // Filter.
        Filter();
        // Mine.
        //Mine();
        // Represent.
        Represent();
        // Interact.
        AddInteraction();

    }

    void Parse(string csvText)
    {
        // Split by new lines in rows.
        string[] rowContents = csvText.Split('\n');

        // For each row.
        for (int a = 1; a < rowContents.Length; a++)
        {
            string rowContent = rowContents[a];
            string[] fieldContents = rowContent.Split(';');
            Animal animal = new Animal(a);

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
                        if (parseSucceededID) animal.id = id;
                        break;
                    case 1:
                        // ANIMAL RESERVOIR
                        animal.animalReservoir = fieldContent;
                        foreach (string stringAnimal in listOfHosts)
                            if (animal.animalReservoir == stringAnimal)
                            {
                                if (_hostAnimNumb.ContainsKey(animal.animalReservoir))
                                {

                                    _hostAnimNumb[animal.animalReservoir] += 1; //Add 1 value to the specified key in the dictionary
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

            _animals.Add(animal);

            

        }
        //Debug.Log(" Bats: " + _hostAnimNumb["Bats"] + " Birds: " + _hostAnimNumb["Birds"] + " Mosquitos: " + _hostAnimNumb["Mosquitos"] + " Primates: " + _hostAnimNumb["Primates"] + " Pigs: " + _hostAnimNumb["Pigs"]);
    }

    void Filter()
    {

        foreach (KeyValuePair<string, int> attachStat in _hostAnimNumb)
        {
            //Now you can access the key and value both separately from this attachStat as:
            //Debug.Log(attachStat.Value);
            circRadArray.Add(attachStat.Value);
        }

    }

    void Represent()
    {

        for (int i = 0; i < AllObjects.Length; i++)
        {
            //Animal animal = new Animal(i);

            AllObjects[i] = new GameObject(listOfHosts[i]);
            AllObjects[i].transform.SetParent(transform);
            AllObjects[i].transform.localPosition = new Vector3(0, 0, 0);
            AllObjects[i].tag = "Animal";

            animalObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            animalObject.name = AllObjects[i].name;
            animalObject.transform.SetParent(AllObjects[i].transform);

         
            animalObject.tag = "Animal";


            positions[i] = new Vector3(10, 0, 0); //Random.insideUnitSphere * 5;

            animalList.Add(animalObject);


        }
        for (int a = 0; a < _animals.Count; a++)
        {
            Animal animal = _animals[a];
      

            if (_animals[a].animalReservoir == listOfHosts[0])
            {
                //6                
                childObjects = new GameObject(animal.id + " " + _animals[a].animalReservoir);
                childObjects.transform.SetParent(AllObjects[0].transform);
                childObjects.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
                batArray.Add(animal.id);
            }
            else if (_animals[a].animalReservoir == listOfHosts[1])
            {
                Debug.Log("Birds: " + _animals[a].id);
                //4            

                childObjects = new GameObject(animal.id + " " + _animals[a].animalReservoir);
                childObjects.transform.SetParent(AllObjects[1].transform);
                childObjects.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
                birdsArray.Add(animal.id);

            }
            else if (_animals[a].animalReservoir == listOfHosts[2])
            {
                Debug.Log("Mosquitoes: " + _animals[a].id);
                childObjects = new GameObject(animal.id + " " + _animals[a].animalReservoir);
                childObjects.transform.SetParent(AllObjects[2].transform);
                childObjects.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
                mosquitoesArray.Add(animal.id);
            }
            else if (_animals[a].animalReservoir == listOfHosts[3])
            {
                Debug.Log("Pigs: " + _animals[a].id);
                //circDiam += 1;

                childObjects = new GameObject(animal.id + " " + _animals[a].animalReservoir);
                childObjects.transform.SetParent(AllObjects[3].transform);
                childObjects.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
                pigsArray.Add(animal.id);
            }
            else if (_animals[a].animalReservoir == listOfHosts[4])
            {
                Debug.Log("Primates: " + _animals[a].id);
                //circDiam += 1;

                childObjects = new GameObject(animal.id + " " + _animals[a].animalReservoir);
                childObjects.transform.SetParent(AllObjects[4].transform);
                childObjects.transform.localPosition = new Vector3(posXYZ.x, posXYZ.y, 0);
                primatesArray.Add(animal.id);
            }
            else
            {
                Debug.Log("Other: " + _animals[a].id);
            }
            animalObject = childObjects;
       
            _mainAnimLookUp.Add(animal.id, animalObject);

            //Debug.Log(_animalNames + " & " + _animals[a].id);
          
        }
        float offsetY = 0;
        for (int i = 0; i < AllObjects.Length; i++)
        {
            offsetY += 100f;
            animalList[i].transform.localPosition = new Vector3(positions[i].x, positions[i].y + offsetY, 0);
            animalList[i].transform.localScale = new Vector3(50, 50, 50);



            StartColor(animalList[i], colors[i]);

            //minRad, maxRad, radiuses[i],
        }


    }

    public void StartColor(GameObject animObj, Color color)
    {

        //float normalized = Mathf.InverseLerp(minRad, maxRad, circRad);
        //lerpedColor = Color.Lerp(Color.blue, Color.green, normalized);
        //Debug.Log(sphereObject.name + " " + normalized);
        //animObj.GetComponent<MeshRenderer>().material.color = lerpedColor;

        animObj.GetComponent<MeshRenderer>().material = mat01;
        mat01.color = color;
    }

    void AddInteraction()
    {
       

            for (int i = 0; i < AllObjects.Length; i++)
            {
            GameObject mainObject = animalList[i];
            //Debug.Log(mainObject.transform.position);

                //Debug.Log(mainObject);
                GameObject textObject = Instantiate(textObjectPrefab); // Make a copy of the original text object.
                textObject.SetActive(true);

                textObject.transform.SetParent(AllObjects[i].transform);
                textObject.transform.position = new Vector3(mainObject.transform.position.x, mainObject.transform.position.y + 50,0);
                textObject.transform.Rotate(0, 0, 0);




            Collider barCollider = AllObjects[i].transform.GetComponentInChildren<SphereCollider>();
            textRevealer = barCollider.gameObject.AddComponent<TextRevealer>(); // Instantiate (create) a new script and add it to colliders gameobject.
                                                                               //TextObject of "TextRevealer" script equals textObject declared in this script
            textRevealer.textObject = textObject;

            textObject.layer = 10;
            textObject.GetComponent<TextMesh>().characterSize = 5;
            textObject.GetComponent<TextMesh>().text = animalList[i].name;
        }
            
        
    }

}