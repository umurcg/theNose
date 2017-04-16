using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class DeadManGameController : GameController {



    // Use this for initialization
    public override void Start()
    {
        base.Start();
        CharGameController.getSun().GetComponent<DayAndNightCycle>().makeNight();
        Timing.RunCoroutine(dialogue());
    }


    IEnumerator<float> dialogue()
    {
        yield return 0;

        handlerHolder = Timing.RunCoroutine(Vckrs._lookTo(player, gameObject, 1f));
        yield return Timing.WaitUntilDone(handlerHolder);

        sc.callSubtitleWithIndex(0);

        registerAsUsed();

        yield break;
    }

    // Update is called once per frame
    void Update () {
	
	}

}
