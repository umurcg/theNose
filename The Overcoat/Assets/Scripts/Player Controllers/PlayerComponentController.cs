using UnityEngine;
using System.Collections;

public class PlayerComponentController : MonoBehaviour {
	MoveTo moveto;
	CharacterControllerKeyboard cck;
	CharacterLook cl;

	public void StopToWalk(){
		moveto.Stop ();
		moveto.enabled = false;
		cck.enabled = false;
		cl.enabled = false;

	}

	public void ContinueToWalk(){
		moveto.enabled = true;
		cck.enabled = true;
		cl.enabled = true;
	}

	// Use this for initialization
	void Start () {
		moveto = GetComponent<MoveTo>();
		cck = GetComponent<CharacterControllerKeyboard>();
		cl = GetComponent<CharacterLook> ();
		}
	
	// Update is called once per frame
	void Update () {
	
	}
}
