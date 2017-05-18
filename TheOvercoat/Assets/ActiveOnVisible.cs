using UnityEngine;
using System.Collections;

public class ActiveOnVisible : MonoBehaviour {

    Camera mainCam;

	// Use this for initialization
	void Start () {
        mainCam = CharGameController.getCamera().GetComponent<Camera>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    private void OnBecameInvisible()
    {
        if (Camera.current != mainCam) return;

        Debug.Log("I became Invisible");
        enabled = false;
    }

    private void OnBecameVisible()
    {

        if (Camera.current != mainCam) return;

        Debug.Log("I became visible");
        enabled = true;
    }
}
