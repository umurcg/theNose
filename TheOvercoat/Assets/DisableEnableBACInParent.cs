using UnityEngine;
using System.Collections;


//This script disables and enables of parent's basic chare animcontroller and animator when it gets out from screen or gets in to screen
public class DisableEnableBACInParent : MonoBehaviour {

    Animator parentAnim;
    BasicCharAnimations bca;
    Renderer rend;

	// Use this for initialization
	void Start () {

        rend = GetComponent<Renderer>();
        if(!rend)
        {
            return;
        }


        if (transform.parent == null)
        {
            enabled = false;
            return;
        }

        parentAnim = transform.parent.GetComponent<Animator>();
        bca = transform.parent.GetComponent<BasicCharAnimations>();

        if(!bca || !parentAnim)
        {
            enabled = false;
            return;
        }


        if (!rend.isVisible)
        {
            if (parentAnim) parentAnim.enabled = false;
            if (bca) bca.enabled = false;
        }
    }

    private void OnBecameVisible()
    {
#if UNITY_EDITOR
        if (Camera.current && Camera.current.name == "SceneCamera")
            return;
#endif

        //Debug.Log("Became visile");
        if (parentAnim) parentAnim.enabled = true;

        if (bca) bca.enabled = true;

    }

    private void OnBecameInvisible()
    {

#if UNITY_EDITOR
        if (Camera.current && Camera.current.name == "SceneCamera")
            return;
#endif

        //Debug.Log("Became invisible");

        if (parentAnim) parentAnim.enabled = false;

        if (bca) bca.enabled = false;

    }



}
