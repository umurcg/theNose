using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour {

    CharacterController cc;
    public float rotateSpeed=1f;
    public float speed=0.25f;
    public float elevSpeed = 0.5f;
    public float maxHeight,maxX,maxZ = 70f;
    public float minHeight,minX,minZ = 0f;
    public float camSizeFactor=1; //One height -> x Size
    public float camSizeChangeLimit = 10f;

    [HideInInspector]
    public bool pauseMovement;
    [HideInInspector]
    public bool pauseLimits;

    float firstCamSize;
    Camera cam;
    CameraRotator rotator;

    ////TODO
    //public enum direction {posFor,negFor,posRigh,negRigh,posUp,negUp };
    //public direction forward;
    //public direction up;
    //public direction right;

    //const float degreeLimit = 5f;
    

	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();
        cam = Camera.main;
        firstCamSize = cam.orthographicSize;
        rotator = cam.gameObject.GetComponent<CameraRotator>();
	}
	
	// Update is called once per frame
	void Update () {

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        float elev = Input.GetAxis("Elevation");
        float xAngle = transform.rotation.eulerAngles.x;
        float zAngle = transform.rotation.eulerAngles.z;

        Debug.Log("hor: " + hor + " ver: " + ver + "elev: " + elev);

        //Pause movement
        if (pauseMovement)
        {
            ver = 0;
            elev = 0;
        }

        //Prevent backward movement
        if (ver < 0)
            ver = 0;


        //If camera is rotating you should prevent elevation
        if (rotator != null)
        {
            if (rotator.rotating)
            {
                elev = 0;
            }
        }

        //Move
        cc.Move(transform.forward * ver * speed);

        //Rotate around up axis
        transform.Rotate(Vector3.up * hor, Space.World);

        //Set camera zoom
        float calculatedSize = calculateCamSize(transform.position.y);
        if(cam!=null)
        {

            if (cam.orthographicSize != calculatedSize)
            {
                cam.orthographicSize = calculatedSize;
            }
        }

        //Check rotation glitch
        preserveRotationZ(zAngle);

        //Limit height
        //float prevHeight = transform.position.y;
        //if (shouldLockElevation(xAngle))
        //{
        //    transform.position =new Vector3(transform.position.x, prevHeight , transform.position.z);
        //}


        //Limits
        if (!pauseLimits)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;
            if (x > maxX) { x = maxX; } else if (x < minX) { x = minX; }
            if (y > maxHeight) { y = maxHeight; } else if (y < minHeight) { y = minHeight; }
            if (z > maxZ) { z = maxZ; } else if (z < minZ) { z = minZ; }
            transform.position = new Vector3(x, y, z);
        }

        //Change head angle for elevation
        if (elev == 0)
        {
            if (xAngle < 180f)
            {
                if (xAngle > 1f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * Quaternion.Euler(-1, 0, 0), Time.deltaTime * 40);
                }
                else if (xAngle > 0)
                {
                    transform.rotation = transform.rotation * Quaternion.AngleAxis(-xAngle, transform.right);
                }
            }
            else
            {
                if (xAngle < 359)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * Quaternion.Euler(1, 0, 0), Time.deltaTime * 40);
                }
                else if (xAngle < 360)
                {
                    transform.rotation = transform.rotation * Quaternion.AngleAxis((360 - xAngle), transform.right);
                }

            }

        } else 
        {
            if(  ((xAngle>360-45||xAngle==0) || xAngle<45) || ((xAngle < 360 - 45 && xAngle>180 && elev < 0) || (xAngle > 45 && xAngle < 180 && elev > 0)))   
            transform.Rotate(-elev, 0, 0);

        }
        

	}

    //xAngle is value of angle of head. It sepcify wether or not object is trying to elevate.
    bool shouldLockElevation(float xAngle)
    {
        if ((xAngle > 180 && transform.position.y < minHeight) || (xAngle > 0 && xAngle < 180 && transform.position.y > maxHeight))
        {
            //print(xAngle);
            return true;
        }        
        return false;
        
    }

    float calculateCamSize(float height)
    {
        if (height > camSizeChangeLimit) 
            return firstCamSize + (height-camSizeChangeLimit) * camSizeFactor;
        return firstCamSize;
    }

    void preserveRotationZ(float zAngle)
    {
        //print(zAngle);
        if (zAngle != 0)
        {

            transform.Rotate(new Vector3(0, 0, -zAngle));
        }
    }


}
