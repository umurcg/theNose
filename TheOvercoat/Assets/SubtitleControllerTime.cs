using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubtitleControllerTime : SubtitleController {
    public float[] manualTimerArray;
    public enum timeMode {automatic,manual };
    public timeMode TimeMode;

   
    public float timeInhibator=13;
    float[] automaticTimerArray;

    float timer = 0;
    // Use this for initialization
    new void Start () {
        text = subtitle.GetComponent<Text>();
        if (text == null)
        {
            print("PUT THE SUBTITLE YOU FAGGOT");
        }
        index = -1;
        this.enabled = false;
        
        
    }
	
	// Update is called once per frame
	new void Update () {
        //print(timer);

        if (timer > 0)
            timer -= Time.deltaTime;

        if (index != -1&&timer<=0)
        {         

            if (index < subtitleTexts.Length -1)
            {
                
                index++;
                text.text = subtitleTexts[index];
                assignTimer();

            }else if(timer<=0)
            {
                timer = 0;
                text.text = "";
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
        Debug.Log("starting subtitles, obj name " + name);

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
        assignTimer();

    }

    //fills time array
    void calculateTimes()
    {
        Debug.Log("Calculating times, obj name " + name);
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
}
