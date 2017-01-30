using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//_SeeBehindWall.cs
//Dependent to: Renderer, Material
//This scripts makes transparent objects that are between camera and target object or player.For usage originalMaterial and tranparent material must be assigned.Also target objects must assign too.

public class SeeBehindWall : MonoBehaviour {
	public Material originalMat;
	public Material globalTrans;
	Material transparent;
	GameObject player;
	bool isTransparent=false;
	Renderer rend;
	public float speed=0.01f;
	public GameObject[] targetObjects;
	bool makeTransparentBool=false;
    bool mouseIsOver = false;




	Color color;
	// Use this for initialization


	void Awake () {
        if (Camera.main == null)
        {
            this.enabled = false;
        }

		player = GameObject.FindGameObjectWithTag ("Player");
	    rend = GetComponent<Renderer> ();
		rend.material = globalTrans;
		createTransparent ();


		color.r = 1;
		color.g = 1;
		color.b = 1;
	}


	void createTransparent(){


		transparent = rend.material;
//		transparent = rend.material;
//		transparent.SetFloat("_Mode", 3);
//		transparent.EnableKeyword ("_NORMALMAP");
//		transparent.EnableKeyword ("_ALPHAPREMULTIPLY_ON");
	
	
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

		if(rayCastTargets()||mouseIsOver){
			makeTransparent ();
		}
		else{
			makeSolid ();
		}
					

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

    
	bool isWallBetweenCameraAndTarget(Transform target){
		RaycastHit[] hits = Physics.RaycastAll (Camera.main.transform.position, target.transform.position - Camera.main.transform.position);
		for (int i = 0; i < hits.Length; i++) {
			if (hits [i].transform == transform)
				return true;
		}
		return false;

	}

	bool rayCastTargets(){

		for (int i = 0; i < targetObjects.Length ; i++) {
            if (targetObjects[i] != null)
            {
                if (isWallBetweenCameraAndTarget(targetObjects[i].transform))
                    return true;
            }

		}
		return false;
	}



	bool rayCast(){
		
		return isWallBetweenCameraAndTarget(player.transform);
        	
  }

   void OnMouseEnter()
    {
        if (!mouseIsOver)
            mouseIsOver = true;
    }

    void OnMouseExit()
    {
        if (mouseIsOver)
            mouseIsOver = false;
    }


}
