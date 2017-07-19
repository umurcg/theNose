using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookToCamera : MonoBehaviour {

    Camera cam;
    public float lerpSpeed=1;
    public bool lerp = true;

	// Use this for initialization
	void Start () {
        cam = CharGameController.getMainCameraComponent();
	}
	
	// Update is called once per frame
	void Update () {

        if (!cam) cam=Camera.main;

        Quaternion aimRot = Quaternion.LookRotation(-cam.transform.forward, cam.transform.up);
        
        if (lerp)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, aimRot, Time.deltaTime*lerpSpeed);
        }
        else
        {
            transform.rotation = aimRot;
        }

        //transform.LookAt(cam.transform.position);
	}
}
