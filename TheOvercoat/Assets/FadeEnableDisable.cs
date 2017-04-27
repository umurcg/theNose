using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//This script makes object fade in fade out when it is enabled and disabled repectivvely
public class FadeEnableDisable : MonoBehaviour {

    public float fadeSpeed = 1f;
    Renderer rend;
    // Use this for initialization
    void Start () {

        //Debug.Log("Enabled");
        Vckrs.makeObjectTransparent(gameObject, true);
        Enable();
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        Disable();
    }

    public void Disable()
    {
        Timing.RunCoroutine(_Disable());
    }
    
    public void Enable()
    {
        Timing.RunCoroutine(_Enable());
    }

    public IEnumerator<float> _Disable()
    {
        

        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs._fadeObject(gameObject, fadeSpeed, true));

        yield return Timing.WaitUntilDone(handler);

        yield return Timing.WaitForSeconds(1);

        gameObject.SetActive(false);

        yield break;
    }

    public IEnumerator<float> _Enable()
    {
        

        IEnumerator<float> handler =Timing.RunCoroutine(Vckrs._fadeObject(gameObject, fadeSpeed, true));


        yield return Timing.WaitUntilDone(handler);

        gameObject.SetActive(true);

        yield break;
    }

}
