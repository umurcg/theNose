using UnityEngine;
using System.Collections;


//This script tell parent wether or not it is visible by camera.
//Script that listens this script should have IVisibility interface

public class TellParentVisibility : MonoBehaviour {

    IVisibility[] scripts;
    Renderer rend;

	// Use this for initialization
	void Awake () {


        rend = GetComponent<Renderer>();
        if (!rend)
        {
            return;
        }

        scripts = transform.parent.GetComponents<IVisibility>();


        if (!rend.isVisible)
        {
            foreach (IVisibility s in scripts) s.onInvisible();
        }
    }
	


    private void OnBecameVisible()
    {
#if UNITY_EDITOR
        if (Camera.current && Camera.current.name == "SceneCamera")
            return;
#endif

        foreach (IVisibility s in scripts) s.onVisible();
        

    }

    private void OnBecameInvisible()
    {

#if UNITY_EDITOR
        if (Camera.current && Camera.current.name == "SceneCamera")
            return;
#endif
        foreach (IVisibility s in scripts) s.onInvisible();



    }

}
