using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This scripts holds objects that collides with parent object collider.


public class CollidingObjects : MonoBehaviour {

    //List holding colliding objects.
    public List<GameObject> colObjs;

    // Use this for initialization
	void Awake () {
        colObjs = new List<GameObject>();
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider collider) {
        colObjs.Add(collider.gameObject);
        
	}

    void OnTriggerExit(Collider collider)
    {
        colObjs.Remove(collider.gameObject);

    }



}
