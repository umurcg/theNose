using UnityEngine;
using System.Collections;

public class DrawerScript : MonoBehaviour, IClickAction {
	GameObject player;
	Animator acPlayer;
	PlayerComponentController pcc;
	float timer=0;
	public float holdTime=5;
	bool used=false;
	ObjectChildSpawn ocs;
	public GameObject door;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		acPlayer = player.GetComponent <Animator>();
		pcc = player.GetComponent <PlayerComponentController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (timer > 0) {
			timer -= Time.deltaTime;

			if (timer <= 0) {
				acPlayer.SetBool ("Hands",false);
			
				callSpawn ();
				timer = 0;
			}
		}
	
	}
	void callSpawn(){

				ocs.Spawn ();
		pcc.ContinueToWalk();
	
		door.GetComponent<ChangeMaterial> ().change();
		door.GetComponent<KeySlideCompletely> ().enabled = true;
		door.transform.tag = "ActiveObject";
	    
	}

	public void Action(){
		if (player != null&&used==false) {
			used = true;
			ocs = player.GetComponentInChildren<ObjectChildSpawn> ();
			if (ocs != null) {
				timer = 5;
				acPlayer.SetBool ("Hands", true);
				pcc.StopToWalk ();
			}
		}
	}



}
