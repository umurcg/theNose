using UnityEngine;
using System.Collections;


//This script broadcast a message when user clicks on it with mouse
public class BroadcastOnClick : MonoBehaviour {

    public GameObject reciever;
    public string message;
    public bool destroyAfterBC = false;
    public bool sendMessageWithObject = false;
        

    private void OnMouseDrag()
    {
        if (sendMessageWithObject)
        {
            reciever.SendMessage(message, gameObject);
        }
        else
        {
            //Debug.Log("Broaadcaaast");
            reciever.SendMessage(message);
         
        }

        if (destroyAfterBC)
        {
            Debug.Log("Destroiiiiiiiing");
            Destroy(this);

        }
    }
}
