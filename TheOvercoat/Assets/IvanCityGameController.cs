using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//This game controller only calls narrator subtitle at first. But if you add to that scene some games you can use this script

public class IvanCityGameController : GameController {



	// Use this for initialization
	public override void Start () {
        base.Start();

        //Debug.Log("Hello");
        Timing.RunCoroutine(_narratorSpeech());


	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator<float> _narratorSpeech()
    {
        pcc.StopToWalk();
        yield return Timing.WaitForSeconds(3f);

        sc.callSubtitleWithIndexTime(0);
        while (narSubtitle.text != "") yield return 0;

        pcc.ContinueToWalk();

        registerAsUsed();
    }

    public override void activateController()
    {
        base.activateController();
        if (isUsed()) return;
        enabled = true;
    }

    public override void deactivateController()
    {
        base.deactivateController();
        enabled = false;
    }
}
