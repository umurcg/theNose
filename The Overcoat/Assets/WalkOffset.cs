using UnityEngine;
using System.Collections;
using System;

//This function holds position for iclikaction script.
//With that player walks differnt position when click to active object.
//This position is position of object + offset position

public class WalkOffset : MonoBehaviour , IClickActionDifferentPos{


    public Vector3 walkOffset;

    public Vector3 giveMePosition()
    {
        return transform.position+walkOffset;
    }

    // Use this for initialization
    void Start () {
	
	}

   
	
	// Update is called once per frame
	void Update () {
	
	}
}
