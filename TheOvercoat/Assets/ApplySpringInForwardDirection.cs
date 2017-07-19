using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Aplly spring if goes straigth
public class ApplySpringInForwardDirection : MonoBehaviour {

    public float springFactor = 30;
    public float lerpSpeed = 2;
    //  public float damperFactor=1;
    //public float tol = 0.5f;
    HingeJoint hj;

    //Vector3 prevPos;
    Vector3 prevForwadDir;

    // Use this for initialization
    void Start () {
        hj = GetComponent<HingeJoint>();
        prevForwadDir = transform.forward;
    }
	
	// Update is called once per frame
	void Update () {

        float turnAngle = Vector3.Angle(prevForwadDir, transform.forward);
        var cross = Vector3.Cross(prevForwadDir, transform.forward);
        if (cross.y > 0) turnAngle = -turnAngle;
             

        //Debug.Log("ANGLE " + turnAngle);

        float pos = Mathf.Clamp(turnAngle*springFactor, -60, 60);

        JointSpring hingeSpring = hj.spring;
        hingeSpring.targetPosition=Mathf.Lerp(hingeSpring.targetPosition, turnAngle * springFactor,Time.deltaTime*lerpSpeed);
        hj.spring = hingeSpring;


        prevForwadDir = transform.forward;
	}

    //void recordPosAndDir()
    //{
    //    prevPos = transform.position;
    //    prevForwadDir = transform.forward;
    //}

}
