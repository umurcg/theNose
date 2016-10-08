using UnityEngine;
using System.Collections;

namespace CinemaDirector{
public class PauseCSWhileWalking : MonoBehaviour {

		public Cutscene cs;
		public float tolerance=0.001f;
	// Use this for initialization
	void Start () {

      
	}
	
	// Update is called once per frame
	void Update () {
	
	}

		public IEnumerator PauseForWalking(){
	
			cs.Pause ();
			Vector3 prevLocation=new Vector3(0,0,0);
			while (Vector3.Distance (transform.position, prevLocation) > tolerance) {
				
				prevLocation = transform.position;
				yield return null;
			}

      
			cs.Play ();
	}
}
}