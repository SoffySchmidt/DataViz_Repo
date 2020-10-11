using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererExample : MonoBehaviour
{
    [SerializeField]
    private GameObject linePointPrefab;
    [SerializeField]
    private GameObject lineGeneratorPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos.z = 0;
            CreatePointMarker(newPos);
        }
        if (Input.GetMouseButtonDown(1))
        {
            ClearAllPoints();
        }
        if(Input.GetKeyDown("e"))
        {
            GenerateNewLine();
        }
    }

    void CreatePointMarker(Vector3 pointPosition)
    {
        Instantiate(linePointPrefab, pointPosition, Quaternion.identity);
    }

    void ClearAllPoints()
    {
        GameObject[] allPoints = GameObject.FindGameObjectsWithTag("PointMarker");

        foreach(GameObject p in allPoints)
        {
            Destroy(p);
        }
    }

    void GenerateNewLine()
    {
        GameObject[] allPoints = GameObject.FindGameObjectsWithTag("PointMarker");
        Vector3[] allPointPositions = new Vector3[allPoints.Length];
        if(allPoints.Length >= 2)
        {
            for (int i = 0; i < allPoints.Length; i++)
            {
                allPointPositions[i] = allPoints[i].transform.position;
            }

            SpawnLineGenerator(allPointPositions);
        }
        else
        {
            Debug.Log("Need 2 or more points to draw a line");
        }
    }
    void SpawnLineGenerator(Vector3[] linePoints)
    {
        GameObject newLineGen = Instantiate(lineGeneratorPrefab);
        LineRenderer lRend = newLineGen.GetComponent<LineRenderer>();

        lRend.positionCount = linePoints.Length;
        lRend.SetPositions(linePoints);
        //lRend.loop = false;
        /*
        lRend.positionCount = 4;

        lRend.SetPosition(0, new Vector3(-2, 2, 0));
        lRend.SetPosition(1, new Vector3(2, 2, 0));
        lRend.SetPosition(2, new Vector3(2, -2, 0));
        lRend.SetPosition(3, new Vector3(-2, -2, 0));
        */
        Destroy(newLineGen, 5);
    }
}
