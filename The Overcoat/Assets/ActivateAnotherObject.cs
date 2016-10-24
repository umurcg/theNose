using UnityEngine;
using System.Collections;

//This script activates another object.
//It basically enables all the scripts of an object and changes tag to ActiveObbject.
//It triggered with click or space trigger scripts.


public class ActivateAnotherObject : MonoBehaviour, IClickAction {

    public GameObject go;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public static void Disable(GameObject go)
    {
        MonoBehaviour[] scripts = go.GetComponents<MonoBehaviour>();

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
        MonoBehaviour[] scripts = go.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;

            if (script is ChangeMaterial)
            {
                ChangeMaterial cm = (ChangeMaterial)(script);
                cm.change();
            }
        }



        go.transform.tag = "ActiveObject";




    }

    public void Action()
    {
        print("activated");

        MonoBehaviour[] scripts = go.GetComponents<MonoBehaviour>();

        foreach(MonoBehaviour script in scripts)
        {
            script.enabled = true;

            if(script is ChangeMaterial)
            {
                ChangeMaterial cm = (ChangeMaterial)(script);
                cm.change();
            }

        }



        go.transform.tag = "ActiveObject";


    }
}
