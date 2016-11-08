using UnityEngine;
using System.Collections;

//Rotates around object.

public class RotateCameraAroundCharacter : MonoBehaviour {

    public bool debug = false;
    public float speed = 1;
    public GameObject aim;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (debug)
        {
            transform.RotateAround(aim.transform.position,Vector3.up,speed*Time.deltaTime);
        }

	}
}
