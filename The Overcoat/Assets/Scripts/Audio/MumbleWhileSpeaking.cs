using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
