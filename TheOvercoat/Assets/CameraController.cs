using UnityEngine;
using System.Collections;

//This script controlles camera properties. At first place it is just written for getting camera type and setting accroding to that and also listtening settings changing.
//But you can use it another purposes for camera later.


public class CameraController : MonoBehaviour {

    public static CameraController activeCamera;
    Camera cam;

    //Orthographic clipping sizes
    public float nearOrt = -40;
    public float farOrt = 2000;

    private void OnEnable()
    {
        activeCamera = this;
    }

    // Use this for initialization
    void Start () {
        cam=gameObject.GetComponent<Camera>();
        updateCameraType();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateCameraType()
    {
        if (!cam) cam = GetComponent<Camera>();

        if(CharGameController.cgc!=null)
        {
            CharGameController.cameraType type = CharGameController.getCameraType();

            if (type == CharGameController.cameraType.Ortographic)
            {
                Debug.Log(gameObject.name + " setting ortho");
                cam.orthographic = true;
                cam.farClipPlane = farOrt;
                cam.nearClipPlane = nearOrt;
            }else
            {
                Debug.Log(gameObject.name + " setting perso");
                cam.orthographic = false;
            }
        }
    }



    //public void setSize(float size){
    //    if (cam.orthographic)
    //    {
    //        cam.orthographicSize = size;
    //    }else
    //    {
    //        cam.fieldOfView = size;
    //    }
    //}

    //public float getSize()
    //{
    //    if (cam.orthographic)
    //    {
    //        return cam.orthographicSize;
    //    }
    //    else
    //    {
    //        return cam.fieldOfView;
    //    }
    //}


}
