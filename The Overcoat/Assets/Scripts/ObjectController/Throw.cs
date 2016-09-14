	using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {

	public Vector3 force;
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.AddForce (force, ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
