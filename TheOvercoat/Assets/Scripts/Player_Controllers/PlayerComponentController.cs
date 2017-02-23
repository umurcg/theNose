using UnityEngine;
using System.Collections;

public class PlayerComponentController : MonoBehaviour {
	MoveTo moveto;
	CharacterControllerKeyboard cck;
	CharacterMouseLook cl;
	CharacterController cc;
	NavMeshAgent nma;
	public void StopToWalk(){

        if (moveto!=null)
        moveto.enabled = false;

        if(cck!=null)
		cck.enabled = false;

        if(cl!=null)
        cl.enabled = false;

        if(cc!=null)
        cc.enabled = false;
		//nma.enabled = false;

	}

	public void ContinueToWalk(){
        //nma.enabled = true;
        if (moveto != null)
            moveto.enabled = true;
        if (cck != null)
            cck.enabled = true;

        if (cl != null)
            cl.enabled = true;
        if (cc != null)
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
