using UnityEngine;
using System.Collections;

//This script enables to adjust camera fov and position for hitcock shot. 
//One parameters affects other parameter oppositly. So hitcock shot can be obtained with adjustin one parameter.


public class HitchcockShot : MonoBehaviour {
    Camera cam;

    [Range(1, 100.0f)]
    public float fov=69;
    [Range(-10f, 10.0f)]
    public float dist;
    public GameObject mirrorPlane;

    float initialDist;

    public float distTolerance = 0.2f;


    public float prevDist;
    public float prevFov;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        fov = cam.fieldOfView;
        dist = Vector3.Distance(transform.position, mirrorPlane.transform.position);
 


	}
	
    void calFov()
    {
        fov-=(dist - prevDist) * 5f;
    }

    void calDist()
    {
         dist -= (fov - prevFov) * 0.2f;
    }

	// Update is called once per frame
	void Update () {

        if (prevDist != dist)
        {
            calFov();
        }
        else if (prevFov != fov)
        {
            calDist();

        }


        transform.position= mirrorPlane.transform.position -  transform.forward*dist;

        cam.fieldOfView = fov;

        //float currentDist = Vector3.Distance(transform.parent.position, transform.position);

        /*    transform.position += transform.forward * (currentDist-dist*/

        prevDist = dist;
        prevFov = fov;



    }
}
