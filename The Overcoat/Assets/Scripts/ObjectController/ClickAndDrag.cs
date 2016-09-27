﻿using UnityEngine;
using System.Collections;

public class ClickAndDrag : MonoBehaviour {

	bool touched=false;
	public float speed=10f;

	//public Vector3 offset ;


	//for child scripts


	// Use this for initialization
	protected void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {

			if (touched)
				dragObject();
			
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit)) {
				
				if (hit.transform == transform) {
			
					touched = true;
				}
			}
		

		}else if (Input.GetMouseButtonUp (0)) {

			touched = false;
		}

	}




	public virtual void dragObject(){

		//transform.position += new Vector3 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"), 0) * Time.deltaTime * speed;
			
		transform.position=Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition),Time.deltaTime*speed);

		//		if (Input.GetAxis ("Mouse X") !=0|| Input.GetAxis ("Mouse Y")!=0) {
		//			transform.position += (transform.up/3-transform.forward)* Time.deltaTime*speed;
		//		}
	}

}