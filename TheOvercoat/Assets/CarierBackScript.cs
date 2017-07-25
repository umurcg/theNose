using UnityEngine;
using System.Collections;

//Followes carier front object with lerp. In old version you were yousing hinge joitn. But it was causing vibration so lets try this mother fucker.
public class CarierBackScript : MonoBehaviour {

    public GameObject frontObject;
    Vector3 relativePos;
    Quaternion relativeAngle;
    public float linearDamper=0.5f;
    public float angularDamper=0.5f;

    public bool linearLerp = false;
    public bool angularLerp = true;
    
	// Use this for initialization
	void Start () {
        relativePos = transform.position - frontObject.transform.position;
        relativeAngle = Quaternion.FromToRotation( frontObject.transform.forward , transform.forward);
	}
	
	// Update is called once per frame
	void Update () {

        var wantedPos = relativePos + frontObject.transform.position;
        var wantedRot = frontObject.transform.rotation * relativeAngle;

        

        if (linearLerp)
        {
            transform.position = Vector3.Lerp(transform.position, wantedPos, linearDamper * Time.deltaTime);
        }
        else
        {
            transform.position =wantedPos;

        }

        if (angularLerp)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, wantedRot, angularDamper * Time.deltaTime);
        }
        else
        {
            transform.rotation =wantedRot;
        }

  
      



	}
}
