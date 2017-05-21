using UnityEngine;
using System.Collections;

public class ZoomInCameraOrhographic : MonoBehaviour {

    Camera cam;
    public float speed=1f;

	// Use this for initialization
	void Awake () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

        float value = Input.GetAxis("Mouse ScrollWheel");
        //Debug.Log(value);
        cam.orthographicSize += value* speed;




    }
}
