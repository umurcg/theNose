using UnityEngine;
using System.Collections;

public class VrCaptureController : MonoBehaviour {

    // Use this for initialization

    public bool stop = false;
	void Start () {
        VRCapture.VRCapture.Instance.StartCapture();	
	}
	
	// Update is called once per frame
	void Update () {
        if (stop)
        {
            VRCapture.VRCapture.Instance.StopCapture();
            enabled = false;
        }
    }
}
