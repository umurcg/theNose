using UnityEngine;
using System.Collections;

public class AlwaysLookTo : MonoBehaviour {

    //This script make object always look to aim.
    //You can use a game oobject for aim or aimPos. GameObject will have priotrize.
    //But why dont you just disable it.

    public GameObject aim;
    public Vector3 aimPos;
    public float speed=1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

       Vector3 localAim = aimPos;
        if (aim != null)
        {
            localAim = aim.transform.position;
        }

        localAim.y = transform.position.y;


       // print(localAim);
        Quaternion aimRot = Quaternion.LookRotation( localAim- transform.position);


        
        transform.rotation = Quaternion.Slerp(transform.rotation, aimRot, speed*Time.deltaTime);


    }


}

