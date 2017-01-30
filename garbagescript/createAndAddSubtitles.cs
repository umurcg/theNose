using UnityEngine;
using System.Collections;
using CinemaDirector.Helpers;

namespace CinemaDirector{
public class createAndAddSubtitles : MonoBehaviour {
    
   
	public string subtitles;
		char c="\\"[0];
	public void createAndAdd(){
			

		string[] subtArray = subtitles.Split (c);
		foreach (string s in subtArray) {
//				GameObject ob = new GameObject ();
//
//
//				ob.AddComponent (TextGenerationEvent);

		}
	}

}
}