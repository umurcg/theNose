using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class travelCity : MonoBehaviour {

    public GameObject pointsParent;
    public Camera cam;
    public float speed = 1f;

    GameObject[] points;

    Vector3 initialPos;
    int currentAim = 0;
    float ratio = 0;

    Vector3 prevPos;
    Vector3 prevMovDirection;



    

    private void Awake()
    {
        points=new GameObject[pointsParent.transform.childCount];

        for (int i = 0; i < points.Length; i++) points[i] = pointsParent.transform.GetChild(i).gameObject;

        setHeightOfThePoint();

        initialPos = cam.transform.position;

        prevPos = cam.transform.position;






    }

    // Use this for initialization
    void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {

        

        if (ratio < 1)
        {
            var aim = points[currentAim].transform.position;
            float distance = Vector3.Distance(initialPos, aim);

            ratio += Time.deltaTime * speed/distance;

            

            cam.transform.position = Vector3.Lerp(initialPos, points[currentAim].transform.position, ratio);
            
        }
        else
        {
            ratio = 0;
            initialPos = cam.transform.position;
            if(currentAim+1>=points.Length)
            {
                currentAim=0;
            }
            else
            {
                currentAim++;
            }
        }

        //Rotate towards movement
        Vector3 movementDir = transform.position - prevPos;

        var dirChange = movementDir - prevMovDirection;
        var lookRot = transform.position + transform.forward + dirChange;

        //cam.transform.LookAt(lookRot);
        cam.transform.RotateAround(cam.transform.position, Vector3.up, Time.deltaTime * speed);

        prevPos = cam.transform.position;
        prevMovDirection = movementDir;

    }

    void setHeightOfThePoint()
    {
        foreach (GameObject p in points)
        {
            var pos = p.transform.position;
            pos.y = cam.transform.position.y;
            p.transform.position = pos;
        }
            
    }

}
