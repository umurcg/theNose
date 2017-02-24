using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;


public class NewsGameController : MonoBehaviour {

    

	// Use this for initialization
	void Start () {

        //Check for girty
        GirtController gc= CharGameController.getOwner().GetComponentInChildren<GirtController>();
        if (gc != null)
        {
            Destroy(GetComponents < SubtitleController >()[0]);
        }
        else
        {
            Destroy(GetComponents<SubtitleController>()[1]);
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
