using UnityEngine;
using System.Collections;



//This scripts pause and play cutscene.
//It can be called from lots of interface. So becareful!!!!

namespace CinemaDirector
{
    public class PausePlayCS : MonoBehaviour, ISubtitleFinishFunction, ITryingTomove, INearObjectAciton, IFinishedSwitching
    {
        public Cutscene cs;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Stop()
        {
            if (cs != null) 
           
            cs.Pause();
        
        }

        //This script called when switchObject funstion finishes its job
        public void finishedSwitching()
        {
            cs.Play();
       
        }

        //This method is for subtitles. When a subtitle controller finishes his job, it calls this function. 
        //This function enables cutscene to continue when subtitle is finished.
        public void finishFunction()
        {
          //  print("finish");
            Play();

        }

       public void noAction()
        {
            Play();
        }

        public void trying()
        {
            Play();
        }

        public void stopAndCallSubtitle()
        {
            cs.Pause();
            SubtitleCaller sc = GetComponent<SubtitleCaller>();
            if (sc)
                sc.callSubtitle();
        }

        

        public void Play()
        {

            if (cs != null)
            {
                cs.Play();
            }else
            {
                print("PUT THE CUTSCENE FAGGOT!");
            }

        } 


    }
}