using UnityEngine;
using System.Collections;

//_RotationAnimation.cs
//_Dependent to: Animator

//This script trigger set Rotation float of AC according to rotation of object.

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
		
		if (prevRot!=Vector3.zero) {
			float rot=transform.rotation.eulerAngles.y - prevRot.y;
			ac.SetFloat("Rotation",rot);
			print (rot);
			

		}

		prevRot = transform.rotation.eulerAngles;

	}
}
