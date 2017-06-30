using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class Cancanfly : GameController,IClickAction {

    public GameObject heatGame;

    IEnumerator<float> startConv()
    {
        sc.callSubtitleWithIndex(0);


        while (subtitle.text != "") yield return 0;

        heatGame.SetActive(true);

        //debug 
        //finishHeadGame();

        yield break;
        
        

    }

    public void finishHeadGame()
    {
        heatGame.SetActive(false);
        Timing.RunCoroutine(continueConv());
    }

    IEnumerator<float> continueConv()
    {
        sc.callSubtitleWithIndex(1);

        while (subtitle.text != "") yield return 0;

        sc.callSubtitleWithIndexTime(0);

        SubtitleController cont = sc.getActiveController();

        while (cont.enabled) yield return 0;
        //while (subtitle.text != "") yield return 0;

        Debug.Log("Starting last subt");

        sc.callSubtitleWithIndex(2);

        
        transform.tag = "Untagged";
    }


    public override void Action()
    {
        //base.Action();

        if (!isUsed())
        {
            Debug.Log("Starting conv");
            Timing.RunCoroutine(startConv());
            registerAsUsed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isUsed()) return;

        sc.callSubtitleWithIndexTime(0);
        pcc.ContinueToWalk();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isUsed()) return;

        sc.termianteCurrentController();

    }



    public override void activateController()
    {
        base.activateController();
        gameObject.SetActive(true);
    }

    public override void deactivateController()
    {
        base.deactivateController();
        gameObject.SetActive(false);
    }





}
