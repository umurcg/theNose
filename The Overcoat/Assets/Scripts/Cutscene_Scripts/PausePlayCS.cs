using UnityEngine;
using System.Collections;
namespace CinemaDirector
{
    public class PausePlayCS : MonoBehaviour, ISubtitleFinishFunction
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

        //This method is for subtitles. When a subtitle controller finishes his job, it calls this function. 
        //This function enables cutscene to continue when subtitle is finished.
        public void finishFunction()
        {
            
            Play();

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