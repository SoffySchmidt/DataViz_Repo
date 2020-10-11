using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererExample3 : MonoBehaviour
{
    public Zoonose2 zoonoseScript;
    [SerializeField]
    private GameObject lineGeneratorPrefab;
    // Start is called before the first frame update
    GameObject[] zoonoseObjects = new GameObject[16];
    void Awake()
    {
       
        SpawnLineGenerator();
    }
  
    void ClearAllPoints()
    {
        GameObject[] allPoints = GameObject.FindGameObjectsWithTag("PointMarker");

        foreach(GameObject p in allPoints)
        {
            Destroy(p);
        }
    }

 
    public void SpawnLineGenerator()
    {
        GameObject newLineGen = Instantiate(lineGeneratorPrefab);
        LineRenderer lRend = newLineGen.GetComponent<LineRenderer>();

        //lRend.positionCount = linePoints.Length;
        //lRend.SetPositions(linePoints);
        //lRend.loop = false;
        
        lRend.positionCount = 3;
        lRend.startWidth = 0.5f;
        lRend.endWidth = 0.5f;

        /*
        lRend.SetPosition(0, cube1.transform.position);
        lRend.SetPosition(1, cube2.transform.position);
        lRend.SetPosition(2, cube3.transform.position);
        */

        //Destroy(newLineGen, 5);
    }
}
