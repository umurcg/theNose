﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using MovementEffects;

//This scirpt triggeres maze game.
//When bird enters trigger or click on it, it switches characters with fade screen. For now just 'mplemeent iclick action
//After that it starts game

public class MazeGameStarter : GameController, IClickAction {

    public GameObject birdPrefab, nosePefab;
    public GameObject starterPos;

    // Use this for initialization
    public override void Start () {
        base.Start();


	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //public void movePlayerBirdToStarterPos()
    //{
    //    Timing.RunCoroutine(_movePlayerBirdToStarterPos());
    //}

    //IEnumerator<float> _movePlayerBirdToStarterPos()
    //{
    //    Debug.Log("_movePlayerBirdToStarterPos");
    //    MoveToWithoutAgent mtwa = player.GetComponent<MoveToWithoutAgent>();
    //    BirdLandingScript bls = player.GetComponent<BirdLandingScript>();

    //    handlerHolder = Timing.RunCoroutine(mtwa._lookAndGo(starterPos.transform.position));
    //    Timing.WaitUntilDone(handlerHolder);

    //    bls.setAsLanded(true);

    //    handlerHolder = Timing.RunCoroutine(Vckrs._lookTo(player, nosePefab, 1f));
    //    Timing.WaitUntilDone(handlerHolder);

    //    yield break;
    //}

    IEnumerator<float> _startMazeGame()
    {

        //Disable character mouse look of bird
        CharacterMouseLook cml = player.GetComponent<CharacterMouseLook>();
        cml.enabled = false;

        //Wait for bird to go to start position
        while (Vector3.Distance(player.transform.position, starterPos.transform.position) > 0.1f) yield return 0;

        BirdLandingScript bls = player.GetComponent<BirdLandingScript>();
        bls.setAsLanded(true);

        handlerHolder = Timing.RunCoroutine(Vckrs._lookTo(player, nosePefab, 1f));
        yield return Timing.WaitUntilDone(handlerHolder);

        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "") yield return 0;

        //Fade out
        handlerHolder= blackScreen.script.fadeOut();
        yield return Timing.WaitUntilDone(handlerHolder);

        //Old position of player
        Vector3 oldPos = player.transform.position;

        Quaternion oldRotation = player.transform.rotation;
        float birdSpeed = player.GetComponent<MoveToWithoutAgent>().speed;

        //Disable bot nose and make player to that nose, in other words transform bot to player
        enableDisableSkinnedMesh(nosePefab,false);
        CharGameController.movePlayer(nosePefab.transform.position);
        player=CharGameController.setCharacter("Nose");
        player.transform.rotation = nosePefab.transform.rotation;
        CharGameController.getCamera().GetComponent<CameraFollower>().updateTarget();

        //Add bot bird instead of old bird player
        birdPrefab.transform.position = oldPos;
        birdPrefab.transform.rotation = oldRotation;
        enableDisableSkinnedMesh(birdPrefab, true);

        
        //Fade in
        handlerHolder = blackScreen.script.fadeIn();
        yield return Timing.WaitUntilDone(handlerHolder);

        Timing.WaitForSeconds(3f);

        handlerHolder =Timing.RunCoroutine(sendBirdToAway(oldPos - Vector3.forward * 100 +Vector3.up*20, birdSpeed));
        yield return Timing.WaitUntilDone(handlerHolder);

        Destroy(birdPrefab);

        //Start maze game
        transform.parent.GetComponent<MazeGameController>().enabled = true;

        yield return 0;

        
    }

    IEnumerator<float> sendBirdToAway(Vector3 pos, float speed)
    {
        Animator animBird = birdPrefab.GetComponent<Animator>();
        animBird.SetBool("land", false);

        yield return Timing.WaitForSeconds(3f);

        float dist = Vector3.Distance(pos, transform.position);
        float time = speed / dist;


        IEnumerator<float> localHandler = Timing.RunCoroutine(Vckrs._lookTo(birdPrefab, pos - transform.position, 2f));
        yield return Timing.WaitUntilDone(localHandler);
        localHandler = Timing.RunCoroutine(Vckrs._Tween(birdPrefab, pos, time));
        yield return Timing.WaitUntilDone(localHandler);

        yield break;
    }

    public void Action()
    {
        Timing.RunCoroutine(_startMazeGame());
        Debug.Log("startiing maze");
    }

    void enableDisableSkinnedMesh(GameObject obj, bool enable)
    {
        SkinnedMeshRenderer smr = obj.GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr) smr.enabled = enable;
    }
}