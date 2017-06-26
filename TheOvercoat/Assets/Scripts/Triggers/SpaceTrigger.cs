using UnityEngine;
using System.Collections;

public class SpaceTrigger : MonoBehaviour {

    public KeyCode[] keys =new KeyCode[]{KeyCode.Space,KeyCode.Joystick1Button1 };


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
        bool b = false;

        //if(ic==null) ic = IconController.ico;
        //Debug.Log((ic == null));

        //if (focusObj != null && !ic.gameObject.activeSelf )
        //{
        //    ic.gameObject.SetActive(true);
        //}else if (ic.gameObject.activeSelf)
        //{
        //    ic.gameObject.SetActive(false);
        //}


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
            ic.gameObject.SetActive(true);
        }
	}

	void OnTriggerExit(Collider col){

        if (col.gameObject == focusObj)
        {
            focusObj = null;
            focusScr = null;
            ic.gameObject.SetActive(false);
        }
	}



}
