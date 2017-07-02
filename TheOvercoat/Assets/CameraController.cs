﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using MovementEffects;

//This script controlles camera properties. At first place it is just written for getting camera type and setting accroding to that and also listtening settings changing.
//But you can use it another purposes for camera later.


public class CameraController : MonoBehaviour {

    public static CameraController activeCamera;
    Camera cam;

    //Orthographic clipping sizes
    public float nearOrt = -40;
    public float farOrt = 2000;
    //public float size=

    //Perspective settings
    //public float FOV = 10;
    public float nearPers = 0.3f;
    public float farPers = 1000;

    public float sizeToDistance=9;

    public GameObject focus;
    float defaultDistance;
    float defaultSize;


    CameraFollower cf;

    ////Test values
    //public float testZoomIn, testZoomOut,  testSmoothSpeed, testMinSpeed;

    private void OnEnable()
    {
        activeCamera = this;
    }

    // Use this for initialization
    void Start () {
        cam=gameObject.GetComponent<Camera>();
        updateCameraType();

        if(focus== null)
            updateFocus();

        defaultDistance = Vector3.Distance(focus.transform.position, transform.position);
        defaultSize = cam.orthographicSize;

        cf = GetComponent<CameraFollower>();
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
                cam.nearClipPlane = nearPers;
                cam.farClipPlane = farPers;
            }
        }
    }

    public void zoomOut(float zoomAmount)
    {
        if(cf)
        cf.enabled = false;

        if(cam==null) cam = gameObject.GetComponent<Camera>();

        if (cam.orthographic)
        {
            cam.orthographicSize += zoomAmount;

        }
        else
        {
            cam.transform.position -= cam.transform.forward * zoomAmount * sizeToDistance;
        }

        if (cf)
        {
            cf.updateRelative();
            cf.enabled = true;
        }
    }


    public void zoomIn(float zoomAmount)
    {
        if (cf)
            cf.enabled = false;

        if (cam.orthographic )
        {
            if(cam.orthographicSize-zoomAmount>0)
                 cam.orthographicSize -= zoomAmount;

        }
        else
        {
            if(Vector3.Distance(transform.position,focus.transform.position)>zoomAmount*sizeToDistance)
                cam.transform.position += cam.transform.forward * zoomAmount * sizeToDistance;
        }

        if (cf)
        {
            cf.updateRelative();
            cf.enabled = true;
        }
    }


    public void defaultZoom()
    {
        if (cf)
            cf.enabled = false;

        cam.orthographicSize = defaultSize;

        Vector3 forwardDir = transform.forward;
        float dist = Vector3.Distance(focus.transform.position, transform.position);

        transform.position += forwardDir * (dist - defaultDistance);

        if (cf)
        {
            cf.updateRelative();
            cf.enabled = true;
        }
    }

    public void updateFocus()
    {
        focus = CharGameController.getActiveCharacter();
    }


    public void smoothZoomOut(float zoomAmount, float speed)
    {
        Timing.RunCoroutine(_smoothZoomOut(zoomAmount, speed));
    }


    public void smoothZoomIn(float zoomAmount, float speed)
    {
        Timing.RunCoroutine(_smoothZoomIn(zoomAmount, speed));
    }

    public void smoothZoomOutToVeryFar(float zoomAmount, float speed, float minSpeed)
    {
        Timing.RunCoroutine(_smoothZoomOutToVeryFar(zoomAmount, speed,minSpeed));
    }


    public void smoothZoomInFromVeryFar(float zoomAmount, float speed, float minSpeed)
    {
        Timing.RunCoroutine(_smoothZoomInFromVeryFar(zoomAmount, speed, minSpeed));
    }


    public IEnumerator<float> _smoothZoomOut(float zoomAmount,float speed)
    {

        float totalZoom=0;
        while (totalZoom <= zoomAmount)
        {
            totalZoom += Time.deltaTime * speed;
            zoomOut(Time.deltaTime * speed);
            yield return 0;
        }

        zoomIn(totalZoom - zoomAmount);


        yield break;
    }

    public IEnumerator<float> _smoothZoomIn(float zoomAmount, float speed)
    {

        float totalZoom = 0;
        while (totalZoom <= zoomAmount)
        {
            totalZoom += Time.deltaTime * speed;
            zoomIn(Time.deltaTime * speed);
            yield return 0;
        }


        zoomOut(totalZoom - zoomAmount);

        yield break;
    }




    public IEnumerator<float> _smoothZoomOutToVeryFar(float zoomAmount, float maxSpeed, float minSpeed = 0.2f)
    {


        float totalZoom = 0;
        while (totalZoom <= zoomAmount)
        {
            float deltaSpeed = (totalZoom / zoomAmount) * maxSpeed;
            if (deltaSpeed < minSpeed) deltaSpeed = minSpeed;

            float deltaZoom = Time.deltaTime * deltaSpeed;

            totalZoom += deltaZoom;
            zoomOut(deltaZoom);
            yield return 0;
        }


        zoomIn(totalZoom - zoomAmount);


        yield break;
    }


    public IEnumerator<float> _smoothZoomInFromVeryFar(float zoomAmount, float maxSpeed, float minSpeed = 0.2f)
    {
        

        float totalZoom = 0;
        while (totalZoom <= zoomAmount)
        {
            float deltaSpeed = ((zoomAmount - totalZoom) / zoomAmount) * maxSpeed;
            if (deltaSpeed < minSpeed) deltaSpeed = minSpeed;

            float deltaZoom = Time.deltaTime *deltaSpeed;
            
            totalZoom += deltaZoom;
            zoomIn(deltaZoom);
            yield return 0;
        }

        Debug.Log(totalZoom + " " + zoomAmount);

        zoomOut(totalZoom - zoomAmount);


        yield break;
    }


}



//[CustomEditor(typeof(CameraController), true)]
//public class CameraControllerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();
//        CameraController script = (CameraController)target;
//        if (GUILayout.Button("TestDefault zoom "))
//        {
//            script.defaultZoom();

//        }

//        if (GUILayout.Button("Test zoom in"))
//        {
//            script.zoomIn(script.testZoomIn);
//        }


//        if (GUILayout.Button("Test zoom out"))
//        {
//            script.zoomOut(script.testZoomOut);
//        }


//        if (GUILayout.Button("Test smooth zoom in"))
//        {
//            script.smoothZoomIn(script.testZoomIn,script.testSmoothSpeed);
//        }


//        if (GUILayout.Button("Test smooth zoom out"))
//        {
//            script.smoothZoomOut(script.testZoomOut, script.testSmoothSpeed);
//        }

//        if (GUILayout.Button("Test smooth zoom in from very far"))
//        {
//            script.smoothZoomInFromVeryFar(script.testZoomIn, script.testSmoothSpeed, script.testMinSpeed);
//        }


//        if (GUILayout.Button("Test smooth zoom out to very far"))
//        {
//            script.smoothZoomOutToVeryFar(script.testZoomOut, script.testSmoothSpeed, script.testMinSpeed);
//        }
//    }
//}