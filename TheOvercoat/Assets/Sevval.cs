﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class Sevval : GameController, IClickAction {

    public GameObject paintingGame;
    Vector3 originalLook;
    Animator ac;

    public override void Start()
    {
        base.Start();
        originalLook = transform.position + transform.forward;
        ac = GetComponent<Animator>();

        //startPainting();

     }

    // Update is called once per frame
    void Update () {
	
	}

    public override void Action()
    {
        Timing.RunCoroutine(paintConv(!isUsed()));
    }

    IEnumerator<float> paintConv(bool firstTime)
    {
        pcc.StopToWalk();
        ac.SetBool("Paint", false);

        handlerHolder= Timing.RunCoroutine(Vckrs._lookTo(gameObject, player, 1f));

        yield return Timing.WaitUntilDone(handlerHolder);



        if (firstTime)
        {
            sc.callSubtitleWithIndex(0);
        }else
        {
            sc.callSubtitleWithIndex(2);
        }

        while (subtitle.text != "") yield return 0;

        if(firstTime)
            registerAsUsed();

        //Start painting
        startPainting();

        //finishPainting();

        yield break;

    }

    public void finishPainting()
    {
        Timing.RunCoroutine(_finishPainting());
    }

    public void startPainting()
    {
        pcc.StopToWalk();
        paintingGame.SetActive(true);
    }


    IEnumerator<float> _finishPainting()
    {

        paintingGame.SetActive(false);

        sc.callSubtitleWithIndex(1);
        while (subtitle.text != "") yield return 0;

        pcc.ContinueToWalk();
        ac.SetBool("Paint", true);

        Timing.RunCoroutine(Vckrs._lookTo(gameObject, originalLook, 1f));

        pcc.ContinueToWalk();

        

        yield break;

    }


    public override void deactivateController()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public override void activateController()
    {
        transform.parent.gameObject.gameObject.SetActive(true);
    }

}
