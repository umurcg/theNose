using UnityEngine;
using System.Collections;
using MovementEffects;
using UnityEngine.UI;
using System.Collections.Generic;

//This is just for holding black screen object
//So you can find it easily
public class blackScreen : MonoBehaviour {

    public static GameObject obj;
    public static blackScreen script;
    public bool fadeInAtStart=true;
    public float fadeSpeed = 0.5f;

    RawImage ri;

    void Awake()
    {
        if (obj != null || script != null) Destroy(this);

        obj = gameObject;
        script = this;

        //Debug.Log("Hi");

        RectTransform rt = GetComponent<RectTransform>();
        //rt.rect.Set(0,0, Screen.width*2,Screen.height*2);


    }

    

    // Use this for initialization
    void Start () {

        Timing.RunCoroutine(lateStart());

        ////Set size to device screen
        //RectTransform rt = GetComponent<RectTransform>();
        //rt.sizeDelta = new Vector2(Screen.width, Screen.height);        

       ri = GetComponent<RawImage>();

        setAsBlack();

	}

    IEnumerator<float> lateStart()
    {
        yield return 0;
        if (fadeInAtStart)
        {
            //Debug.Log("Fade in at start");
            fadeIn();
        }
        yield break;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //Fade in to scene
    public IEnumerator<float> fadeIn()
    {
        RawImage ri = GetComponent<RawImage>();
        Color color = ri.color;
        color.a = 1;
        ri.color = color;

        return Timing.RunCoroutine(Vckrs._fadeInfadeOut(obj, fadeSpeed));
    }

    //Fade out from scene
    public IEnumerator<float> fadeOut()
    {
        if(!ri)
            ri = GetComponent<RawImage>();

        Color color = ri.color;
        color.a = 0;
        ri.color = color;

        return Timing.RunCoroutine(Vckrs._fadeInfadeOut(obj, fadeSpeed));
    }

    public void setAsBlack()
    {
        if (!ri)
            ri = GetComponent<RawImage>();

        Color color = ri.color;
        color.a = 1;
        ri.color = color;
    }

    public void setAsTransparent()
    {
        if (!ri)
            ri = GetComponent<RawImage>();

        Color color = ri.color;
        color.a = 0;
        ri.color = color;
    }

    public float getAlpha()
    {
        if (!ri)
            ri = GetComponent<RawImage>();

        return ri.color.a;
    }

}
