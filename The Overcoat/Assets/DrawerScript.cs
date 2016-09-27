using UnityEngine;
using System.Collections;

public class DrawerScript : MonoBehaviour, IClickAction {
	GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Action(){

		if (player != null) {

			ObjectChildSpawn ocs = player.GetComponentInChildren<ObjectChildSpawn> ();
			if(ocs!=null)
			ocs.Spawn ();

//			for (int i = 0; i < player.transform.childCount; i++) {
//				Transform child = player.transform.GetChild (i);
//				if (child.name == "Nose") {
//					ObjectChildSpawn ocs = child.GetComponent<ObjectChildSpawn> ();
//					ocs.Spawn ();
//				}
//			}
		}

	}

}
