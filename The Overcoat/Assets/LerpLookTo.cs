using UnityEngine;
using System.Collections;

public class LerpLookTo : MonoBehaviour {

	public Transform aimObject;
	public Vector3 aim;
	public float speed=1;
	public float rotTolerance = 0;
	Quaternion initialRot;
	Quaternion aimRot;
	float ratio;

	//public bool debug=false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	//	if (debug) {
	//		StartCoroutine ("LookToLerp");
	//		debug = false;
	//	}
	}

	public IEnumerator LookToLerp(){
		Vector3 localAim = aim;
		if (aimObject != null) {
			localAim = aimObject.transform.position;
		}
	
		initialRot = transform.rotation;
		aimRot = Quaternion.LookRotation (localAim);
		ratio = 0;
	
	
		while (Vector3.Distance (transform.rotation.eulerAngles, aimRot.eulerAngles) > rotTolerance) {

			ratio += Time.deltaTime*speed;
			transform.rotation = Quaternion.Lerp (initialRot, aimRot, ratio);
	
			yield return null;
		}
		transform.rotation = aimRot;
	
	}
}
