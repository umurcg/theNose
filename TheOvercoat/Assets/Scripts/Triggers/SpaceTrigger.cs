using UnityEngine;
using System.Collections;

public class SpaceTrigger : MonoBehaviour {
	IClickAction iclick;
	public bool colliding;
	// Use this for initialization
	void Start () {
		iclick = GetComponent<IClickAction> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Input.GetKeyDown(Interaction)
		if (Input.GetKeyDown(KeyCode.Space)&&colliding) {
			iclick.Action ();
		}
        


	}

	void OnTriggerEnter(Collider col){
		colliding = (col.tag == "Player");
	}

	void OnTriggerExit(Collider col){
		colliding = !(col.tag == "Player");
	}

    void OnMouseDown()
    {
        if(colliding&&iclick!=null)
        iclick.Action();
    }


}
