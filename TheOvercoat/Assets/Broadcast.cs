using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A parent class for broadcasting a message
public class Broadcast : MonoBehaviour {

    public GameObject[] recievers;
    public string[] messages;
    public bool destroyAfterSend = false;

    public void addReciever(GameObject obj)
    {
       
       recievers= Vckrs.addToArray<GameObject>(recievers, obj);
    }

    public void addMessage(string message)
    {
        messages = Vckrs.addToArray<string>(messages, message);
    }

    public void sendMessage()
    {
        foreach(GameObject rec in recievers)
        {
            foreach (string mes in messages) {

                rec.SendMessage(mes);

             }
        }

        if (destroyAfterSend) Destroy(this);
    }

}
