using UnityEngine;
using System.Collections;

public class ClickAndDragNormal : MonoBehaviour {

	bool touched=false;
	float speed=10f;




	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {

			if (touched) {

				if (Input.GetAxis ("Mouse X") !=0|| Input.GetAxis ("Mouse Y")!=0) {
					transform.position += (transform.up/3-transform.forward)* Time.deltaTime*speed;
				}

			}

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform == transform) {
					print ("You are touching to object");
			
					touched = true;
				}
			}
		

		}

		if (Input.GetMouseButtonUp (0)) {

			touched = false;
		}

	}
}
