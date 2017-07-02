using UnityEngine;
using System.Collections;


//This script sets camera type at start function
//Add this to camera that are not main 
public class CameraTypeGetter : MonoBehaviour {

    Camera cam;
    CameraController cc;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        CharGameController.cameraType type = CharGameController.getCameraType();

        cc=CharGameController.getCamera().GetComponent<CameraController>();

        if (type == CharGameController.cameraType.Ortographic)
        {
            cam.orthographic = true;
            cam.farClipPlane = cc.farOrt;
            cam.nearClipPlane = cc.nearOrt;
        }
        else
        {
            cam.orthographic = false;
            cam.farClipPlane = cc.farPers;
            cam.nearClipPlane = cc.nearPers;
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
