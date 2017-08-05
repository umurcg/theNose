using UnityEngine;
using System.Collections;

public class AlwaysLookTo : MonoBehaviour {

    //This script make object always look to aim.
    //You can use a game oobject for aim or aimPos. GameObject will have priotrize.
    //But why dont you just disable it.
    public bool aimIsPlayer = false;

    public GameObject aim;
    public Vector3 aimPos;
    public float speed=1;

    public bool lerps = true;

	// Use this for initialization
	void Start () {
        if (aimIsPlayer) aim = CharGameController.getActiveCharacter();
    }
	
	// Update is called once per frame
	void Update () {

        //If not intilized return 
        if (aim == null && aimPos == Vector3.zero) return;

       Vector3 localAim = aimPos;
        if (aim != null)
        {
            localAim = aim.transform.position;
        }

        localAim.y = transform.position.y;


       // print(localAim);
        Quaternion aimRot = Quaternion.LookRotation( localAim- transform.position);

        if (lerps)
        {

            transform.rotation = Quaternion.Slerp(transform.rotation, aimRot, speed * Time.deltaTime);
        }
        else
        {
            transform.rotation = aimRot;
        }

    }


}

