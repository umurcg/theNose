using UnityEngine;
using System.Collections;

namespace CinemaDirector{
   public class CutsceneTriggerViaAction : MonoBehaviour, IClickAction {
		public Cutscene cs;
  

	// Use this for initialization
	    void Start () {
	
	   }
	
	// Update is called once per frame
	    void Update () {
	
	    }
      
		public void Action(){
			if (cs != null) {
				cs.Play ();
                transform.parent.gameObject.SetActive(false);
                Destroy(this);
			}
		}
	
	}
}