using UnityEngine;
using System.Collections;

//Applies gravity in local space of localObject.

public class LocalGravity : MonoBehaviour {

    public float gravity = 1f;
    public GameObject localObject;
    Rigidbody rb;

	// Use this for initialization
	void OnEnable () {
        //Debug.Log("Adding acceleration");
        rb = GetComponent<Rigidbody>();
        rb.AddForce(-gravity * localObject.transform.up, ForceMode.Acceleration);
        
    }
	
	// Update is called once per frame
	void Update () {

        //transform.Translate(-gravity * localObject.transform.up,Space.World);
        

	}
}
