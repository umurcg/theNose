using UnityEngine;
using System.Collections;

public class SendMessageGroundLevelTrigger : MonoBehaviour {

    //public enum triggerType {Above, Below  };
    //public triggerType TriggerWhenPlayer = triggerType.Above;

    public float groundLevel;
    public bool oneTimeUse = true;
    public GameObject reciever;
    public string belowMessage, aboveMessage;
    GameObject player;

    bool above = false;
    
	// Use this for initialization
	void Start () {
        player = CharGameController.getActiveCharacter();

        above = player.transform.position.y > groundLevel;


    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("Player gorund level is " + player.transform.position.y);

        //bool call = (TriggerWhenPlayer == triggerType.Above && player.transform.position.y > groundLevel) ||
        //    (TriggerWhenPlayer == triggerType.Below && player.transform.position.y < groundLevel);

        if (above && player.transform.position.y < groundLevel)
        {
            above = false;
            callCoroutine();
        }
        else if (!above && player.transform.position.y > groundLevel)
        {
            above = true;
            callCoroutine();
        }


        //if (call)
        //{
        //    callCoroutine();
        //}
	}

    void callCoroutine()
    {
        Debug.Log(player.transform.position.y);

        string message = above ? aboveMessage : belowMessage;
        reciever.SendMessage(message);
        if (oneTimeUse) Destroy(this);

    }
}
