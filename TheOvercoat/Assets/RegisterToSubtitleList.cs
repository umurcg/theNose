using UnityEngine;
using System.Collections;


//This scripts registers object to subtitle list in whoistalking script
public class RegisterToSubtitleList : MonoBehaviour {

	// Use this for initialization
	void Start () {
        WhoIsTalking.self.addCharacterToDict(gameObject, gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
