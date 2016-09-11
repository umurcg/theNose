using UnityEngine;
using System.Collections;

public class ObjectUncollectTrigger : MonoBehaviour {
	CollectableObject co;
	public GameObject obj;

	public bool canBeCollectedAgain=false;
	// Use this for initialization
	void Awake () {

		co=obj.GetComponent<CollectableObject> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit)) {

				if (hit.transform == transform) {

					if(CollectableObject.collected.Contains(obj)){
						
						co.UnCollect (transform.position);
						if (canBeCollectedAgain == false)
							co.enabled = false;

						
					
				}

			}

		}
	}
}
}