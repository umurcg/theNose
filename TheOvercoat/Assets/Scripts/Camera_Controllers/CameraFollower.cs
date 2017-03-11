using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//_Camera Follower
//_Dependent to:

//This script makes camera to follow the target object.



public class CameraFollower : MonoBehaviour {
    public GameObject target;
    public float lookSpeed=3f;
    public float transformSpeed=3f;
    public bool assignPlayerAutomatically=true;

    Vector3 relativePosition;


    Vector3 defaultRelative = new Vector3(-89.1f, 72.7f, 85.1f);
    float xRotation;

    void Start () {

        if (assignPlayerAutomatically)
            updateTarget();

        if (target == null)
        {
            //myValue = anyFloat > 0 ? 1f : 2f;
            target= (gameObject== transform.parent.GetChild(0).gameObject) ? transform.parent.GetChild(1).gameObject: transform.parent.GetChild(0).gameObject;
        }

        updateRelative();
        xRotation = transform.eulerAngles.x;
    }
	
	// Update is called once per frame
	void Update () {
        if (target)
        {
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            //Preserve euler angles x
            transform.rotation = Quaternion.Euler(30, transform.eulerAngles.y, 0);
            transform.position = relativePosition + target.transform.position;
            //Lerp
           // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), Time.deltaTime * lookSpeed);
           //transform.position = Vector3.Lerp(transform.position, relativePosition+ target.transform.position, Time.deltaTime * transformSpeed);
        }

        //Debug.Log(relativePosition);
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

    public void changeTarget(GameObject obj)
    {
        target = obj;
        updateRelative();
    }

    //Use this if your camera mess up when new sceene is loaded
    public void fixRelativeToDefault()
    {
        relativePosition = defaultRelative;

    }


    public IEnumerator<float> changeTargetWithLerp(GameObject obj, float speed)
    {
        return Timing.RunCoroutine(_changeTargetWithLerp(obj,speed));
    }

    IEnumerator<float> _changeTargetWithLerp(GameObject obj,float speed)
    {

        //Stop update function
        enabled = false;


        Vector3 initialPosition = gameObject.transform.position;

        float ratio = 0;
        while (ratio < 1)
        {
            ratio += Time.deltaTime * speed;
            gameObject.transform.position = Vector3.Lerp(initialPosition, relativePosition+ obj.transform.position, ratio);
            yield return 0;

        }
        gameObject.transform.position = relativePosition+ obj.transform.position;


        changeTarget(obj);


        //Continue update function
        enabled = true;
        yield break;
    }


}
