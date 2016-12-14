using UnityEngine;
using System.Collections;

public class PlayerComponentController : MonoBehaviour {
	MoveTo moveto;
	CharacterControllerKeyboard cck;
	CharacterMouseLook cl;
	CharacterController cc;
	NavMeshAgent nma;
	public void StopToWalk(){
        moveto.enabled = false;

		cck.enabled = false;
		cl.enabled = false;
		cc.enabled = false;
		//nma.enabled = false;

	}

	public void ContinueToWalk(){
        //nma.enabled = true;
        moveto.enabled = true;
		cck.enabled = true;
		cl.enabled = true;

		cc.enabled = true;
	}

	// Use this for initialization
	void Awake () {
		moveto = GetComponent<MoveTo>();
		cck = GetComponent<CharacterControllerKeyboard>();
		cl = GetComponent<CharacterMouseLook> ();
		cc = GetComponent<CharacterController> ();
		nma = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
