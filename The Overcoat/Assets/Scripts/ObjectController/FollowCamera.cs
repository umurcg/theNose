using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	Vector3 reference;
	Camera cam;
	public Vector3 initialCamPosition;
	// Use this for initialization
	void Start () {
		cam = Camera.main;
		reference = transform.position - initialCamPosition;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = cam.gameObject.transform.position + reference;
	}
}
