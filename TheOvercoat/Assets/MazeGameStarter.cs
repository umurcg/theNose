using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using MovementEffects;

//This scirpt triggeres maze game.
//When bird enters trigger or click on it, it switches characters with fade screen. For now just 'mplemeent iclick action
//After that it starts game
//Also it makes scene night when bird gets near of nose

public class MazeGameStarter : GameController, IClickAction {

    public GameObject birdPrefab, nosePefab, spawner;
    public GameObject starterPos;
            
    
    DayAndNightCycle danc;
    Light dancL;
    float minInt, maxInt;
    float currentInt;
    float firtsDist;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        danc = CharGameController.getSun().GetComponent<DayAndNightCycle>();
        minInt = danc.minIntensity;
        maxInt = danc.maxIntensity;
        dancL = danc.GetComponent<Light>();
        currentInt = dancL.intensity;
        firtsDist = Vector3.Distance(player.transform.position, nosePefab.transform.position);

        //movePlayerBirdToStarterPos();
    }

    // Update is called once per frame
    void Update()
    {
        //Dim the light while bird is approaching to nose

        //Percentage of distance between bird and nose
        float percentage = Mathf.Clamp(Vector3.Distance(player.transform.position, nosePefab.transform.position) / firtsDist,minInt,maxInt);

        if (percentage < dancL.intensity) dancL.intensity = percentage;



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
        player = CharGameController.getActiveCharacter();

        //Disable character mouse look of bird
        //CharacterMouseLook cml = player.GetComponent<CharacterMouseLook>();
        //cml.enabled = false;

        pcc.StopToWalk();


        //Wait for bird to go to start position
        while (!player.GetComponent<BirdLandingScript>().isBirdOnLand())
        {
            //Debug.Log(Vector3.Distance(player.transform.position, starterPos.transform.position));
            yield return 0;

        }

        BirdLandingScript bls = player.GetComponent<BirdLandingScript>();
        bls.setAsLanded(true);

        handlerHolder = Timing.RunCoroutine(Vckrs._lookTo(player, nosePefab, 1f));
        yield return Timing.WaitUntilDone(handlerHolder);

        Debug.Log("Calling first subt");

        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "") yield return 0;

        //Fade out
        handlerHolder= blackScreen.script.fadeOut();
        yield return Timing.WaitUntilDone(handlerHolder);

        //Old position of player
        Vector3 oldPos = player.transform.position;

        Quaternion oldRotation = player.transform.rotation;
        MoveToWithoutAgent mtwa = player.GetComponent<MoveToWithoutAgent>();
        if (mtwa == null) Debug.Log("mtwa is null");
        float birdSpeed = mtwa.speed;

        //Disable bot nose and make player to that nose, in other words transform bot to player
        enableDisableSkinnedMesh(nosePefab,false);
        CharGameController.movePlayer(nosePefab.transform.position);

        yield return 0;

        player=CharGameController.setCharacter("Nose");
        player.transform.rotation = nosePefab.transform.rotation;
        CharGameController.getCamera().GetComponent<CameraFollower>().updateTarget();
        CharGameController.getCamera().GetComponent<CameraFollower>().fixRelativeToDefault();

        //Add bot bird instead of old bird player
        birdPrefab.transform.position = oldPos;
        birdPrefab.transform.rotation = oldRotation;
        enableDisableSkinnedMesh(birdPrefab, true);

        transform.gameObject.layer = 2;

        //Fade in
        handlerHolder = blackScreen.script.fadeIn();
        yield return Timing.WaitUntilDone(handlerHolder);

        Timing.WaitForSeconds(3f);

        handlerHolder =Timing.RunCoroutine(sendBirdToAway(oldPos - Vector3.forward * 100 +Vector3.up*20, birdSpeed));
        yield return Timing.WaitUntilDone(handlerHolder);

        Destroy(birdPrefab);

        //Start maze game
        transform.parent.GetComponent<MazeGameController>().enabled = true;

        //Enable can you see me scriprs of police bots
        for(int i=0;i<spawner.transform.childCount;i++)
        {
            spawner.transform.GetChild(i).gameObject.GetComponent<CanSeeYou>().enabled = true;
        }

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

    public override void Action()
    {
        base.Action();
        Timing.RunCoroutine(_startMazeGame());
        //When start make remove tag so it can't be started again
        transform.tag = "Untagged";

        Debug.Log("startiing maze");
    }

    void enableDisableSkinnedMesh(GameObject obj, bool enable)
    {
        SkinnedMeshRenderer smr = obj.GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr) smr.enabled = enable;
    }


    public override void activateController()
    {
        base.activateController();
        spawner.SetActive(true);
        gameObject.SetActive(true);
    }

    public override void deactivateController()
    {
        base.deactivateController();
        spawner.SetActive(false);
        gameObject.SetActive(false);
    }
}
