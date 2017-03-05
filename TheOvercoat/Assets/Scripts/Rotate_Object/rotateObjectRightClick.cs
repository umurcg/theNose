using UnityEngine;
using System.Collections;

public class rotateObjectRightClick : MonoBehaviour {
    public float speed = 150;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (1)&&!Input.GetMouseButton (2)) {
		  transform.Rotate(new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * Time.deltaTime * speed);	
		}

		if (Input.GetMouseButton (1)&&Input.GetMouseButton (2)) {
			transform.Rotate(new Vector3(0, 0, Input.GetAxis("Mouse X")) * Time.deltaTime * speed);	
		}
	}
}
