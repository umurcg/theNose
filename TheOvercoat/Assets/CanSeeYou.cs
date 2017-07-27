using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script sends message when it sees an object having specific layer

public class CanSeeYou : MonoBehaviour {

    public List<string> layers;
    public float seeAngle=60f;
    public int numberOfRay = 2;
    public float lengthOfRays = 10f;
    public GameObject headPos;

    public GameObject reciever;
    public string message;

    public bool destroyAfterMessage = false;
    public bool disableAfterMessage = false;
    public bool sendWithHittedObject = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {


        float angleBetweenRays = seeAngle / numberOfRay;

        //Debug.Log(numberOfRay);
        for (int i = -numberOfRay/2; i < numberOfRay/2+1; i++)
        {
            if (numberOfRay % 2 == 0 && i == 0) continue;

            Vector3 startPos = headPos.transform.position;

            Vector3 direction = headPos.transform.forward * Mathf.Cos(Mathf.Deg2Rad * angleBetweenRays*i) + headPos.transform.right * Mathf.Sin(Mathf.Deg2Rad * angleBetweenRays*i);
            direction = direction.normalized;

            Ray ray = new Ray(startPos,direction );
            Debug.DrawRay(startPos, direction*lengthOfRays, Color.red);
            //Debug.Log(direction);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, lengthOfRays))
            {
         
                if (layers.Contains(hit.transform.tag)) iCanSeeYou(hit.transform.gameObject);
            }

        }

        //Debug.DrawRay(transform.position, transform.right, Color.red);
        //Debug.DrawRay(transform.position, transform.forward, Color.red);
    }

    void iCanSeeYou(GameObject hitObj)
    {
        //Debug.Log("Gotchya "+hitObj.transform.name);
        if (reciever != null)
        {
            if (sendWithHittedObject)
            {
                reciever.SendMessage(message,hitObj);
            }
            else
            {
                reciever.SendMessage(message);
            }
        }
        if (destroyAfterMessage) Destroy(this);
        if (disableAfterMessage) enabled = false;
    }
}
