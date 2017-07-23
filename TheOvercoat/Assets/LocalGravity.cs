using UnityEngine;
using System.Collections;

//Applies gravity in local space of localObject.
//This script is dangerous because it changes graviy direction for all rigidbodies
public class LocalGravity : MonoBehaviour {

    public float gravity = 1f;
    public GameObject localObject;

    Vector3 originalGravity;

    //Rigidbody rb;

	// Use this for initialization
	void OnEnable () {
        //Debug.Log("Adding acceleration");
        //rb = GetComponent<Rigidbody>();
        //rb.AddForce(-gravity * localObject.transform.up, ForceMode.Acceleration);

        originalGravity = Physics.gravity;
        Physics.gravity = (localObject.transform.up * -1)*gravity;

    }

    private void OnDisable()
    {
        Physics.gravity = originalGravity;
    }

    // Update is called once per frame
    void Update () {

        //transform.Translate(-gravity * localObject.transform.up, Space.World);
        //transform.position -= gravity * localObject.transform.up * Time.deltaTime;


    }
}
