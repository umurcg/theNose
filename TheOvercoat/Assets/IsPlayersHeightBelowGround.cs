using UnityEngine;
using System.Collections;

public class IsPlayersHeightBelowGround : MonoBehaviour {

    public float groundLevel;
    public bool oneTimeUse = true;
    public GameObject reciever;
    public string message;
    GameObject player;
	// Use this for initialization
	void Start () {
        player = CharGameController.getActiveCharacter();
	}
	
	// Update is called once per frame
	void Update () {
        if (player.transform.position.y < groundLevel)
        {
            callCoroutine();
        }
	}

    void callCoroutine()
    {
        Debug.Log(player.transform.position.y);
        reciever.SendMessage(message);
        if (oneTimeUse) Destroy(this);

    }
}
