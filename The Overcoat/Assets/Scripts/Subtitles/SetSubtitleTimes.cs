using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CinemaDirector{



public class SetSubtitleTimes : MonoBehaviour {
		
		List<float> times;
		public float timeFactor=3;
		public float Duration=0;
		public float minTime=2;

	void Start () {
	 
	



	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setTimes(){
			if (transform.childCount > 0) {
				times = new List<float> ();
			

				for (int i = 0; i < transform.childCount; i++) {

					TextGenerationEvent tge = transform.GetChild (i).GetComponent<TextGenerationEvent> ();
					times.Add (tge.Firetime);
					if (i > 0) {
						TextGenerationEvent tgePrevious = transform.GetChild (i - 1).GetComponent<TextGenerationEvent> ();

						if (tge.lockTime == false) {
							int index =	tgePrevious.textValue.IndexOf (":");
							print (index);
							float time =  TextToTimes(tgePrevious.textValue.Substring (index, tgePrevious.textValue.Length - index - 1));
							if (time > minTime) {

								tge.Firetime = tgePrevious.Firetime + time;
							} else {
								tge.Firetime = tgePrevious.Firetime + minTime;
							}

							}
					}
				}

			}

	}

		public void setDuration (){
			for (int i = 0; i < transform.childCount; i++) {

				TextGenerationEvent tge = transform.GetChild (i).GetComponent<TextGenerationEvent> ();
				tge.Duration = Duration;
			}
		}

		public void restore(){
			if (times!=null) {
				for (int i = 0; i < transform.childCount ; i++) {

					transform.GetChild (i).GetComponent<TextGenerationEvent> ().Firetime = times[i];
				}
			}
		}


		float TextToTimes(string text){
//			if (text.Length >= 8) {
//				//print (text.Substring (0, 4));
//				if (text.Substring (text.Length-8, 4) == "Wait") {
//					float time = float.Parse (text.Substring (text.Length-2, 2));
//
//					//print (time);
//					return time;
//				}
//			}

			float length = text.Length;
			//print (length);
			return length/timeFactor;


	}

}
}