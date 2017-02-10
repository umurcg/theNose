﻿using UnityEngine;
using System.Collections;

//This script enables and disables rigidbody


public class EnableAndDisableRigidBody : MonoBehaviour {
	Rigidbody rb;
	public bool gravity;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void enable(){

		rb = GetComponent<Rigidbody> ();

		rb.useGravity = gravity;

		rb.isKinematic = false;
	}

	public void disable(){
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
	}

}