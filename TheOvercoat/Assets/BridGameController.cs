using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using MovementEffects;

public class BridGameController : MonoBehaviour {
        
    public GameObject uiText;
    //public GameObject buttonPrefab;
    public Transform canvas;
    public GameObject lowPolyBird;
    public GameObject highPolyBird;
    public GameObject wizard;
    public float fadeSpeed = 0.3f;

    Text uiT;
    //Button button;

    public int maxHint;
    DrawEdgesBetweenVertices debv;

    string textMessage = "";

    Quaternion initialRot;

    public bool debug;

	// Use this for initialization
	void Awake () {

        debv=GetComponent<DrawEdgesBetweenVertices>();
        initialRot = transform.rotation;
        instantiateButtonAndText();

         

        
        
	}
    private void Start()
    {
        textMessage = GetComponent<DynamicLanguageTexts>().getTextForCurrentLanguage();

        score();
    }

    void instantiateButtonAndText()
    {
        //GameObject b=Instantiate(buttonPrefab,canvas) as GameObject;
        GameObject t = Instantiate(uiText, canvas) as GameObject;

        DynamicLanguageTexts lantext = t.GetComponent<DynamicLanguageTexts>();
        if (lantext) Destroy(lantext);

        t.transform.position = Vckrs.screenRatioToPosition(0.25f, 0.75f);
        //b.transform.position = t.transform.position - t.transform.up * Screen.height / 5;

        uiT = t.GetComponentInChildren<Text>();
        //button = b.GetComponentInChildren<Button>();


    }

    // Update is called once per frame
    void Update () {

        if(Debug.isDebugBuild && Input.GetKeyDown(KeyCode.T))
        {
            win();
        }

        if (debug)
        {
            debug = false;
            win();
        }
	}

    public void giveHint()
    {
        debv.giveHint();
    }

    public void score()
    {
        //Update score
        uiT.text= textMessage + debv.getRemainedNumberOfEdges();
        Debug.Log("Score");
    }

    [ContextMenu ("win")]
    public void win()
    {
        Timing.RunCoroutine(_win());

    }
    
    IEnumerator<float> _win()
    {
        fadeEveryChild();

        //Rotate to initial rotation
        float ratio=0;
        Quaternion lastRot = transform.rotation;

        while (ratio < 1)
        {
            transform.rotation = Quaternion.Slerp(lastRot, initialRot, ratio);
            ratio += Time.deltaTime * 0.5f;
            yield return 0;
        }

        yield return Timing.WaitForSeconds(1f);

        Renderer rend = GetComponent<Renderer>();
        Vckrs.makeObjectTransparent(gameObject);
        rend.enabled = true;
        IEnumerator<float> handler=Timing.RunCoroutine(Vckrs._fadeObjectIn(gameObject, fadeSpeed));
        yield return Timing.WaitUntilDone(handler);

        yield return Timing.WaitForSeconds(1f);

        Renderer rendLow = lowPolyBird.GetComponent<Renderer>();
        Vckrs.makeObjectTransparent(lowPolyBird);
        rendLow.enabled = true;
        handler = Timing.RunCoroutine(Vckrs._fadeObjectIn(lowPolyBird, fadeSpeed));
        Timing.RunCoroutine(Vckrs._fadeObjectOut(gameObject, fadeSpeed, true));
        yield return Timing.WaitUntilDone(handler);

        yield return Timing.WaitForSeconds(1f);

        Renderer rendHigh = highPolyBird.GetComponent<Renderer>();
        Vckrs.makeObjectTransparent(highPolyBird);
        rendHigh.enabled = true;
        handler = Timing.RunCoroutine(Vckrs._fadeObjectIn(highPolyBird, fadeSpeed));
        Timing.RunCoroutine(Vckrs._fadeObjectOut(lowPolyBird, fadeSpeed, true));
        yield return Timing.WaitUntilDone(handler);

        yield return Timing.WaitForSeconds(1f);


        wizard.GetComponent<WizardController>().loadCity();

    }

    void fadeEveryChild()
    {
        
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        Debug.Log("nUMBER of renderers " + rends.Length);
        foreach (Renderer r in rends)
        {
            Timing.RunCoroutine(Vckrs._fadeObjectOut(r, 1f, true));
        }
    }


}
