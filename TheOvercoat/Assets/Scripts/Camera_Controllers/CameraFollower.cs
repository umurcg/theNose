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
    public float rotatorSpeed = 3f;
    public bool assignPlayerAutomatically=true;
    public float damper = 0;

    public string axis = "CameraRotator";

    public Vector3 relativePosition;


    public Vector3 defaultRelative = new Vector3(-90f, 75f, 90f);
   
    float xRotation;
    public bool lockCamRotation = false;

    //Vector3 prevPos;

    Vector3 ghost;
    //GameObject ghostObject;

    //GameObject primitiveCube;
    //public Vector3 relativePositionBeforeDisabling;

    void Start () {

        //primitiveCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (assignPlayerAutomatically)
            updateTarget();
                
        if (target == null)
        {
            //myValue = anyFloat > 0 ? 1f : 2f;
            target= (gameObject== transform.parent.GetChild(0).gameObject) ? transform.parent.GetChild(1).gameObject: transform.parent.GetChild(0).gameObject;
            
        }


        //prevPos = target.transform.position;

        fixRelativeToDefault();
        //updateRelative();
        xRotation = transform.eulerAngles.x;

        transform.position = target.transform.position+relativePosition;

        ghost = target.transform.position;
        //ghostObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //ghostObject.name = "ghost";
        //ghostObject.GetComponent<Collider>();

        Debug.Log("Finishing start function "+gameObject.name);

    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Updating " + gameObject.name);
        if (target!=null)
        {
           
            rotater();
            follow();

        }
        else
        {
            Debug.Log("target is null " + gameObject.name);
        }

        //ghostObject.transform.position = ghost;
        //preserveXRotation();


        //prevPos = target.transform.position;


        //Debug.Log(relativePosition);
    }

    void preserveXRotation()
    {
        Quaternion rot = transform.rotation;
        rot.x = 30;
        transform.rotation = rot;
    }

    public void rotater()
    {
        if (lockCamRotation) return;

        //After setting all position and rotation for player movement, check for ratator
        //Debug.Log(Input.GetAxis("CameraRotator"));
        if (Input.GetAxis(axis) != 0)
        {

            //Debug.Log("Rotating");
            var wantedRotation = rotatorSpeed * Input.GetAxis(axis);
            //transform.RotateAround(/*getFocus()*/ target.transform.position, target.transform.up, Time.deltaTime * rotatorSpeed * wantedRotation);
            transform.RotateAround(/*getFocus()*/ ghost, target.transform.up, Time.deltaTime * rotatorSpeed * wantedRotation);

            //transform.position += target.transform.position - prevPos;

            updateRelative();

        }
        
    }

    //Vector3 getFocus()
    //{
    //    float angleToDown = Vector3.Angle(transform.forward, -Vector3.up);
    //    float d = transform.position.y - target.transform.position.y;
    //    float x = d / (Mathf.Cos(Mathf.Deg2Rad * angleToDown));
    //    Vector3 result= transform.position + transform.forward * x;
    //    primitiveCube.transform.position = result;
    //    return result;

    //}

    public void follow()
    {
        //Set position according to relative position
        Vector3 wantedPosition = relativePosition + target.transform.position;



        //if camera rotator is active then doesn't lerp directly change position because lerping 
        //makes camera to looses its focus 
        //if (Input.GetAxis(axis) != 0)
        //{

        //    transform.position = wantedPosition;
        //}
        //else
        //{
            //Debug.Log("LERPİNG");
            transform.position = Vector3.Lerp(transform.position, wantedPosition, damper * Time.deltaTime);
            ghost = transform.position - relativePosition;

        //}

        transform.LookAt(ghost);

        ////Preserve euler angles x
        //transform.rotation = Quaternion.Euler(30, transform.eulerAngles.y, 0);
    }
    

    public void updateRelative()
    {
        //Debug.Log("Updating relative");
        //Vector3 playerMove = target.transform.position + relativePosition - transform.position;

        //Debug.Log("UPDATING RELATIVE");
        //relativePosition = transform.position - target.transform.position;
        relativePosition = transform.position - ghost;
    }

    public void updateTarget()
    {
        //Debug.Log("UPDATE IS CALLED");
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
            //Debug.Log("Lerping");
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


    public void lockCameraRotation(bool b)
    {
        lockCamRotation = b;
    }

}
