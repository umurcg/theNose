using UnityEngine;
using System.Collections;

public class FeedCat : MonoBehaviour, IClickAction {

	GameObject nose;
	GameObject player;
	public GameObject playerNose;
	NavMeshAgent nma;
	RandomWalkBot rwb;

	// Use this for initialization
	void Start () {
		nose=transform.GetChild(0).gameObject;
		player = GameObject.FindGameObjectWithTag ("Player");
		rwb = GetComponent<RandomWalkBot> ();
		nma = GetComponent<NavMeshAgent> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Action(){
		
		playerNose.SetActive (false);
		nose.SetActive (true);
		rwb.enabled = false;
		nma.Stop ();
	}

}
