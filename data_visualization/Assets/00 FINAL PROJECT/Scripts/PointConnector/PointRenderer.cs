using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PointRenderer : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public LineRenderer lineRenderer;
    public Transform towerPos;
    GameObject[] myLines;

    public Transform point;
    public bool isClear;

    Material material;
    List<Vector3> zooPosArray = new List<Vector3>();
    public List<Transform> pointList = new List<Transform>();

    public List<GameObject> listOfPoints = new List<GameObject>();

    IEnumerator coroutine;


    // Start is called before the first frame update
    void Start()
    {
     
        coroutine = CreatePoints(5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!point1 ||!point2 ||!lineRenderer)
        {
            return;
        }

        if (isClear)
        {
            zooPosArray.Clear();
        }

        lineRenderer.positionCount = 4;
        //lineRenderer.SetPositions(DrawCorneredAngle(point1.position, point2.position));
        //lineRenderer.SetPositions(point1.position, point2.position);
    }

    public void AnimalPosition(Vector3 animPos)
    {
        point1.position = animPos;
    }

    public void ZoonosePosition(Vector3 zooPos)
    {
        zooPosArray.Add(zooPos);
        StartCoroutine(coroutine);
    }

    IEnumerator CreatePoints(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        foreach (Vector3 zooPos in zooPosArray)
        {
            point = point2;
            point.transform.position = zooPos;
            pointList.Add(point);
          
        }
        
    }

    public void DrawLines(GameObject Zoonose, Color lineColor)
    {
        myLines = new GameObject[pointList.Count];
        Vector3 startPos = Zoonose.transform.position;
       

        // Cache your material so you don't need to make so many copies.
        material = Resources.Load("mat03", typeof(Material)) as Material;


        for (int i = 0; i < pointList.Count; i++)
        {
            // FIRST: Create & assign your GameObject into the array slot.
            var lineObject = new GameObject("Line" + i);
            myLines[i] = lineObject;
            listOfPoints.Add(lineObject);

            lineObject.transform.position = startPos;

            LineRenderer lr = lineObject.AddComponent<LineRenderer>();


            // Use sharedMaterial to be batching-friendly and avoid excess allocation.
            lr.sharedMaterial = material;

            lr.startColor = lineColor;
            lr.startWidth = 0.5f;
            lr.SetPosition(0, startPos);
            lr.SetPosition(1, pointList[i].transform.position);
        }
    }
    /*
    Vector3[] DrawCorneredAngle(Vector3 point1, Vector3 point2)
    {

        float xDiff = point1.x - point2.x;
        float yDiff = point1.y - point2.y;

        Vector3 point1Up = point1;
        Vector3 point2Up = point2;

        Vector3 mid = (point1 + point2) / 2;

        if(Mathf.Abs(xDiff) < Mathf.Abs(yDiff))
        {
            //X less than Y
            var halfDiff = xDiff / 2;
            point1Up = new Vector3(point1.x, mid.y - halfDiff, point1.z);
            point2Up = new Vector3(point2.x, mid.y + halfDiff, point2.z);
        }

        else
        {
            //Y less than X 
            var halfDiff = yDiff / 2;
            point1Up = new Vector3(mid.x - halfDiff, point1.y, point1.z);
            point2Up = new Vector3(mid.x + halfDiff, point2.y, point2.z);
        }

        return new Vector3[] { point1, point1Up, point2Up, point2 };
    }
    */
}
