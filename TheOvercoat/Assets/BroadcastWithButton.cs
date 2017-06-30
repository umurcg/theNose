using UnityEngine;
using System.Collections;

public class BroadcastWithButton : MonoBehaviour {

    public string[] buttonAxes;
    public GameObject reciever;
    public string message;

    public bool destroyItself = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        foreach(string b in buttonAxes)
        {
            if (Input.GetButtonDown(b))
            {
                reciever.SendMessage(message);
                if (destroyItself) Destroy(this);
            }
        }

	}
}
