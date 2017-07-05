using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

//This script fades object out when player is in collider
//If player exit from collider it fade object in
public class FadeInOutTrigger : MonoBehaviour {



    public enum FadeType { transparent, fullFade};
    public FadeType fadeType= FadeType.fullFade;
    public GameObject objectToFade;
    IEnumerator<float> handler=null;

    //public bool isTargetOnlyPlayer=false

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Fadeing out");
            Timing.RunCoroutine(fade(true));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Fadeing in");
            Timing.RunCoroutine(fade(false));
           
        }
    }

    IEnumerator<float> fade(bool fade)
    {
        if (handler != null) yield return Timing.WaitUntilDone(handler);
        
        if (fade)
        {
            handler = Timing.RunCoroutine(Vckrs._fadeObjectOut(objectToFade, 1f, fadeType == FadeType.fullFade));
        }else
        {
            handler = Timing.RunCoroutine(Vckrs._fadeObjectIn(objectToFade, 1f, fadeType == FadeType.fullFade));
        }
    }

}
