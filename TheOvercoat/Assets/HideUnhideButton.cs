using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;
//Hides and unhides button with text

public class HideUnhideButton : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
   
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void activate()
    {
        Timing.RunCoroutine(_activate());
    }

    public void deactivate()
    {
        Timing.RunCoroutine(_deactivate());
    }

    public IEnumerator<float> _deactivate()
    {
        //is active or not
        bool isActive = gameObject.activeSelf;
        if (!isActive) yield break;

        IEnumerator<float> handler = null;

        handler= Timing.RunCoroutine(Vckrs._fadeInfadeOut<MaskableGraphic>(gameObject, 1f));

        if(handler!=null) yield return Timing.WaitUntilDone(handler);

        gameObject.SetActive(false);
    }

        public IEnumerator<float> _activate()
         {
        //Debug.Log("Activating");
        //is active or not
        bool isActive = gameObject.activeSelf;
        if (isActive) yield break;
        gameObject.SetActive(true);

        //IEnumerator<float> handler=Timing.RunCoroutine(Vckrs._fadeInfadeOutImage(gameObject, 1f));
        //IEnumerator<float> handler2=Timing.RunCoroutine(Vckrs._fadeInfadeOutText(transform.GetChild(0).gameObject, 1f));

        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs._fadeInfadeOut<MaskableGraphic>(gameObject, 1f));

        yield return Timing.WaitUntilDone(handler);
        //yield return Timing.WaitUntilDone(handler2);

  
       }

}
