using UnityEngine;
using System.Collections;

public class ApplySpringWhenForwardObjectMoving : MonoBehaviour {

    public float springFactor=1;
  //  public float damperFactor=1;
    public float tol=0.5f;
    public GameObject forwardObject;
    HingeJoint hj;
    Vector3 prevPos;



	// Use this for initialization
	void Start () {

        hj = GetComponent<HingeJoint>();
        prevPos = forwardObject.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
        float speed = (Vector3.Distance(forwardObject.transform.position,prevPos));
        if (speed > tol)
        {

            JointSpring js = hj.spring;
            js.spring = springFactor*speed;
            //js.damper = damperFactor*speed;
            hj.spring = js;

        }else
        {
            JointSpring js = hj.spring;
            js.spring = 0;
            //js.damper = 0;
            hj.spring = js;
        }

        prevPos = forwardObject.transform.position;
        

    }
}
