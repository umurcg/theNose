﻿using UnityEngine;
using System.Collections;

public class RotateAroundObject : MonoBehaviour {

    public GameObject target;
    public float speed;

   

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.RotateAround(target.transform.position, target.transform.right , Time.deltaTime * speed);
	}
}
