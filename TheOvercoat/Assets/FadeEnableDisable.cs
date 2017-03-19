using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class FadeEnableDisable : MonoBehaviour {

    public float fadeSpeed = 1f;
    
	// Use this for initialization
	void Start () {
        Enable();
	}
	
	// Update is called once per frame
	void Update () {
	
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
        if (!gameObject.activeSelf) yield break;

        IEnumerator<float> handler = Vckrs._fadeObject(gameObject, fadeSpeed, true);

        yield return Timing.WaitUntilDone(handler);

        yield return Timing.WaitForSeconds(1);

        gameObject.SetActive(false);

        yield break;
    }

    public IEnumerator<float> _Enable()
    {
        if (gameObject.activeSelf) yield break;

        IEnumerator<float> handler = Vckrs._fadeObject(gameObject, fadeSpeed, true);


        yield return Timing.WaitUntilDone(handler);

        gameObject.SetActive(true);

        yield break;
    }

}
