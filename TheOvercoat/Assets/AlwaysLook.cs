using UnityEngine;
using System.Collections;

public enum aimType {vector,gameObject };

public class AlwaysLook : MonoBehaviour {

    public aimType type;
    public GameObject aimObject;
    public Vector3 aim;
 

	// Use this for initialization
	void Start () {
	  
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 aim = Vector3.zero;

        if (type == aimType.gameObject)
        {
            aim =aimObject. transform.position;
        }

        aim.y = transform.position.y;

        transform.rotation = Quaternion.LookRotation(aim - transform.position);

	}
}
