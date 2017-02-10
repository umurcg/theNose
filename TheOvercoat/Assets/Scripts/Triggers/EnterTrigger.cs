using UnityEngine;
using System.Collections;

public class EnterTrigger : MonoBehaviour {

    public enum sendMessage { inExit,inEnter};
    public sendMessage sm = sendMessage.inEnter;
    public bool deleteAfterSendMessage = false;
    public bool onlyPlayerCanTrigger = true;

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

    void send()
    {
        //Sending message
        if (messageReciever != null && message != "")
        {
            messageReciever.SendMessage(message);
            if (deleteAfterSendMessage) Destroy(this);
        }
    }

    void OnTriggerEnter(Collider col)
    {


        //IClickAction ic=  GetComponent<IClickAction>();
        //if(ic!=null)
        //ic.Action();
        

        if (col.tag == "Player"||!onlyPlayerCanTrigger){
            //print("hello");
            enter = true;
            IEnterTrigger iet = GetComponent<IEnterTrigger>();
            if (iet != null)
                iet.TriggerAction(col);

            if (sm == sendMessage.inEnter)   send();
            
        }
        
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player"||!onlyPlayerCanTrigger)
        {
            enter = false;
            IEnterTrigger iet = GetComponent<IEnterTrigger>();
            if (iet != null)   iet.exitTriggerAction(col);

            if (sm == sendMessage.inExit) send(); 

        }

    }

}
