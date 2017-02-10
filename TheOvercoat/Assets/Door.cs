using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class Door : MonoBehaviour {

    public float speed = 40f;
    public float maxAngle = 70f;
    public GameObject turnAxis;

    Quaternion closeRotation;
    IEnumerator<float> openDoorHandler;


	// Use this for initialization
	void Start () {
        closeRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void openDoor()
    {
        openDoorHandler= Timing.RunCoroutine(_openDoor());
    }

    IEnumerator<float> _openDoor()
    {
        
        while (Quaternion.Angle(closeRotation,transform.rotation)<maxAngle) {
            float dif=Quaternion.Angle(closeRotation, transform.rotation);
            transform.RotateAround(turnAxis.transform.position,turnAxis.transform.forward, speed * Timing.DeltaTime * Mathf.Sin(Mathf.Deg2Rad*dif+10));
            yield return 0;
        }
        yield break;
    }

    public void closeDoor()
    {
        Timing.KillCoroutines(openDoorHandler);
        Timing.RunCoroutine(_closeDoor());
    }

    IEnumerator<float> _closeDoor()
    {
   
        while (Quaternion.Angle(closeRotation, transform.rotation)>0.5f)
        {
            float dif = Quaternion.Angle(closeRotation, transform.rotation);
            //Debug.Log(dif);
            transform.RotateAround(turnAxis.transform.position, turnAxis.transform.forward,- speed * Timing.DeltaTime * Mathf.Sin(Mathf.Deg2Rad * dif + 10));
            yield return 0;
        }
        transform.rotation = closeRotation;
        yield break;
    }
}
