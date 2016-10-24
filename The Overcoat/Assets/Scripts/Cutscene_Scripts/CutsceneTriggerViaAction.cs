using UnityEngine;
using System.Collections;


//_CutsceneTriggerViaAction
//_Dependent to: _IClickAction.cs

//This script activates Cutscene cs when Action function called.

namespace CinemaDirector{
   public class CutsceneTriggerViaAction : MonoBehaviour, IClickAction {
		public Cutscene cs;
        public bool useAgain = false;
        bool disabled = false;
        

	// Use this for initialization
	    void Start () {
	
	   }
	
	// Update is called once per frame
	    void Update () {
	
	    }
      
		public void Action(){
			if (cs != null && disabled==false) {
                print("ACTİON");
				cs.Play ();
              //  transform.parent.gameObject.SetActive(false);
                if(useAgain)
                    disabled=true;
			}
		}

        public void enableCS()
        {
            disabled = false;

        }
	
	}
}