using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SeeBehindWallver2 : MonoBehaviour {
	Material originalMat;
	public Material transient;
	public Material transparent;

	GameObject player;

	bool up=false;
	bool isTransient=false;


	Renderer rend;
	public float speed=0.01f;
	public GameObject[] targetObjects;
	bool makeTransparentBool=false;





	Color color;
	// Use this for initialization


	void Start () {


		player = GameObject.FindGameObjectWithTag ("Player");


		rend = GetComponent<Renderer> ();
		originalMat = rend.material;
		color.r = 1;
		color.g = 1;
		color.b = 1;
	}

	void makeTransparent(){

		up = false;
		isTransient = true;
		color.a = 1;
		transient.color = color;
		rend.material = transient;


	}

	void makeSolid(){
		up = true;
		isTransient = true;
		color.a = 0;
		transient.color = color;
		rend.material = transient;

	}

	// Update is called once per frame
	void Update () {

		if(rayCast() || rayCastTargets()){
			if(rend.material==originalMat)
			makeTransparent ();
		}
		else{
			if(rend.material==transparent)
			makeSolid ();
		}
		
      
		if (isTransient) {
			if (up) {
				color.a += Time.deltaTime * speed;
				transient.color = color;
				if (color.a <= 0) {
					rend.material = transparent;
					isTransient = false;
					color.a = 0;
				}
			} else {
				color.a -= Time.deltaTime * speed;
				transient.color = color;
				if (color.a >= 1) {
					color.a = 1;
					isTransient = false;
					rend.material = originalMat;
				}
			}

		}

//
//		if (isTransient && color.a != 0) {
//			color.a -= Time.deltaTime * speed;
//			transparent.color = color;
//			if (color.a <= 0) {
//				color.a = 0;
//			}
//
//		} else if (!isTransient && color.a != 1) {
//			color.a += Time.deltaTime * speed;
//			transparent.color = color ;
//
//
//			if (color.a >= 1) {
//				color.a =1;
//				originalMat.color = color;
//				rend.material = originalMat;
//			}
//
//		}

	}

	bool rayCastTargets(){

		bool b=false;
		for (int i = 0; i < targetObjects.Length ; i++) {

			RaycastHit hitPoint;
	

			if (Physics.Raycast (Camera.main.transform.position, targetObjects[i].transform.position - Camera.main.transform.position, out hitPoint)) {
				
				if (hitPoint.transform == transform) {
					
					b = true;
				} 
			}

		}
		return b;
	}



	bool rayCast(){
		
		RaycastHit hitPoint;
        
	
		if (Physics.Raycast (Camera.main.transform.position, player.transform.position - Camera.main.transform.position, out hitPoint)) {

			if (hitPoint.transform == transform) {
								
				return true;
			} else {
				return false;
			}

		}
		return false;

	
  }

}
