using UnityEngine;
using System.Collections;

public class ClickTriggerWithoutWalkingPlayerInAnObject : ClickTriggerWithoutWalking {
	public GameObject obj;
	SpaceTrigger st;
	NavMeshAgent nma;
	// Use this for initialization
	new void Start () {
		base.Start();
		st=obj.GetComponent<SpaceTrigger>();
		nma.GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public override void OnMouseDown(){
		if (st.colliding) {

			base.ic.Action();
			print ("fuck");
			nma.SetDestination (transform.position);

		}
	}
}
