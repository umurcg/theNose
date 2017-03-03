using UnityEngine;
using System.Collections;

public class CanvasCameraSetter : MonoBehaviour {

    Canvas canvas;

    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
        canvas = GetComponent<Canvas>();
        GameObject mainCamera = CharGameController.getCamera();
        if (mainCamera != null) canvas.worldCamera = mainCamera.GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
