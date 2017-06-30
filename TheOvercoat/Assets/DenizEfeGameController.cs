using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class DenizEfeGameController : GameController {


    public FishGameController fgc;
    public FisherStandController fsc;
    public GameObject bucket;

	
	// Update is called once per frame
	void Update () {
	
	}

    public override void gameIsUsed()
    {
        base.gameIsUsed();
        gameObject.SetActive(false);
    }

    void callKovalev()
    {
        if(!isUsed())
            sc.callSubtitleWithIndex(2);
    }

    public void startConverstaion()
    {
        if (isUsed())
        {
            loopSubtitle();
        }
        else
        {
            Debug.Log("Startgin conversation");
            Timing.RunCoroutine(_startConverstaion());
        }
    }

    public void fishingGameIsFinished()
    {
        Timing.RunCoroutine(_fishingGameIsFinished());
    }

    IEnumerator<float> _startConverstaion()
    {

        sc.callSubtitleWithIndex(1);
        while (subtitle.text != "") yield return 0;

        fgc.gameObject.SetActive(true);

    }
    

    public void loopSubtitle()
    {
        sc.callSubtitleWithIndex(3);
    }

    IEnumerator<float> _fishingGameIsFinished()
    {
        fgc.gameObject.SetActive(false);
        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "") yield return 0;

        registerAsUsed();
        ActivateAnotherObject.Activate(bucket);

        fsc.transform.tag = "ActiveObject";
        fsc.enabled = true;
        fsc.bucket = bucket;

        yield break;
    }

    public override void activateController()
    {
        gameObject.SetActive(true);
    }

    public override void deactivateController()
    {
        gameObject.SetActive(false);
    }


}
