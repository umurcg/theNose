using UnityEngine;
using System.Collections;

public class CanvasCameraSetter : MonoBehaviour {

    Canvas canvas;
    public float planeDistane = 50;

    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
        updateCamera();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateCamera()
    {
        canvas = GetComponent<Canvas>();
        GameObject mainCamera = CharGameController.getCamera();
        if (mainCamera != null && mainCamera.activeSelf)
        {
            //Debug.Log("Assigning main camera");
            canvas.worldCamera = mainCamera.GetComponent<Camera>();
        }
        else
        {

            
            Debug.Log("Assigning current camera");
            canvas.worldCamera = CameraController.activeCamera.GetComponent<Camera>();
        }

        canvas.planeDistance = planeDistane;

    }
}
