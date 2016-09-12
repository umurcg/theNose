using UnityEngine;
using System.Collections;

public class OnEnterTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider collider) {
		IEnterTrigger iet = GetComponent<IEnterTrigger> ();
		iet.TriggerAction (collider);

	}



}
