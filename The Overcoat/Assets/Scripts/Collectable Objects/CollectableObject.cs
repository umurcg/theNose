using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CollectableObject : MonoBehaviour, IClickAction {
	public static List<GameObject> collected;
	// Use this for initialization
	public bool onHand=false;
	Transform rightHand;
	Transform leftHand;

	Transform player;

	public GameObject[] placeholders;

	public bool canBeCollectedAgain=false;


	Transform parent;

	void Start () {
		if (CollectableObject.collected == null)
			CollectableObject.collected = new List<GameObject> ();
		parent = transform.parent;

		player = GameObject.FindGameObjectWithTag ("Player").transform;
		rightHand = player.Find ("Armature/Torso/Chest/Arm_R/Hand_R");
		leftHand = player.Find ("Armature/Torso/Chest/Arm_L/Hand_L");

		foreach (GameObject placeholder in placeholders){
			placeholder.active =false;
		}
	}
	
	// Update is called once per frame

	void Collect(){
		CollectableObject.collected.Add (gameObject);
		if (onHand == false) {
			gameObject.active = false;
		} else {

			if (rightHand.childCount == 0) {
				
				transform.parent = rightHand;

				transform.localPosition = new Vector3 (-0.60f, 0, 0);
			} else if (leftHand.childCount == 0) {
				transform.parent = leftHand;

				transform.localPosition = new Vector3 (-0.60f, 0, 0);

			} else {

				rightHand.GetChild (0).gameObject.active = false;
				transform.parent = rightHand;
				transform.localPosition = new Vector3 (-0.60f, 0, 0);

			}



		}

		foreach (GameObject placeholder in placeholders){
			placeholder.active =true;
		}

	}

	public void UnCollect(Vector3 position){
		transform.parent = parent;
		collected.Remove (gameObject);
		transform.position = position;
		gameObject.active = true;
		foreach (GameObject placeholder in placeholders){
			placeholder.active =false;
		}

		if (canBeCollectedAgain == false) {
			gameObject.GetComponent<ClickTrigger> ().enabled = false;
			this.enabled = false;
		}

	}

	public void Action(){
		
		Collect ();
	}

	void Update () {


//		if (Input.GetMouseButtonUp (0)) {
//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//			RaycastHit hit;
//
//			if (Physics.Raycast (ray, out hit)) {
//				
//				if (hit.transform == transform) {
//					
//					Collect ();
//				}
//
//			}
//	
//		}
	}
}
