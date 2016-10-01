using UnityEngine;
using System.Collections;

public class RotationAnimation : MonoBehaviour {
	Animator ac;
	Vector3 prevRot;
	public float tolerance=0;
	// Use this for initialization
	void Start () {
		ac = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (prevRot != null && prevRot!=Vector3.zero) {
			float rot=transform.rotation.eulerAngles.y - prevRot.y;
			ac.SetFloat("Rotation",rot);
			print (rot);
			

		}

		prevRot = transform.rotation.eulerAngles;

	}
}
