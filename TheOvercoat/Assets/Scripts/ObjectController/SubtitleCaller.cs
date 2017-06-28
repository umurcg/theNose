using UnityEngine;
using System.Collections;


//This object starts first subtitleController script.
//It is used for using more than subtitleController scripts on same object.

public class SubtitleCaller : MonoBehaviour,ISubtitleTrigger {

    //Start automatic when player clicks the object or start manually from script
    public bool startAutomatic = false;

    SubtitleController activeController;

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
        if (index < scs.Length)
        {
            scs[index].startSubtitle();
            activeController = scs[index];
            //currentIndex = index;
        }
     }


    public void callSubtitle()
    {

        Debug.Log("Call subtitle");
        SubtitleController scs = GetComponent<SubtitleController>();
        if (scs)
        {
            scs.startSubtitle();
            activeController = scs;
        }
    }

    public void callSubtitleWithIndexTime(int index)
    {
     
        SubtitleControllerTime[] scs = GetComponents<SubtitleControllerTime>();
    
        if (index < scs.Length)
        {
            scs[index].startSubtitle();
            activeController = scs[index];
            //print(scs.Length);
            //currentIndex = index;
        }
    }


    public void callSubtitleTime()
    {


        SubtitleControllerTime scs = GetComponent<SubtitleControllerTime>();
        if (scs)
        {
            scs.startSubtitle();
            activeController = scs;
        }
    }


    public bool isAutomatic()
    {
        return startAutomatic;
    }
    
    public SubtitleController getActiveController()
    {
        return activeController;
    }

    public void termianteCurrentController()
    {
        activeController.terminateSubtitle();
        activeController = null;
    }

}
