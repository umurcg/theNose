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
       // print("staaay");
    }

    void OnTriggerEnter(Collider col)
    {
        print("triggerEnter");

        IClickAction ic=  GetComponent<IClickAction>();
        if (ic != null)
        {
            ic.Action();
        }
        else
        {
            
            IEnterTrigger iet = GetComponent<IEnterTrigger>();
            iet.TriggerAction(col);

        }
    }

}
