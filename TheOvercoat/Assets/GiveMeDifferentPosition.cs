using UnityEngine;
using System.Collections;
using System;

//Use this script if you don't have another script to put different position for active objects
public class GiveMeDifferentPosition : MonoBehaviour, IClickActionDifferentPos {

    public GameObject posObject;

    public Vector3 giveMePosition()
    {
        return posObject.transform.position;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
