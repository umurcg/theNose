﻿using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine.UI;

public class WizardController : GameController {

    public GameObject throne;
    characterComponents pontiffCC;

    // Use this for initialization
    public override void Start () {
        base.Start();
        pontiffCC = new characterComponents(gameObject);

        Timing.RunCoroutine(loadCity());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void basement()
    {
        //Debug.Log("Basement trigered");
        Timing.RunCoroutine(_basement());
    }

    IEnumerator<float> _basement()
    {
        //First subt
        sc.callSubtitleWithIndex(0);

        while (subtitle.text != "") yield return 0;

        //Pontiff get closer
        pontiffCC.navmashagent.SetDestination(player.transform.position + Vector3.forward * 3);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(gameObject, 0));
        
        yield return Timing.WaitUntilDone(handlerHolder);

        //Second sub
        sc.callSubtitleWithIndex(1);
        while (subtitle.text != "") yield return 0;

        playerNma.Resume();
        playerNma.SetDestination(throne.transform.position + throne.transform.forward * 1);

        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(player));
        yield return Timing.WaitUntilDone(handlerHolder);

        WalkLookAnim wla = throne.GetComponent<WalkLookAnim>();
        wla.start();

        //Wait for the sit
        while (!wla.isSitting()) yield return 0;

        pontiffCC.navmashagent.SetDestination(throne.transform.position + throne.transform.forward * 4);

        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(player));
        yield return Timing.WaitUntilDone(handlerHolder);

        //Third sub
        sc.callSubtitleWithIndex(2);
        while (subtitle.text != "") yield return 0;






    }

    IEnumerator<float> loadCity()
    {        
        //It is just for now. I will add a mini game here.
        LoadScene ls = GetComponent<LoadScene>();
        blackScreen bs = blackScreen.obj.GetComponent<blackScreen>();

        IEnumerator<float> handler = blackScreen.script.fadeOut();
        yield return Timing.WaitUntilDone(handler);
        
        //Set bird as main character
        player=CharGameController.setCharacter("Bird");
        CharGameController.getCamera().GetComponent<CameraFollower>().updateTarget();


        //Set bird anim to land and set the birdlandscript according to that
        BirdLandingScript bls = player.GetComponent<BirdLandingScript>();
        Animator anim = player.GetComponent<Animator>();
        anim.SetBool("land", true);
        bls.setAsLanded(true);

        //Load
        ls.Load();

    }
}