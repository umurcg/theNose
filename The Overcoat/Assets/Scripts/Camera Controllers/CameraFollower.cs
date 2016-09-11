using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {
    public GameObject target;
    public float lookSpeed=3f;
    public float transformSpeed=3f;
    Vector3 relativePosition;
	// Use this for initialization
	void Awake () {
        relativePosition = transform.position - target.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (target)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), Time.deltaTime * lookSpeed);
            transform.position = Vector3.Lerp(transform.position, relativePosition+ target.transform.position, Time.deltaTime * transformSpeed);
        }
        }
}
