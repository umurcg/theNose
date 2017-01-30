using UnityEngine;
using System.Collections;

//EDITOR SCRIPT
//This script gets all child subtitles and put them into one textgenerator object in cutscenes.

namespace CinemaDirector
{
    public class GetAllChildSubtitle : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {


        }
        public void get()
        {
            GameObject go = new GameObject("total");
            go.transform.SetParent(gameObject.transform.parent);
            go.AddComponent<TextGenerationEvent>();
            TextGenerationEvent tgeGO = go.GetComponent<TextGenerationEvent>();

            for (int i = 0; i < transform.childCount; i++)
            {

                TextGenerationEvent tge = transform.GetChild(i).GetComponent<TextGenerationEvent>();
                if(tge!=null)
                tgeGO.textValue = tgeGO.textValue + tge.textValue + "\n";


            }

        }
    }
}