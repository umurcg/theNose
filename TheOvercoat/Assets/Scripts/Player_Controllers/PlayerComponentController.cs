using UnityEngine;
using System.Collections;

public class PlayerComponentController : MonoBehaviour {
	MoveTo moveto;
	CharacterControllerKeyboard cck;
	protected CharacterMouseLook cl;
	protected CharacterController cc;
	UnityEngine.AI.NavMeshAgent nma;

    protected bool bcanPlayerWalk = true;

	public virtual void StopToWalk(){

        if (moveto!=null)
        moveto.enabled = false;

        if(cck!=null)
		cck.enabled = false;

        if(cl!=null)
        cl.enabled = false;

        if(cc!=null)
        cc.enabled = false;
        //nma.enabled = false;

        if (nma && nma.isOnNavMesh) nma.isStopped = true; 

        bcanPlayerWalk = false;

	}

	public virtual void ContinueToWalk(){
        //nma.enabled = true;
        if (moveto != null)
            moveto.enabled = true;
        if (cck != null)
            cck.enabled = true;

        if (cl != null)
            cl.enabled = true;
        if (cc != null)
            cc.enabled = true;

        if (nma&& nma.isOnNavMesh) nma.isStopped = false;

        bcanPlayerWalk = true;
        
	}

	// Use this for initialization
	protected virtual void Awake () {
		moveto = GetComponent<MoveTo>();
		cck = GetComponent<CharacterControllerKeyboard>();
		cl = GetComponent<CharacterMouseLook> ();
		cc = GetComponent<CharacterController> ();
		nma = GetComponent<UnityEngine.AI.NavMeshAgent> ();
	}



    public bool canPlayerWalk()
    {
        return bcanPlayerWalk;

    }

    public virtual void pauseNma()
    {
        nma.Stop();
    }

    public virtual void disableNma()
    {
        pauseNma();
        nma.enabled = false;
    }

    public virtual void enableNma()
    {
        nma.enabled = true;
        nma.Resume();
    }
}
