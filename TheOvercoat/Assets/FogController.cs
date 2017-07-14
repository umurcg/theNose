using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class FogController : MonoBehaviour {

    public GameObject fogGameControllerObj;
 
    FogGameController fgc;

    public ParticleSystem ps;

    [HideInInspector]
    public int numberOfGem;

    Text gemText;

    // Use this for initialization
    void Start () {
        fgc = fogGameControllerObj.GetComponent<FogGameController>();
        numberOfGem = transform.childCount;
        gemText = fgc.windUI.GetComponentInChildren<Text>();
        gemText.text = "x" + numberOfGem;
    }
	
	// Update is called once per frame
	void Update () {
        if (numberOfGem <= 0)
        {
            Timing.RunCoroutine(clearFog());
            enabled = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player" && fgc)
             fgc.inFog = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && fgc)
            fgc.inFog = false;
    }

    public FogGameController getGameController()
    {
        return fgc;
    }

    IEnumerator<float> clearFog()
    {


        Gradient gradient= ps.colorOverLifetime.color.gradient;
        var colorOverLifetime = ps.colorOverLifetime;
        float alpha = gradient.alphaKeys[1].alpha;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * 0.1f;


            gradient.SetKeys(gradient.colorKeys, new GradientAlphaKey[] { gradient.alphaKeys[0], new GradientAlphaKey(alpha,0.5f), gradient.alphaKeys[2] });


            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

            //Debug.Log(key.alpha);
            yield return 0;
        }


        alpha = 0;
        gradient.SetKeys(gradient.colorKeys, new GradientAlphaKey[] { gradient.alphaKeys[0], new GradientAlphaKey(alpha, 0.5f), gradient.alphaKeys[2] });
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

        //Debug.Log(gradient.alphaKeys[0].alpha + " " + gradient.alphaKeys[1].alpha+" " + gradient.alphaKeys[2].alpha);

        ps.Stop();
        gameObject.SetActive(false);

        fgc.inFog = false;
        fgc.fogIsDestroyed(this);

        Destroy(this);

        yield break;
    }

    public void gemIsCollected()
    {
        numberOfGem--;
        gemText.text = "x" + numberOfGem;
    }

    
}
