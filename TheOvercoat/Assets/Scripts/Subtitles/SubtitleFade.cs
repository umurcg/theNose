using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class SubtitleFade : MonoBehaviour {

    public static Dictionary<string, Text> subtitles;

    Text subt;
    //bool fadedIn=false;
    Color color;
    RawImage ri;
    public float t = 0;
    public float fadeSpeed=3;

    public float maxTransparency = 0.5f;

	// Use this for initialization
	void Awake () {

        if (subtitles == null)
        {
            subtitles = new Dictionary<string, Text>();
        }
       
        subt = GetComponentInChildren<Text>();
        subtitles.Add(gameObject.name, subt);
        ri = GetComponent<RawImage>();
        color = ri.color;
        subt.text = "";
	}
	
	// Update is called once per frame
	void Update () {


        if (subt.text != ""&&color.a!=maxTransparency)
        {
            if (color.a > maxTransparency)
            {
                color.a = maxTransparency;
                t = 1;
            }else
            {
                t += fadeSpeed * Time.deltaTime;
                color.a = Mathf.Lerp(0, maxTransparency, t);
            }

        } else if (subt.text == "" && color.a != 0)
        {
            if (color.a <0)
            {
                color.a = 0;
                t = 0;
            }
            else
            {
                t -= fadeSpeed * Time.deltaTime;
                color.a = Mathf.Lerp(0, maxTransparency, t);
           //     print(color.a);
            }

        }

        ri.color = color;

        //if (subt.text !="" && !fadedIn)
        //{
        //    t = fadeSpeed * Time.deltaTime;
        //    color.a+=Mathf.Lerp(0,0.5f,t);
         
        //    ri.color = color;
        //    if (color.a>=0.5)
        //    {
        //        color.a = 0.5f;
        //        fadedIn = true;
        //    }
        //}  else if (subt.text == "" && fadedIn)
        //{
        //    t = fadeSpeed * Time.deltaTime;
        //    color.a -= Mathf.Lerp(0, 0.5f, t);
        //    ri.color = color;
        //    if (color.a <=0)
        //    {
        //        color.a = 0;
        //        fadedIn = false;
        //    }
        //}  else if (subt.text != "" && !fadedIn)
        //{


        //}
        
        
        
         
	}

    void OnDisable()
    {
        if (subtitles.Count>0)
        {
            subtitles.Clear();
        }
    }
}
