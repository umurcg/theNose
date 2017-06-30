using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using MovementEffects;

public class SubtitleControllerTime : SubtitleController {
    public float[] manualTimerArray;
    public enum timeMode {automatic,manual };
    public timeMode TimeMode;
    public bool narrator = true;

    
    public float timeInhibator=13;
    float[] automaticTimerArray;

    float timer = 0;
    
    AudioSource narratorAudioSource;
    GlobalController.Language language;


    //This is dictionary to clip list for each language
    [System.Serializable]
    public struct AudioClips
    {
        public GlobalController.Language language;
        public AudioClip[] clips;
    }


    //Clips must have same length with subtitles.
    public AudioClips[] clips;

    //Get clip list according to language
    AudioClips findAudioClipsWithLanguage(GlobalController.Language lan)
    {
        if (clips == null || clips.Length == 0) return new AudioClips();
        foreach (AudioClips c in clips) if (c.language == lan) return c;
        return new AudioClips();
    }


    // Use this for initialization
    public override void Start () {
         
        if (textAsset != null) importFromTextFile();

        if (narrator)
        {
            text = SubtitleFade.subtitles["NarratorSubtitle"];
        }
        else
        {
            text= SubtitleFade.subtitles["CharacterSubtitle"];
            
        }
    

        //If couldnt find subtitle with SubtitleFade get it with tag
        if (text == null)
        {
            text = getNarSubt();
            Debug.Log("Getting narrator subtitle with tag");
        }

        if (text == null)
        {
           
            print("PUT THE SUBTITLE YOU FAGGOT");
        }
        index = -1;


        //Get language 
        language= GlobalController.Instance.languageSetting;

        narratorAudioSource = CharGameController.getOwner().GetComponents<AudioSource>()[2];


        //If number of clips doesnt match subtitle length destroy clips
        int lengthOfSubtitles = subtitleTexts.Length;
        AudioClips currentLanguageClips = findAudioClipsWithLanguage(language);
        if (currentLanguageClips.clips == null || currentLanguageClips.clips.Length == 0 || currentLanguageClips.clips.Length != lengthOfSubtitles)
        {
                //Debug.Log("Audio clip length and subtitle length are not mathc each other. So canceling audio clips");
                clips = null;
        }
        
       
        this.enabled = false;
        

        
    }

    // Update is called once per frame
    protected override void Update () {
        //print(timer);

        if (timer > 0)
            timer -= Time.deltaTime;

        if (index != -1&&timer<=0)
        {         

            if (index < subtitleTexts.Length -1)
            {
                
                index++;
                text.text = subtitleTexts[index];
                setNarratorAudio(index);
                assignTimer();

                

            }else if(timer<=0)
            {
                timer = 0;
                text.text = "";

                clearNarratorAudio();

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

    void assignTimer()
    {
        if (TimeMode == timeMode.manual)
        {
            timer = manualTimerArray[index];
        }
        else
        {
            timer = automaticTimerArray[index];
        }
    }

    public new void startSubtitle()
    {
        //Debug.Log("starting subtitles, obj name " + name);
        text.fontStyle = FontStyle.Italic;

        this.enabled = true;
        if (TimeMode == timeMode.automatic)
        {
            calculateTimes();
        }else if(manualTimerArray.Length  != subtitleTexts.Length)
        {
            TimeMode = timeMode.automatic;
            calculateTimes();
            print("You set time array wrong. So I calculated times by myself. :/");
        }

        index = 0;
        text.text = subtitleTexts[index];
        setNarratorAudio(index);
        assignTimer();

    }

    public override void terminateSubtitle()
    {
        base.terminateSubtitle();
        timer = 0;

    }

    //fills time array
    void calculateTimes()
    {
        //Debug.Log("Calculating times, obj name " + name);
        automaticTimerArray = new float[subtitleTexts.Length ];
        for (int i = 0; i < subtitleTexts.Length; i++)
        {          
               automaticTimerArray[i]= calculate(subtitleTexts[i]);
          
        }

        //foreach (float t in automaticTimerArray)
        //{
        //    //print(t);
        //}
    }

    //calculates time from string
    float calculate(string s)
    {
        string trimmedString;
        if (s.Contains(":")){
            int index = s.IndexOf(":");
            trimmedString = s.Substring(index, s.Length - index - 1);
        }else{
            trimmedString = s;
        }
        //print(trimmedString);
        float length = trimmedString.Length;
        return length / timeInhibator;

       
    }

    void setNarratorAudio(int index)
    {
        if (clips==null || clips.Length!=0 ) return;
        Debug.Log(clips.Length);
        if (!narratorAudioSource) Debug.Log("No audio source");
        narratorAudioSource.clip = findAudioClipsWithLanguage(language).clips[index];
        narratorAudioSource.Play();
    }

    void clearNarratorAudio()
    {
        narratorAudioSource.clip = null;
    }
    public void randomSubtitle()
    {
        Timing.RunCoroutine(_randomSubtitle());
    }

    IEnumerator<float> _randomSubtitle()
    {
        Debug.Log("rANDOM SUBT");
        text.fontStyle = FontStyle.Italic;
        int subtIndex = Random.Range(0, subtitleTexts.Length);
        string sub = subtitleTexts[subtIndex];
        text.text = sub;

        if (TimeMode == timeMode.automatic)
        {
            calculateTimes();
        }
        else if (manualTimerArray.Length != subtitleTexts.Length)
        {
            TimeMode = timeMode.automatic;
            calculateTimes();
            print("You set time array wrong. So I calculated times by myself. :/");
        }

        float t = 0;

        if (TimeMode == timeMode.manual)
        {
            t = manualTimerArray[subtIndex];
        }
        else
        {
            t = automaticTimerArray[subtIndex];
        }

        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return 0;
        }

        text.text = "";

        yield break;
    }

}
