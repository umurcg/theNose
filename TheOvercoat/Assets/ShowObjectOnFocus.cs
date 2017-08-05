using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectOnFocus : MonoBehaviour, IClickAction {

        

    public Canvas canvas3D;
    public float camDistance;
    public float minScale = 1;
    public float maxScale = 10;
    


    GameObject focusObject;
    Camera cam;

    PlayerComponentController pcc;
	// Use this for initialization
	void Start () {
        cam = CharGameController.getMainCameraComponent();
        pcc = CharGameController.getActiveCharacter().GetComponent<PlayerComponentController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(focusObject!=null && Input.GetMouseButtonDown(0))
        {
            destroySpawn();
        }
	}


    [ContextMenu ("Spawn")]
    void spawn()
    {
        if (!cam || !pcc)
        {
            cam = CharGameController.getMainCameraComponent();
            pcc = CharGameController.getActiveCharacter().GetComponent<PlayerComponentController>();
        }

        if (focusObject != null) return;

        focusObject = Instantiate(gameObject);
        focusObject.transform.parent = canvas3D.transform;


        Destroy(focusObject.GetComponent<ShowObjectOnFocus>());

        focusObject.transform.position = cam.ScreenToWorldPoint((Vector3)Vckrs.centerOfScreen() + Vector3.forward * camDistance);

        focusObject.AddComponent<rotateObjectRightClick>();
        ScaleObjectWithMouse sowm = focusObject.AddComponent<ScaleObjectWithMouse>();
        sowm.minScale = minScale;
        sowm.maxScale = maxScale;
        sowm.scaleLerp = true;
        sowm.lerpSpeed = 2;

        focusObject.transform.LookAt(cam.transform.position);

        pcc.StopToWalk();

    }

    void destroySpawn()
    {
        Destroy(focusObject);
        focusObject = null;
        pcc.ContinueToWalk();

    }

    public void Action()
    {
        spawn();

    }
}
