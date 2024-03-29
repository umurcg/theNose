﻿using UnityEngine;
using System.Collections;



//_StopAndStartTrigger.cs
//_Depentent to: _PlayerComponentController, AnimationController

//This script stops character movements and triggers an animation of character when one of the Action methods are called.
//It stops main character with using _PlayerComponentController.
//If you add MoveToHere script to same gameObject then it moves the player(or object that owner of entered collider) specified position.
//If you check one time use boolean then it will disable its functionality when it is used first time.


public class StopAndStartAnimation : MonoBehaviour , IEnterTrigger,IClickAction{
	public float getUpTime;
	Animator objAnim;
	PlayerComponentController pcc;
	float timer;
    public string animBool;

	public Vector3 offset_MoveToHere;

	bool onStop=false;

	bool clickable=true;
	public bool oneTimeUse=false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (timer > 0) {
			timer -= Time.deltaTime;
			if (timer <= 0)
				getUp ();

		} 

		if (getUpTime==0&&onStop) {
			if (Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0) {
				getUp ();
			} else if (Input.GetMouseButtonDown (0)) {

				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast (ray, out hit)) {
					if (hit.transform.tag == "Floor")
						getUp ();
				}

			}
		}



	}


	public void Action(){
		if (clickable) {
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			objAnim = player.GetComponent<Animator> ();
			objAnim.SetTrigger (animBool);
			pcc = player.GetComponent<PlayerComponentController> ();
			Invoke ("StopToWalk", 0.5f);
			if (getUpTime > 0) {
				timer = getUpTime;


			}
			clickable = false;
		}

	}

	public void TriggerAction(Collider collider){
		if(collider.tag.Equals("Player")){
			objAnim = collider.GetComponent<Animator> ();
			objAnim.SetTrigger(animBool)	;
			pcc = collider.GetComponent<PlayerComponentController> ();
			Invoke("StopToWalk",0.5f);
			if (getUpTime > 0) {
				timer = getUpTime;


			}
		}

	}

    public void exitTriggerAction(Collider col)
    {

    }

	void StopToWalk(){
		onStop = true;
		pcc.StopToWalk ();

		MoveToHere mth = GetComponent<MoveToHere> ();
		if (mth != null) {
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			mth.Move (offset_MoveToHere, player);
		}


	}
	void ContinueToWalk(){
		onStop = false;
		pcc.ContinueToWalk();
		clickable = !oneTimeUse;

	}



	public void getUp(){
		if (objAnim != null) {
	

			objAnim.SetTrigger(animBool);
			Invoke("ContinueToWalk",0.3f);


		}
	}

}
