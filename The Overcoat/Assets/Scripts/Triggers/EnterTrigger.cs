using UnityEngine;
using System.Collections;

public class EnterTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnTriggerStay(Collider col)
    {
     
    }

    void OnTriggerEnter(Collider col)
    {


        //IClickAction ic=  GetComponent<IClickAction>();
        //if(ic!=null)
        //ic.Action();

        if (col.tag == "Player"){
            IEnterTrigger iet = GetComponent<IEnterTrigger>();
            if (iet != null)
                iet.TriggerAction(col);
        }
        
    }

    void OnTriggerExit(Collider col)
    {

        IEnterTrigger iet = GetComponent<IEnterTrigger>();
        if (iet != null)
        iet.exitTriggerAction(col);


    }

}
