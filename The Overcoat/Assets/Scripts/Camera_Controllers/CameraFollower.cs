using UnityEngine;
using System.Collections;

//_Camera Follower
//_Dependent to:

//This script makes camera to follow the target object.



public class CameraFollower : MonoBehaviour {
    public GameObject target;
    public float lookSpeed=3f;
    public float transformSpeed=3f;
    Vector3 relativePosition;
	// Use this for initialization
	void Awake () {
    
        if (target == null)
        {
            //myValue = anyFloat > 0 ? 1f : 2f;
            target= (gameObject== transform.parent.GetChild(0).gameObject) ? transform.parent.GetChild(1).gameObject: transform.parent.GetChild(0).gameObject;
        }
        relativePosition = transform.position - target.transform.position;

    }
	
	// Update is called once per frame
	void Update () {
        if (target)
        {
           transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), Time.deltaTime * lookSpeed);
           // transform.rotation = Quaternion.LookRotation(target.transform.position);
            transform.position = Vector3.Lerp(transform.position, relativePosition+ target.transform.position, Time.deltaTime * transformSpeed);
        }
        }
}
