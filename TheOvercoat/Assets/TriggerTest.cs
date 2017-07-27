using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour {

    public bool entered = false;
    public string enteredTag;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " is entered.");
        entered = true;
        enteredTag = other.tag;
    }



    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + " is exited.");
        entered = false;
        enteredTag = "";
    }
}
