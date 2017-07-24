using UnityEngine;
using System.Collections;

//Todo write a custom editor 
public class EnterTrigger : MonoBehaviour {

    public enum sendMessage { inExit,inEnter};
    public sendMessage sm = sendMessage.inEnter;
    public bool deleteAfterSendMessage = false;
    public bool onlyPlayerCanTrigger = true;

    [HideInInspector]
    public bool enter=false;

    [HideInInspector]
    public GameObject lastEnteredObject = null;


    //Use this variables if you want to send message when trigger is activated.
    public GameObject messageReciever;
    public string message;
    public bool isRecieverActivePlayer=false;

    bool playerInside = false;

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
        if (isRecieverActivePlayer)
        {
            GameObject player = CharGameController.getActiveCharacter();
            if (player != null)
            {
                player.SendMessage(message);
                return;
            }
        }

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

        lastEnteredObject = col.gameObject;

        if (col.tag == "Player"||!onlyPlayerCanTrigger){

            playerInside = true;

            //Debug.Log("Player entered");
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
            playerInside = false;

            enter = false;
            IEnterTrigger iet = GetComponent<IEnterTrigger>();
            if (iet != null)   iet.exitTriggerAction(col);

            if (sm == sendMessage.inExit) send(); 

        }

    }

    public bool isPlayerInside()
    {
        return playerInside;
    }

}
