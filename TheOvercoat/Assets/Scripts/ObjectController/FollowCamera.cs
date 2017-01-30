using UnityEngine;
using System.Collections;



public class FollowCamera : MonoBehaviour {
	Vector3 reference;
	Camera cam;
	public Vector3 initialCamPosition;
	// Use this for initialization
	void Start () {
		cam = Camera.main;
		if(initialCamPosition!=Vector3.zero){
		reference = transform.position - initialCamPosition;
		}else{
			reference = transform.position - cam.transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = cam.gameObject.transform.position + reference;
	}
}
