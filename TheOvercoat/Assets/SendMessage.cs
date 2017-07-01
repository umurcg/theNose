using UnityEngine;
using System.Collections;

public enum triggerType { OnDestroy, OnEnable, OnTriggerEnter, OnTriggerExit };

//Send message script for different purposes.
public class SendMessage : MonoBehaviour {

    public GameObject[] recievers;
    public string message;

    public bool sendWithObject = false;
    public triggerType type;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnDestroy()
    {
        if (type == triggerType.OnDestroy)
        {
            sendMessage();
        }
    }

    private void OnEnable()
    {
        if (type == triggerType.OnEnable)
        {
            sendMessage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (type == triggerType.OnTriggerEnter)
        {
            sendMessage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (type == triggerType.OnTriggerExit)
        {
            sendMessage();
        }
    }

    void sendMessage()
    {
        foreach(GameObject OBJ in recievers)
        {
            if (sendWithObject)
            {
                OBJ.SendMessage(message, gameObject);
            }
            else
            {
                OBJ.SendMessage(message);
            }
        }
    }
}
