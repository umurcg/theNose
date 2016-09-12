using UnityEngine;
using System.Collections;

public class PlayerComponentController : MonoBehaviour {
	MoveTo moveto;
	CharacterControllerKeyboard cck;
	CharacterMouseLook cl;
	CharacterController cc;

	public void StopToWalk(){
		moveto.Stop ();
		moveto.enabled = false;
		cck.enabled = false;
		cl.enabled = false;
		cc.enabled = false;

	}

	public void ContinueToWalk(){
		moveto.enabled = true;
		cck.enabled = true;
		cl.enabled = true;

		cc.enabled = true;
	}

	// Use this for initialization
	void Start () {
		moveto = GetComponent<MoveTo>();
		cck = GetComponent<CharacterControllerKeyboard>();
		cl = GetComponent<CharacterMouseLook> ();
		cc = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
