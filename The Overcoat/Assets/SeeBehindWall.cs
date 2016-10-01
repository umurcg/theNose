using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SeeBehindWall : MonoBehaviour {
	Material originalMat;
	public Material transparent;
	GameObject player;
	bool isTransparent=false;
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
		if (isTransparent == false) {
			isTransparent = true;


			color.a = 1;
			transparent.color = color;
			rend.material = transparent;


		}


	}

	void makeSolid(){
		if (isTransparent) {
			isTransparent = false;



		}
	}

	// Update is called once per frame
	void Update () {

		if(rayCast() || rayCastTargets()){
			makeTransparent ();
		}
		else{
			makeSolid ();
		}
		
      
		if (targetObjects.Length > 0)
			rayCastTargets();
			

		if (isTransparent && color.a != 0) {
			color.a -= Time.deltaTime * speed;
			transparent.color = color;
			if (color.a <= 0) {
				color.a = 0;
			}

		} else if (!isTransparent && color.a != 1) {
			color.a += Time.deltaTime * speed;
			transparent.color = color ;


			if (color.a >= 1) {
				color.a =1;
				originalMat.color = color;
				rend.material = originalMat;
			}

		}

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
