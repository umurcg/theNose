using UnityEngine;
using System.Collections;


//_CutsceneTriggerViaAction
//_Dependent to: _IClickAction.cs

//This script activates Cutscene cs when Action function called.

namespace CinemaDirector{
   public class CutsceneTriggerViaAction : MonoBehaviour, IClickAction {
		public Cutscene cs;
        public bool useAgain = false;
        public bool enabledAction=true;
        bool disabled = false;

        


      
		public void Action(){
            if (enabledAction)
            {
                if (cs != null && disabled == false)
                {
                    print("ACTİON");
                    cs.Play();
                    //  transform.parent.gameObject.SetActive(false);
                    if (useAgain)
                        disabled = true;


                    finishedAction();
                }
              
            }

		}

        public void enableCS()
        {
            disabled = false;

        }

        public void finishedAction()
        {

        }


	
	}
}