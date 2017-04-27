using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using MovementEffects;

public class BridGameController : MonoBehaviour {
        
    public GameObject uiText;
    public GameObject lowPolyBird;
    public GameObject highPolyBird;
    public GameObject wizard;
    public float fadeSpeed = 0.3f;

    Text uiT;
    public int maxHint;
    DrawEdgesBetweenVertices debv;


    Quaternion initialRot;

    public bool debug;

	// Use this for initialization
	void Awake () {
        debv=GetComponent<DrawEdgesBetweenVertices>();
        initialRot = transform.rotation;
        uiT = uiText.GetComponent<Text>();
        score();
        uiText.transform.parent.gameObject.SetActive(true);
        
	}
    private void Start()
    {

    }

    // Update is called once per frame
    void Update () {
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
        uiT.text="Remained number of edges is "+ debv.getRemainedNumberOfEdges();
        Debug.Log("Score");
    }

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
