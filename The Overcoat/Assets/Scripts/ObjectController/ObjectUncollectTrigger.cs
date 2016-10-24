using UnityEngine;
using System.Collections;


//_ObjectUncollectTrigger.cs
//_Dependent to: _CollectableObject.cs

//This script enable playter to uncollect game object (added to script in editor) that is saved to List of collected objects.


public class ObjectUncollectTrigger : MonoBehaviour, IClickAction {
	CollectableObject co;
	public GameObject obj;


	// Use this for initialization
	void Awake () {

		co=obj.GetComponent<CollectableObject> ();

	}


	public void Action(){
			if(CollectableObject.collected.Contains(obj)){
					
				co.UnCollect (transform.position);
		
		
								
							
				}
	}
	// Update is called once per frame
	void Update () {
//		if (Input.GetMouseButtonUp (0)) {
//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//			RaycastHit hit;
//
//			if (Physics.Raycast (ray, out hit)) {
//
//				if (hit.transform == transform) {
//
//					if(CollectableObject.collected.Contains(obj)){
//						
//						co.UnCollect (transform.position);
//						if (canBeCollectedAgain == false)
//							co.enabled = false;
//
//						
//					
//				}
//
//			}
//
//		}
//	}
}
}