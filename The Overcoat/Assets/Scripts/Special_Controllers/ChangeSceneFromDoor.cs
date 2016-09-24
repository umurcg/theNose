using UnityEngine;
using System.Collections;

public class ChangeSceneFromDoor : ChangeSceneTrigger {
	GameObject door;
	SkinnedMeshRenderer smrDoor;
	bool isTrigger;
	// Use this for initialization
	void Start () {
		door=transform.parent.GetChild (0).gameObject;
		smrDoor = door.GetComponent<SkinnedMeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (smrDoor.GetBlendShapeWeight (0) == 100) {
			changeScene ();
			this.enabled = false;
		}
	}

	void OnEnterTrigger(){
		isTrigger = true;

	}

	void OnExitTrigger(){
		isTrigger = false;
	}

}
