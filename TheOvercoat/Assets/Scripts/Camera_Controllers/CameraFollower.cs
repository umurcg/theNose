using UnityEngine;
using System.Collections;

//_Camera Follower
//_Dependent to:

//This script makes camera to follow the target object.



public class CameraFollower : MonoBehaviour {
    public GameObject target;
    public float lookSpeed=3f;
    public float transformSpeed=3f;
    public bool assignPlayerAutomatically=true;

    Vector3 relativePosition;
	// Use this for initialization
	void Start () {

        if (assignPlayerAutomatically)
            updateTarget();

        if (target == null)
        {
            //myValue = anyFloat > 0 ? 1f : 2f;
            target= (gameObject== transform.parent.GetChild(0).gameObject) ? transform.parent.GetChild(1).gameObject: transform.parent.GetChild(0).gameObject;
        }

        updateRelative();
    }
	
	// Update is called once per frame
	void Update () {
        if (target)
        {
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.position = relativePosition + target.transform.position;
            //Lerp
           // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), Time.deltaTime * lookSpeed);
           //transform.position = Vector3.Lerp(transform.position, relativePosition+ target.transform.position, Time.deltaTime * transformSpeed);
        }
    }

    public void updateRelative()
    {
        relativePosition = transform.position - target.transform.position;
    }

    public void updateTarget()
    {
        target = CharGameController.getActiveCharacter();
        if(target!=null)
            updateRelative();
    }
}
