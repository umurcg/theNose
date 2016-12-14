using UnityEngine;
using System.Collections;

public class EnterTrigger : MonoBehaviour {
    public bool enter=false;

    //Use this variables if you want to send message when trigger is activated.
    public GameObject messageReciever;
    public string message;

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
            //print("hello");
            enter = true;
            IEnterTrigger iet = GetComponent<IEnterTrigger>();
            if (iet != null)
                iet.TriggerAction(col);

            //Sending message
            if (messageReciever != null && message != "")
            {
                messageReciever.SendMessage(message);

            }

        }
        
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            enter = false;
            IEnterTrigger iet = GetComponent<IEnterTrigger>();
            if (iet != null)
                iet.exitTriggerAction(col);
        }

    }

}
