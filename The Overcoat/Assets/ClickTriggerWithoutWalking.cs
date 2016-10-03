using UnityEngine;
using System.Collections;

public class ClickTriggerWithoutWalking : MonoBehaviour {
	protected IClickAction ic;
	// Use this for initialization
	protected void Start () {
		ic = GetComponent<IClickAction> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void OnMouseDown(){
		ic.Action ();
	}
}
