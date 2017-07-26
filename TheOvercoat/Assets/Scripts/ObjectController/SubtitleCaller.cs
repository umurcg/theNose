using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//This object starts first subtitleController script.
//It is used for using more than subtitleController scripts on same object.

public class SubtitleCaller : MonoBehaviour,ISubtitleTrigger {

    //Start automatic when player clicks the object or start manually from script
    public bool startAutomatic = false;

    SubtitleController activeController;

    [HideInInspector]
    public Text characterSubt;
    [HideInInspector]
    public Text narratorSubt;

    //int currentIndex = 0;


    // Use this for initialization
    void Start () {

        characterSubt = SubtitleFade.subtitles["CharacterSubtitle"];
        narratorSubt = SubtitleFade.subtitles["NarratorSubtitle"];

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
            scs[index].caller = this;
            //currentIndex = index;
        }
     }


    public void callSubtitle()
    {

        //Debug.Log("Call subtitle");
        SubtitleController scs = GetComponent<SubtitleController>();
        if (scs)
        {
            scs.startSubtitle();
            activeController = scs;
            scs.caller = this;
        }
    }

    public void callSubtitleWithIndexTime(int index)
    {
     
        SubtitleControllerTime[] scs = GetComponents<SubtitleControllerTime>();
    
        if (index < scs.Length)
        {
            scs[index].startSubtitle();
            activeController = scs[index];
            scs[index].caller = this;
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
            scs.caller = this;
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

    //return index of active subtitle controller. 
    //Index is order of subtitle controller in owner object.
    public int getActiveControllerIndex()
    {
        SubtitleController[] allSubtitlesAtOwner = GetComponents<SubtitleController>();
        int index = 0;
        foreach (SubtitleController c in allSubtitlesAtOwner)
        {
            if (activeController == c) return index;
            index++;
        }

        return -1;
    }

    public void termianteCurrentController()
    {
        if (activeController == null) return;
        activeController.terminateSubtitle();
        activeController = null;
    }

    public void callRandomSubtTime(int index)
    {
        SubtitleControllerTime[] scs = GetComponents<SubtitleControllerTime>();

        if (index < scs.Length)
        {
            scs[index].randomSubtitle();
            scs[index].caller = this;

        }
    }

    public void eraseActiveController()
    {
        activeController = null;
    }

    public int getCurrentSubtIndex()
    {
        if (activeController) return activeController.getCurrentIndex();

        return -1;
    }

    public int countSubtitles()
    {
        return countCharSubtitles() + countNarSubtitles();
    }

    public int countCharSubtitles()
    {
        SubtitleController[] scs = GetComponents<SubtitleController>();

        return scs.Length - countNarSubtitles();

    }

    public int countNarSubtitles()
    {
        SubtitleControllerTime[] scts = GetComponents<SubtitleControllerTime>();
        return scts.Length;
    }

}
