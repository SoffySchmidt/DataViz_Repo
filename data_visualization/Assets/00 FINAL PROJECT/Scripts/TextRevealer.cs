/*
	Copyright © Carl Emil Carlsen 2020
	http://cec.dk
*/

using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class TextRevealer : MonoBehaviour
{
    AnimalHost animHostScript;
    Zoonose2 zoonoseScript;
    PointRenderer pointRenderer;

	public GameObject textObject = null;
    Material mat01, mat02;
    Ray ray;
    RaycastHit hit;
    public Vector3 animalPosition, zoonosePosition;
    List<Animal> animals = new List<Animal>();
    List<Virus> zoonoses = new List<Virus>();

    Canvas canvas;
    List<GameObject> textInfoObjects = new List<GameObject>();

    TextMesh textMesh;

	void Start()
	{
        GameObject tempObject = GameObject.Find("Canvas");

        if(tempObject != null)
        {
            canvas = tempObject.GetComponent<Canvas>();
        }
        foreach(Transform child in canvas.transform)
        {
            textInfoObjects.Add(child.gameObject);
        }

        animHostScript = GameObject.Find("Animal").GetComponent<AnimalHost>();

        pointRenderer = GameObject.Find("LineRenderer").GetComponent<PointRenderer>();

        zoonoseScript = GameObject.Find("Zoonose2").GetComponent<Zoonose2>();
        zoonoses = zoonoseScript._viruses;

        mat01 = Resources.Load("mat01", typeof(Material)) as Material;
        mat02 = Resources.Load("mat02", typeof(Material)) as Material;

        textObject.SetActive( true );
        //meshRend = animHostScript.animalList
        
    }
    
	void OnMouseEnter()
	{
        pointRenderer.isClear = false;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
           

            if (hit.transform.tag == "Animal")
            {
                //animals = hit.transform.GetComponent<AnimalHost>()._animals;

                if (hit.transform.name == "Bats")
                {
                    Check(animHostScript.batArray, hit.transform.gameObject, Color.blue);
                    ShowText(hit.transform.name);

                    hit.transform.GetComponentInChildren<MeshRenderer>().material = mat02;
                    animalPosition = hit.transform.GetComponentInChildren<Transform>().position;
                    pointRenderer.AnimalPosition(animalPosition);
                    //Debug.Log(animalPosition);
                }

                else if (hit.transform.name == "Mosquitoes")
                {
                    Check(animHostScript.mosquitoesArray, hit.transform.gameObject, Color.white);
                    ShowText(hit.transform.name);

                    hit.transform.GetComponentInChildren<MeshRenderer>().material = mat02;
                    animalPosition = hit.transform.GetComponentInChildren<Transform>().position;
                    pointRenderer.AnimalPosition(animalPosition);
                    //Debug.Log(animalPosition);
                }

                else if (hit.transform.name == "Primates")
                {
                    Check(animHostScript.primatesArray, hit.transform.gameObject, Color.red);
                    ShowText(hit.transform.name);

                    hit.transform.GetComponentInChildren<MeshRenderer>().material = mat02;
                    animalPosition = hit.transform.GetComponentInChildren<Transform>().position;
                    pointRenderer.AnimalPosition(animalPosition);
                    //Debug.Log(animalPosition);
                }

                else if (hit.transform.name == "Birds")
                {
                    Check(animHostScript.birdsArray, hit.transform.gameObject, Color.yellow);
                    ShowText(hit.transform.name);

                    hit.transform.GetComponentInChildren<MeshRenderer>().material = mat02;
                    animalPosition = hit.transform.GetComponentInChildren<Transform>().position;
                    pointRenderer.AnimalPosition(animalPosition);
                    //Debug.Log(animalPosition);
                }

                else if (hit.transform.name == "Pigs")
                {
                    Check(animHostScript.pigsArray, hit.transform.gameObject, Color.green);
                    ShowText(hit.transform.name);

                    hit.transform.GetComponentInChildren<MeshRenderer>().material = mat02;
                    animalPosition = hit.transform.GetComponentInChildren<Transform>().position;
                    pointRenderer.AnimalPosition(animalPosition);
                    //Debug.Log(animalPosition);
                }
            }

            /*

            else if(hit.transform.tag == "Zoonose")
            {
               
                zoonosePosition = hit.transform.GetComponentInChildren<Transform>().position;
                pointRenderer.ZoonosePosition(zoonosePosition);
                Debug.Log(zoonosePosition);
            }

            */

        }


        //animHostScript.ColorChange(Color.white);
    }

	void OnMouseExit()
	{
        hit.transform.GetComponentInChildren<MeshRenderer>().material = mat01;

        pointRenderer.isClear = true;

        pointRenderer.StopAllCoroutines();

        for (int i = 0; i < pointRenderer.listOfPoints.Count; i++)
        {
            Destroy(pointRenderer.listOfPoints[i].gameObject);
        }
        
        foreach(GameObject child in textInfoObjects)
        {
            child.SetActive(false);
        }

        pointRenderer.point = null;

    }

    void ShowText(string objectName)
    {
            switch (objectName)
            {
                case "Bats":
                canvas.transform.Find("Bats").gameObject.SetActive(true);
                    break;
                case "Mosquitoes":
                canvas.transform.Find("Mosquitoes").gameObject.SetActive(true);

                break;
                case "Primates":
                canvas.transform.Find("Primates").gameObject.SetActive(true);
                break;
                case "Birds":
                canvas.transform.Find("Birds").gameObject.SetActive(true);
                break;
                case "Pigs":
                canvas.transform.Find("Pigs").gameObject.SetActive(true);
                break;
            }
        }

    void Check(List<int> ID_array, GameObject hitObject, Color col)
    {
        pointRenderer.DrawLines(hitObject, col);
        //pointRenderer.ZoonosePosition(hitObject.transform.position);
        foreach (int id in ID_array)
        {

            foreach (KeyValuePair< int, GameObject> attachStat in zoonoseScript._mainObjectLookUp)
                    {
                if (attachStat.Key == id)
                {
                    pointRenderer.ZoonosePosition(attachStat.Value.gameObject.transform.position);
                    pointRenderer.DrawLines(attachStat.Value.gameObject, col);
                    Debug.Log("Zoonose Object: " + attachStat.Value.gameObject.transform.position);
                }

                else
                {
                    continue;
                }
                    }
    
        }
    }


}