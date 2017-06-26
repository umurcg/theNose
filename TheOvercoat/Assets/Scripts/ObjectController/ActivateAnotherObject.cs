using UnityEngine;
using System.Collections;

//This script activates another object.
//It basically enables all the scripts of an object and changes tag to ActiveObbject.
//It triggered with click or space trigger scripts.
//Also it changes material if its have changeMaterial script.

public class ActivateAnotherObject : MonoBehaviour, IFinishedSwitching, IClickAction {

    public GameObject go;

	// Use this for initialization
	void Start () {
        //Activate(go);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public static void Disable(GameObject go)
    {
        MonoBehaviour[] scripts = go.GetComponentsInChildren<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {

            script.enabled = false;


            if (script is ChangeMaterial)
            {
                ChangeMaterial cm = (ChangeMaterial)(script);
                cm.change();
            }
        }



        go.transform.tag = "Untagged";

    }

    public static void Activate(GameObject go)
    {
        Debug.Log("Activate method");

        MonoBehaviour[] scripts = go.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;

            if (script is ChangeMaterial)
            {
                ChangeMaterial cm = (ChangeMaterial)(script);
                cm.change();
            }

            if(script is SwapMaterials)
            {
                ((SwapMaterials)script).activate();
            }

        }



        go.transform.tag = "ActiveObject";




    }

    public void ActivateItself()
    {
        Activate(gameObject);

    }

    public void DisableItself()
    {
        Disable(gameObject);
    }


    public void finishedSwitching()
    {
        DisableItself();
    }


    public void Action()
    {
        Activate(go);
    }

    //public void Action()
    //{
    //    print("activated");

    //    MonoBehaviour[] scripts = go.GetComponents<MonoBehaviour>();

    //    foreach(MonoBehaviour script in scripts)
    //    {
    //        script.enabled = true;

    //        if(script is ChangeMaterial)
    //        {
    //            ChangeMaterial cm = (ChangeMaterial)(script);
    //            cm.change();
    //        }

    //    }



    //    go.transform.tag = "ActiveObject";


    //}
}
