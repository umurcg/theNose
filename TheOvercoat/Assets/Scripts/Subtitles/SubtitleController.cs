﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SubtitleController : MonoBehaviour {
    public string[] subtitleTexts;
    public string AllSubtitles;

    public GameObject subtitle;
    protected Text text;
    

	PlayerComponentController pcc;

    public bool ifDesroyItself = false;
    public bool releaseAfterSub = false;
    protected int index;


    // Use this for initialization
    public virtual void Start () {


        //if (SubtitleFade.subtitles == null)
        //{
        //    print("NULLLLLLLLLL");
        //}
        //else
        //{
        //    print("Length of dcit: " + SubtitleFade.subtitles.Count);
        //}

        if (subtitle == null)
        {

            text = SubtitleFade.subtitles["CharacterSubtitle"];
        }
        else
        {

            text = subtitle.GetComponent<Text>();
        }
        if (text == null)
        {
            //print("PUT THE SUBTITLE YOU FAGGOT");
         
        }
        index = -1;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player!=null)
        pcc = player.GetComponent<PlayerComponentController>();

        //idunow
        this.enabled = false;

    }

    public void AllToArray()
    {
        string[] subts = AllSubtitles.Split('\n');

        string[] z = new string[subtitleTexts.Length+subts.Length];
        subtitleTexts.CopyTo(z, 0);
        subts.CopyTo(z, subtitleTexts.Length);
        subtitleTexts = z;
        


    }

    public void ArrayToAll()
    {

        foreach (string s in subtitleTexts)
        {
            AllSubtitles = AllSubtitles + s + "\n";

        }

    }

    // Update is called once per frame
    protected virtual void Update () {
      //  print("index: "+index+"array: "+subtitleTexts.Length);

        if (index != -1)
        {
            
            if (index < subtitleTexts.Length)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Debug.Log("You pressed to mouse");
                    index++;
                                    
                    if (index < subtitleTexts.Length)
                        text.text = subtitleTexts[index];
                }
            }
            
            else
            {
                ClickTrigger.disabled = false;

                text.text = "";

                if(pcc!=null&&releaseAfterSub)
                pcc.ContinueToWalk ();



                ISubtitleFinishFunction sff = GetComponent<ISubtitleFinishFunction>();
                if (sff != null)
                {
                    //print("finishFunct");
                    sff.finishFunction();
                }
                    

                if (ifDesroyItself)
                {
                    Destroy(gameObject.GetComponent<SubtitleController>());
     
                }
                this.enabled = false;
            }
        }

	}


   public virtual void startSubtitle()
    {

        if (text == null) Debug.Log("No text");
        text.text = subtitleTexts[0];
        index = 0;
        if (pcc != null)
            pcc.StopToWalk ();

        ClickTrigger.disabled = true;
        this.enabled = true;
    }


    public void setCharSubtitle()
    {
        subtitle = GameObject.FindGameObjectWithTag("CharacterSubtitle");

    }

    public void setNarSubtitle()
    {
        subtitle = GameObject.FindGameObjectWithTag("NarratorSubtitle");

    }

}
