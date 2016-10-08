using UnityEngine;
using System.Collections;

public class SubtitleCaller : MonoBehaviour,ISubtitleTrigger {

    //int currentIndex = 0;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {



	}

    public void callSubtitleWithIndex(int index)
    {
        
        SubtitleController[] scs=GetComponents<SubtitleController>();
        if (index <= scs.Length)
        {
            scs[index].startSubtitle();
            //currentIndex = index;
        }
        }


    public void callSubtitle()
    {

        // SubtitleController[] scs = GetComponents<SubtitleController>();
        //print(scs.Length);
        //if (currentIndex < scs.Length)
        //{
        //    scs[currentIndex].startSubtitle();
        //    currentIndex++;
        //}
        SubtitleController scs = GetComponent<SubtitleController>();
        scs.startSubtitle();

    }


}
