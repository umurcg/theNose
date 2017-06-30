using UnityEngine;
using System.Collections;


//This script broadcast a message when user clicks on it with mouse
public class BroadcastOnClick : MonoBehaviour {

    public GameObject reciever;
    public string message;

        

    private void OnMouseDrag()
    {
        //Debug.Log("Broaadcaaast");
        reciever.SendMessage(message);
    }
}
