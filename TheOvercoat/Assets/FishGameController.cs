using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishGameController : MonoBehaviour {

    public float minSpeed=1000;
    public float maxSpeed=2000;


    public float lineThreshold = 1f;

    List<float> difs;
    Vector2 prevMousePos;

    List<Vector2> lines;
    List<Vector2> positions;

    // Use this for initialization
    void Start () {

        difs = new List<float>();
        lines = new List<Vector2>();
        positions = new List<Vector2>();

    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetMouseButton(0))
        {
            recordDif();
        }

        if (Input.GetMouseButtonUp(0)) {

            //Debug.Log("Mouse speed is "+getMeanofList(difs));
            Debug.Log("Curvature is " + calculateCurvature());
            
            difs.Clear();
            lines.Clear();
            positions.Clear();
            prevMousePos = Vector3.zero;
        }

    }

    void recordDif()
    {
        if (prevMousePos == Vector2.zero)
        {
            prevMousePos = Input.mousePosition;
            positions.Add(prevMousePos);
            return;
        }else
        {

            positions.Add(Input.mousePosition);
            float dif = Vector2.Distance((Vector2)Input.mousePosition, prevMousePos);
            difs.Add(dif/Time.deltaTime);


            prevMousePos = Input.mousePosition;
        }

    }


    float calculateCurvature()
    {






        List<float> angles = new List<float>();

        //for(int i = 1; i < lines.Count; i++)
        //{
        //    float angle = 180-Vector2.Angle(-lines[i - 1], lines[i]);
        //    angles.Add(angle);
        //    Debug.Log(angle);

        //}


        for (int i = 1; i < lines.Count; i++)
        {
            float angle = 180 - Vector2.Angle(-lines[i - 1], lines[i]);
            angles.Add(angle);
            Debug.Log(angle);

        }




        return getMeanofList(angles);

    }
    

    float sumList(List<float> list)
    {
        float sum = 0;
        for (var i = 0; i < list.Count; i++)
        {
            sum += list[i];
        }
        return sum;
    }

    float getMeanofList(List<float> list)
    {
        float sum = 0;
        for (var i = 0; i < list.Count; i++)
        {
            sum += list[i];
        }
        return sum / list.Count;
    }


}
