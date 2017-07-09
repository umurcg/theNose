using UnityEngine;
using System.Collections;

public class PlayerComponentController : MonoBehaviour {
	MoveTo moveto;
	CharacterControllerKeyboard cck;
	CharacterMouseLook cl;
	CharacterController cc;
	UnityEngine.AI.NavMeshAgent nma;

    bool bcanPlayerWalk = true;

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

        bcanPlayerWalk = false;

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

        bcanPlayerWalk = true;
        
	}

	// Use this for initialization
	void Awake () {
		moveto = GetComponent<MoveTo>();
		cck = GetComponent<CharacterControllerKeyboard>();
		cl = GetComponent<CharacterMouseLook> ();
		cc = GetComponent<CharacterController> ();
		nma = GetComponent<UnityEngine.AI.NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool canPlayerWalk()
    {
        return bcanPlayerWalk;

    }

    public void pauseNma()
    {
        nma.Stop();
    }

    public void disableNma()
    {
        pauseNma();
        nma.enabled = false;
    }

    public void enableNma()
    {
        nma.enabled = true;
        nma.Resume();
    }
}
