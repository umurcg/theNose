using UnityEngine;
using System.Collections;

public class SpaceTrigger : MonoBehaviour {

    
    public string axis= "InteractionButton";

    //GameObject that is inside collider
    IClickAction focusScr;
    GameObject focusObj;

    IconController ic;

	// Use this for initialization
	void Start () {
        ic = IconController.ico;
	}
	
	// Update is called once per frame
	void Update () {



		if (Input.GetButtonDown(axis) && focusScr!= null && focusObj!=null) {
            focusScr.Action();
            clearFocus();
            
		}


        //if (Input.GetButtonDown(axis))
        //{
        //    Debug.Log("You pressed button");
        //}


    }

	void OnTriggerEnter(Collider col){
        if (col.tag == "ActiveObject")
        {
            focusScr = col.gameObject.GetComponent<IClickAction>();
            focusObj = col.gameObject;
            ic.gameObject.SetActive(true);
        }
	}

	void OnTriggerExit(Collider col){

        if (col.gameObject == focusObj)
        {
            clearFocus();
        }
	}

    public void clearFocus()
    {
        focusObj = null;
        focusScr = null;
        ic.gameObject.SetActive(false);
    }




}
