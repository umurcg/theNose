using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//_MumbleWhileSpeaking
//Dependent To: CharacterSubtitle
//This script chooses random audio clip from array and play it.
//It select audio randomly everytime when talking character changes.
//It uses same logic whit _WhoIsTalking script
//Therefore it should be attached to CharacterSubtitle object

public class MumbleWhileSpeaking : MonoBehaviour {

	public AudioClip[] mumbles;
	char firstLetter;
	Text text;
	AudioSource audioS;

	// Use this for initialization
	void Awake () {
		text = GetComponent<Text> ();

		audioS = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (text.text == "") {
			audioS.Stop ();
		} else {
			
			if (firstLetter != text.text [0]||firstLetter == null) {
				changeAudio ();
				firstLetter = text.text [0];
			}
		}



	}

	void changeAudio(){

		int index=Random.Range (0, mumbles.Length);
		audioS.clip = mumbles [index];
		audioS.Play ();
	}

}
