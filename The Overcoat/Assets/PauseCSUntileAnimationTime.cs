using UnityEngine;
using System.Collections;

namespace CinemaDirector{
public class PauseCSUntileAnimationTime : MonoBehaviour {
		Animator ac;
	public float time;

		public Cutscene cs;
	
	// Use this for initialization
	void Start () {
			ac = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public IEnumerator PauseForAnimation(){

		cs.Pause ();

			AnimatorClipInfo[] clip = ac.GetCurrentAnimatorClipInfo (0);
		
			float animTime = 0;
		while (animTime<time) {
				AnimatorStateInfo asi= ac.GetCurrentAnimatorStateInfo(0);
				animTime = clip [0].clip.length * asi.normalizedTime;
		
			yield return null;
		}
	
		cs.Play ();
	}

}
}