using UnityEngine;
using System.Collections;


//This script broadcast a message when user clicks on it with mouse
public class BroadcastOnClick : MonoBehaviour {

    public GameObject reciever;
    public string message;
    public bool destroyAfterBC = false;

        

    private void OnMouseDrag()
    {
        //Debug.Log("Broaadcaaast");
        reciever.SendMessage(message);
        if (destroyAfterBC) Destroy(this);

    }
}
