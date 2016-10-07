using UnityEngine;
using System.Collections;
namespace CinemaDirector
{
    public class PausePlayCS : MonoBehaviour
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

        public void Play()
        {
       
        if(cs!=null)
         cs.Play();

        }


    }
}