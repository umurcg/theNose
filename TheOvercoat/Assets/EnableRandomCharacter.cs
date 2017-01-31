using UnityEngine;
using System.Collections;

//Enables random character at the start of game
//This script should be attached to camera while it forces to follow player.
public class EnableRandomCharacter : MonoBehaviour {

    GameObject activePlayer;
    Vector3 positionOffset;
    
	// Use this for initialization
	void Start () {
        GameObject playerOwner = CharGameController.getOwner();
        int randomIndex = Random.Range(1, playerOwner.transform.childCount);
        CharGameController.setCharacter(randomIndex);
        //Debug.Log("index is " + randomIndex);
        activePlayer = CharGameController.getActiveCharacter();
        positionOffset = activePlayer.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = activePlayer.transform.position - positionOffset;
	}
}
