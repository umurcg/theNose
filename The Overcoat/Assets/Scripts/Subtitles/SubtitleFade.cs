using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SubtitleFade : MonoBehaviour {
    Text subt;
    bool fadedIn=false;
    Color color;
    RawImage ri;
    float t = 0;
    public float fadeSpeed=3;
	// Use this for initialization
	void Awake () {
        subt = GetComponentInChildren<Text>();
        ri = GetComponent<RawImage>();
        color = ri.color;
        subt.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        if (subt.text !="" && !fadedIn)
        {
            t = fadeSpeed * Time.deltaTime;
            color.a+=Mathf.Lerp(0,0.5f,t);
         
            ri.color = color;
            if (color.a>=0.5)
            {
                color.a = 0.5f;
                fadedIn = true;
            }
        }  else if (subt.text == "" && fadedIn)
        {
            t = fadeSpeed * Time.deltaTime;
            color.a -= Mathf.Lerp(0, 0.5f, t);
            ri.color = color;
            if (color.a <=0)
            {
                color.a = 0;
                fadedIn = false;
            }
        } 
	}
}
