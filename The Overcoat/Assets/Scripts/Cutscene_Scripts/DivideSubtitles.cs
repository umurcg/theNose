using UnityEngine;
using System.Collections;


//This scripts Divide subtitles and create text generator items under owner object.

namespace CinemaDirector
{
    public class DivideSubtitles : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Divide()
        {
            TextGenerationEvent tge = GetComponent<TextGenerationEvent>();
            char c = "/"[0];
            string[] splitString = tge.textValue.Split('\n');


            float k = transform.parent.childCount-1;
            tge.textValue = splitString[0];
            for  (int i=1;i<splitString.Length;i++)
            {
                
                GameObject g = new GameObject("Text Generator "+(i+k));
                g.transform.parent = transform.parent;
                TextGenerationEvent t= g.AddComponent<TextGenerationEvent>();
                t.textValue = splitString[i];
            }
        }
    }
}