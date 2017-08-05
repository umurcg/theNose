using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookToCamera : MonoBehaviour, IVisibility {

    Camera cam;
    public float lerpSpeed=1;
    public bool lerp = true;

    private void Awake()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        if(rend!=null)
          rend.gameObject.AddComponent<TellMeVisibility>().setScript(this);

    }

    // Use this for initialization
    void Start () {
        cam = CharGameController.getMainCameraComponent();

	}
	
	// Update is called once per frame
	void Update () {

        if (!cam) cam = CharGameController.getMainCameraComponent();

        if (!cam)
        {
            //Debug.Log("Cmera is null");
            return;
        }

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

    public void onVisible()
    {
        enabled = true;
    }

    public void onInvisible()
    {
        enabled = false;
    }
}
