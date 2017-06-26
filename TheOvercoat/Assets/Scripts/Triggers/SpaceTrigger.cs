using UnityEngine;
using System.Collections;

public class SpaceTrigger : MonoBehaviour {

    public KeyCode[] keys =new KeyCode[]{KeyCode.Space,KeyCode.Joystick1Button0 };


    //GameObject that is inside collider
    IClickAction focusScr;
    GameObject focusObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bool b = false;

        foreach(KeyCode k in keys)
        {
            b = b || Input.GetKeyDown(k);
        }

        if (b)
        {
            Debug.Log("Presseeeeed");
        }

		if (/*Input.GetAxis("InteractionKeyboard")==1*/ b && focusScr!= null && focusObj!=null) {
            focusScr.Action();
		}
        


	}

	void OnTriggerEnter(Collider col){
        if (col.tag == "ActiveObject")
        {
            focusScr = col.gameObject.GetComponent<IClickAction>();
            focusObj = col.gameObject;
        }
	}

	void OnTriggerExit(Collider col){

        if (col.gameObject == focusObj)
        {
            focusObj = null;
            focusScr = null;
        }
	}



}
