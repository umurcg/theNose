using UnityEngine;
using System.Collections;

public class LookWalkDirection : MonoBehaviour {

    public float rotSpeed = 1f;
    Vector3 direction=Vector3.zero;
    Vector3 prevPos = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (prevPos == Vector3.zero)
        {
            prevPos = transform.position;
        }else if(Vector3.Distance(transform.position,prevPos)!=0)
        {
            direction = transform.position - prevPos;
            direction = direction.normalized;

            transform.rotation = Quaternion.Slerp(
                                       transform.rotation,
                                         Quaternion.LookRotation(direction),
                                              Time.deltaTime * rotSpeed
                                                                             );
        }


        prevPos = transform.position;

	}
}
