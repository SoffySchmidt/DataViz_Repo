using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersonalDataExample : MonoBehaviour
{
    public string dataCsvFileName = "";

    //Lists are slower than arrays
    //<> means generic type
    //_people is private, thus uses "_"
    List<Person> _people = new List<Person>();

    int _ageMin, _ageMax;

    void Awake()
    {
        //Parse
        string csvFilePath = Application.streamingAssetsPath + "/" + dataCsvFileName;
        string csvContent = File.ReadAllText(csvFilePath);
        Parse(csvContent);

        //Filter
        Filter();

        //Mine
        Mine();

        //Represent
        Represent();

        //Debug.Log("_people.Count: " + _people.Count);
    }
    //methods belong to the class of an object
    //in C# we call them methods, not functions
    void Parse(string csvContentText)
    {
        //Split by new lines in rows... '\n' works as "Enter" and jumps to next line
        string[] rowContents = csvContentText.Split('\n');


        //for each row...
        for (int r = 1; r < rowContents.Length; r++)
        {
            string rowContent = rowContents[r];
            //Debug.Log(rowContent);
            //comma separated file, so it must be split between the commas
            string[] fieldContents = rowContent.Split(',');
            //creating object from class/"kageform"
            Person person = new Person(r);

            for (int f = 0; f < fieldContents.Length; f++)
            {
                string fieldContent = fieldContents[f];

              

                switch (f)
                {
                    case 0:
                        //First Name
                        person.firstName = fieldContent;
                        break;
                    case 1:
                        //Last Name
                        person.lastName = fieldContent;
                        break;
                    
                    case 2:
                        //Age                
                        int age;
                        //a bool evaluating whether or not it succeeded to parse the variable "age"
                        bool parseSucceeded = int.TryParse(fieldContent, out age);
                        if (parseSucceeded)
                            person.age = age;
                            
                        break;
                    case 3:   
                        //Had Covid
                        person.hadCovid = fieldContent.ToLower() == "yes";
                        break;
                    case 7:
                        //ZipCode
                        int postNumb;
                        parseSucceeded = int.TryParse(fieldContent, out postNumb);
                        if (parseSucceeded) person.postNumber = postNumb;
                        break;
                    case 8:
                        //Has Pets
                        person.hasPet = fieldContent.ToLower() == "yes";
                        break;
                    case 9:
                        //Cohabitants

                        //string coHabText = fieldContent.ToString();
                        //person.cohabitantsCount = coHabText;
                        int coHab;
                        parseSucceeded = int.TryParse(fieldContent, out coHab);
                        if (parseSucceeded) person.postNumber = coHab;
                        break;
                    case 10:
                        //Stream Games
                        int streamGames;
                        parseSucceeded = int.TryParse(fieldContent, out streamGames);
                        if (parseSucceeded) person.streamGamesCount = streamGames;
                        break;
                    case 11:
                        //Siblings
                        int siblings;
                        parseSucceeded = int.TryParse(fieldContent, out siblings);
                        if (parseSucceeded) person.siblingCount = siblings;
                        break;

                }
            }

            //CASE 4, 5 + 6 are here below

            // Parse covid relation level.
            Person.CovidRelationLevel covidRelationLevel = Person.CovidRelationLevel.None;
            if (fieldContents.Length > 6)
            {
                bool familyHadCovid, familyOrFriendsHadCovid, anyoneHadCovid;
                if (
                    bool.TryParse(fieldContents[4], out familyHadCovid) &&
                    bool.TryParse(fieldContents[5], out familyOrFriendsHadCovid) &&
                    bool.TryParse(fieldContents[6], out anyoneHadCovid)
                )
                {
                    if (anyoneHadCovid) covidRelationLevel = Person.CovidRelationLevel.Anyone;
                    else if (familyOrFriendsHadCovid) covidRelationLevel = Person.CovidRelationLevel.FamilyOrFriend;
                    else if (familyOrFriendsHadCovid) covidRelationLevel = Person.CovidRelationLevel.Family;
                }
            }
            person.covidRelationLevel = covidRelationLevel;

            //Add to person list
            _people.Add(person);
        }
    }

    void Filter()
    {
        //Inverted for-loop running from the highest to the lowest value
        for (int p = _people.Count-1; p >= 0; p--)
        {
            Person person = _people[p];

            if (person.age < 18 || person.age > 127)
            {
                _people.RemoveAt(p);
                Debug.Log("Invalid: " + person.firstName);
            }
        }
    }

    void Mine()
    {
        _ageMin = int.MaxValue;
        _ageMax = int.MinValue;

        foreach(Person person in _people)
        {
            if (person.age > _ageMax) _ageMax = person.age;
            else if (person.age < _ageMin) _ageMin = person.age;

        }

        Debug.Log("Min: " + _ageMin + " Max: " + _ageMax);
    }

    void Represent()
    {
        for (int p = 0; p < _people.Count; p++)
        {
            Person person = _people[p];
            float x = p;
            //Converted to a normalized value from 0 - 1 
            float height = Mathf.InverseLerp(_ageMin, _ageMax, person.age) * 10;
            float y = height * 0.5f;
            float width = 0.95f;

            GameObject mainObject = new GameObject(person.id + " " + person.firstName);

            mainObject.transform.SetParent(transform);
            mainObject.transform.localPosition = new Vector3(x, y, 0);

            GameObject barObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

            barObject.transform.SetParent(mainObject.transform);
            barObject.transform.localPosition = Vector3.zero;
            barObject.transform.localScale = new Vector3(width, height, 1);
        }
    }
}
