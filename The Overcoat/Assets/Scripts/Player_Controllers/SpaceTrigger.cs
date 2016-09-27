using UnityEngine;
using System.Collections;

public class SpaceTrigger : MonoBehaviour {
	IClickAction iclick;
	bool colliding;
	// Use this for initialization
	void Start () {
		iclick = GetComponent<IClickAction> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Input.GetKeyDown(KeyCode.Space)
		if (Input.GetAxis("Interaction")==1&&colliding) {
			iclick.Action ();
		}
	}

	void OnTriggerEnter(Collider col){
		colliding = (col.tag == "Player");
	}

	void OnTriggerExit(Collider col){
		colliding = !(col.tag == "Player");
	}


}
