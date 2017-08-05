using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TellMeVisibility : MonoBehaviour {

    IVisibility script;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setScript(IVisibility script) { this.script = script; }

    private void OnBecameVisible()
    {

        if (script == null)
        {
            //Debug.Log("No ivisibility interface");
            return;
        }
        script.onVisible();


    }

    private void OnBecameInvisible()
    {

 
        if (script == null)
        {
            //Debug.Log("No ivisibility interface");
            return;
        }
        script.onInvisible();



    }
}
