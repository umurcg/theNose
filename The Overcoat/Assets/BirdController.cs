using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour {

    CharacterController cc;
    public float rotateSpeed=1f;
    public float speed=0.25f;
    public float elevSpeed = 0.5f;
    public float maxHeight = 70f;
    public float minHeight = 0f;
    
    //TODO
    public enum direction {posFor,negFor,posRigh,negRigh,posUp,negUp };
    public direction forward;
    public direction up;
    public direction right;

    

	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();
 
	}
	
	// Update is called once per frame
	void Update () {

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        float elev = Input.GetAxis("Elevation");
        float xAngle = transform.rotation.eulerAngles.x;

        bool eliminateHeight = false;
        if ((xAngle > 0 && xAngle<180 && transform.position.y < minHeight) || (xAngle < 360 && xAngle > 180 && transform.position.y > maxHeight))
        {
            eliminateHeight = true;
        }

        //print(eliminateHeight);

        float prevHeight = transform.position.y;
        cc.Move( -transform.forward*ver*speed);

        if (eliminateHeight)
        {
            transform.position =new Vector3(transform.position.x, prevHeight , transform.position.z);
        }

        transform.Rotate(Vector3.up*hor,Space.World);

  

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
}
