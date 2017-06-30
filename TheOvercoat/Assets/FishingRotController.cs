using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public class FishingRotController : MonoBehaviour {

    Animator ac;
    [HideInInspector]
    public bool catcthAnim, throwAnim, cameAnim;

	// Use this for initialization
	void Start () {
        ac = GetComponent<Animator>();
        ac.speed = 300;
        Timing.RunCoroutine(fixSpeed(0.1f));
        
    }
	
	// Update is called once per frame
	void Update () {

        if (cameAnim)
        {
            ac.SetBool("FishCame", true);
            cameAnim = false;
        }else if (catcthAnim)
        {
            ac.SetBool("FishCame", false);
            ac.SetTrigger("Catch");
            catcthAnim = false;
        }else if (throwAnim)
        {
            ac.SetTrigger("Throw");
            throwAnim = false;
        }

	}


    public void fishFound()
    {
        ac.SetBool("FishCame", true);
    }


    public void catchFish()
    {
        ac.SetBool("FishCame", false);
        ac.SetTrigger("Catch");
    }

    public void throwHook()
    {
        ac.SetTrigger("Throw");
    }

    IEnumerator<float> fixSpeed(float time)
    {
        yield return Timing.WaitForSeconds(time);
        ac.speed = 1;
        yield break;
    }

}
