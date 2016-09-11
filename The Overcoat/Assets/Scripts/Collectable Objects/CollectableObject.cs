using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CollectableObject : MonoBehaviour {
	public static List<GameObject> collected;
	// Use this for initialization
	public bool onHand=false;

	void Start () {
		if (CollectableObject.collected == null)
			CollectableObject.collected = new List<GameObject> ();
	
	}
	
	// Update is called once per frame

	void Collect(){
		CollectableObject.collected.Add (gameObject);
		gameObject.active = false;


	}

	public void UnCollect(Vector3 position){
		collected.Remove (gameObject);
		transform.position = position;
		gameObject.active = true;

	}

	void Update () {


		if (Input.GetMouseButtonUp (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit)) {
				
				if (hit.transform == transform) {
					
					Collect ();
				}

			}
	
		}
	}
}
