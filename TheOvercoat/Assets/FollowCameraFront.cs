using UnityEngine;
using System.Collections;

public class FollowCameraFront : MonoBehaviour {

    public float forwardDistance;
    Camera  cam;

	// Use this for initialization
	void Start () {
        cam=CharGameController.getCamera().GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = cam.gameObject.transform.position + cam.transform.forward * forwardDistance;
        transform.LookAt(cam.transform);
            
	}
}
