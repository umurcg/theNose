using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

//TODO kovaev position when canvas is activated


public class NoseEncounterGameController : GameController {
    public GameObject Horse, Nose, trash, newspaper, Girlgame, girl,girlCanvas, girlGameKov, allHorses/*, Obstacles, HorseAimOne, SubHolder,  Girlgame, LightObj, HorseAimTwo*/;

    public bool debugCatch;

    HorseScript hs;
    characterComponents noseCC;


    public float walkTolerance = 0.01f;
    EnterTrigger NoseEt;
    MountCarier mc;


    //Set this as object that you want to player look at.
    GameObject lookAtObject=null;

    //public GameObject IvanSubtitleHolder;
    //SubtitleCaller ivanSc;

    //For tracing obstacle avoidence typ of player
    UnityEngine.AI.ObstacleAvoidanceType playerInitialAvoidenceType;
    UnityEngine.AI.ObstacleAvoidanceType nosesInitialAvoidenceType;

    List<OpenDoorLoad> activeDoors;

    public AudioClip encounterSoundEffect;

    // Use this for initialization
    public override void Start () {
        base.Start();

        initilasePosition();

        hs = Horse.GetComponent<HorseScript>();
        noseCC = new characterComponents(Nose);

        startNoseGame();

        //debugGirlGame();


        //enbaleTrashObjs(true);

        //noseCatched();
        //setGirlPosition();


    }

    void disableEnableHorses(bool enable)
    {
        if (enable) {
            for (int i = 0; i < allHorses.transform.childCount; i++)
            {
                allHorses.transform.GetChild(i).tag = "ActiveObject";
            }
        }else
        {
            for (int i = 0; i < allHorses.transform.childCount; i++)
            {
                allHorses.transform.GetChild(i).tag = "Untagged";
            }
        }
    }


    void initilasePosition()
    {
        //Vector3 pos = player.transform.position + player.transform.forward * 50;
        //NavMeshHit nmh;
        //if(NavMesh.SamplePosition(pos, out nmh, 400f, Horse.GetComponent<NavMeshAgent>().areaMask))
        //{
        //    transform.position = nmh.position;
        //}else
        //{
        //     Debug.Log("Failed to initilize");
        //}


        Vckrs.setPositionToOutsideOfCameraAndOnNavmesh(gameObject, player.transform.position, 100, CharGameController.getCamera().GetComponent<Camera>());


    }

    // Update is called once per frame
    void Update () {

        //Constantly lookat object
        if(lookAtObject!=null)
           player.transform.LookAt(lookAtObject.transform);

        if (debugCatch)
        {
            debugCatch = false;
            noseCatched();
        }


	}

    public void startNoseGame()
    {     
        Timing.RunCoroutine(_start());
    }


    IEnumerator<float> _start()
    {
        yield return 0;

        //Lock all open doors
        lockUnlockAllDoors(true);

        CameraFollower cf=CharGameController.getCamera().GetComponent<CameraFollower>();

        //hs.release();
        handlerHolder = hs.setDes(player.transform.position + Vector3.forward * 20);
        pcc.StopToWalk();

        lookAtObject = Nose;

        sc.callSubtitleWithIndex(0);


        yield return Timing.WaitForSeconds(1f);

        //Playe effect
        LevelMusicController.playSoundEffect(encounterSoundEffect);

        float originalDamper = cf.damper;

        //Set camera to nose
        cf.damper = 2;
        cf.changeTarget(Nose);

        //cf.changeTargetWithLerp(Nose,0.3f);

        yield return Timing.WaitUntilDone(handlerHolder);



        hs.unmount();

        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Nose));
        yield return Timing.WaitUntilDone(handlerHolder);



        Vector3 pos = Vckrs.generateRandomPositionOnCircle(Nose.transform.position, 10f);
        UnityEngine.AI.NavMeshHit nmh;
        if((UnityEngine.AI.NavMesh.SamplePosition(pos, out nmh, 15f, noseCC.navmashagent.areaMask))){

            noseCC.navmashagent.SetDestination(nmh.position);
        }else
        {
            Debug.Log("Couldnt found");
            noseCC.navmashagent.SetDestination(pos);
        }

   
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Nose));
        yield return Timing.WaitUntilDone(handlerHolder);

        //Set camera to active character
        yield return Timing.WaitForSeconds(1f);
        //cf.changeTargetWithLerp(player, 0.3f);
        cf.changeTarget(player);


        //Timing.RunCoroutine(Vckrs._lookTo(player, Nose, 1));

        newspaper.SetActive(true);
        Nose.transform.parent = transform;

        lookAtObject = null;

        

        while (subtitle.text != "") yield return 0;


        sc.callSubtitleWithIndex(1);
        while (subtitle.text != "") yield return 0;

        yield return Timing.WaitForSeconds(3f);

        cf.damper = originalDamper;

        sc.callSubtitleWithIndex(2);
        while (subtitle.text != "") yield return 0;

    

        int numberOfTrial = 3;
        bool inside = false;

        while (Vector3.Distance(player.transform.position, Nose.transform.position) > Nose.GetComponent<SphereCollider>().radius || numberOfTrial>0)
        {
            //Debug.Log(Vector3.Distance(player.transform.position, Nose.transform.position) + " "+Nose.GetComponent<SphereCollider>().radius);
            if (!(Vector3.Distance(player.transform.position, Nose.transform.position) > Nose.GetComponent<SphereCollider>().radius) && !inside)
            {
                numberOfTrial--;
                inside = true;
            }else if (inside)
            {
                if ((Vector3.Distance(player.transform.position, Nose.transform.position) > Nose.GetComponent<SphereCollider>().radius))
                    inside = false;

            }

            yield return 0;

        }
        sc.callSubtitleWithIndexTime(0);

        enbaleTrashObjs(true);

    }

    public void enbaleTrashObjs(bool b) {

        //If you enable trash objects you should set player navmesh obstacle avoidence to none because we don't want to
        //player avoid from boks 
        //If you disable it you should recover first obstacle settings to not leaving a trace
        //Also you should set nose avoidence type to high quality, iftrash objects are enabled while he should fear from boks
        //If you disabletrash objects you should recover its initial av.type too
        if (b)
        {
            playerInitialAvoidenceType = playerNma.obstacleAvoidanceType;
            playerNma.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

            nosesInitialAvoidenceType= noseCC.navmashagent.obstacleAvoidanceType;
            noseCC.navmashagent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
        else
        {
           if(playerInitialAvoidenceType!=UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance) playerNma.obstacleAvoidanceType = playerInitialAvoidenceType;
            noseCC.navmashagent.obstacleAvoidanceType = nosesInitialAvoidenceType;
        }

        for (int i = 0; i < trash.transform.childCount; i++)
        {

            GameObject child = trash.transform.GetChild(i).gameObject;
            CollectableObjSupplier cos = child.GetComponent<CollectableObjSupplier>();
            ChangeMaterial cm =child.GetComponent<ChangeMaterial>();

            if (b)
            {
                cos.enabled = enabled;
                child.tag = "ActiveObject";
                cm.changeWithIndex(1);
            }
            else
            {
                cos.clearInventory();
                cos.enabled = false;
                child.tag = "Untagged";
                cm.changeWithIndex(0);

            }
        }

    }

    [ContextMenu("nose catched")]
    public void noseCatched()
    {
        Timing.RunCoroutine(_noseCatched());
    }

 
    IEnumerator<float> _noseCatched()
    {
        newspaper.SetActive(false);
        enbaleTrashObjs(false);

        noseCC.navmashagent.Stop();
        Timing.RunCoroutine(Vckrs._lookTo(Nose,player,1f));

        Timing.RunCoroutine(Vckrs._lookTo(player, Nose, 1f));

        pcc.StopToWalk();
        sc.callSubtitleWithIndex(3);
        while (subtitle.text != "")
        {
            yield return 0;
        }

        sc.callSubtitleWithIndexTime(1);

        setGirlPosition();

        Vector3 girlPosition = girl.transform.position;
        UnityEngine.AI.NavMeshHit nmh;

        if ((UnityEngine.AI.NavMesh.SamplePosition(girlPosition, out nmh, 10f, girl.GetComponent<UnityEngine.AI.NavMeshAgent>().areaMask)))
        {
            girlPosition = nmh.position;
            
        }

        girl.transform.position=girlPosition;

        girl.SetActive(true);

        
        UnityEngine.AI.NavMeshAgent nmaGirls = girl.GetComponent<UnityEngine.AI.NavMeshAgent>();
        nmaGirls.Resume();
        nmaGirls.SetDestination(player.transform.position + player.transform.forward * 5);


        //danc.makeNight();

        GameObject sun = CharGameController.getSun();
        DayAndNightCycle danc = sun.GetComponent<DayAndNightCycle>();

        IEnumerator<float> lightHnadler = Timing.RunCoroutine(Vckrs._setLightIntensity(sun,0.5f,danc.minIntensity));

        lookAtObject = girl;

        IEnumerator<float> girlWalkHandler = Timing.RunCoroutine(Vckrs.waitUntilStop(girl));
        yield return Timing.WaitUntilDone(girlWalkHandler);

        Timing.RunCoroutine(Vckrs._lookTo(girl, player, 1f));

        lookAtObject = null;
        //Timing.RunCoroutine(Vckrs._lookTo(player, girl.transform.position - player.transform.position, 2f));
        

        girlCanvas.SetActive(true);

        yield return 0;
        //girlCanvas.GetComponent<GirlGameController>().setKovalevPositionToInitialPosition();

        //yield return Timing.WaitForSeconds(0.5f);


        //GameObject girlGameKov = girlCanvas.transform.GetChild(0).GetChild(0).gameObject;
        //girlGameKov.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(300, 0, 0));


        //MovementWithKeyboard2D mwk2 = girlGameKov.GetComponent<MovementWithKeyboard2D>();
        //mwk2.speed = 0.05f;

        ////For movement to left
        //mwk2.scriptInput = -1;

    
        GirlGameController ggc = girlCanvas.GetComponent<GirlGameController>();
        //yield return Timing.WaitUntilDone(Timing.RunCoroutine(ggc.moveKovalevToMiddle(0.05f)));

        //yield return Timing.WaitForSeconds(4f);

        //mwk2.scriptInput = 0;
        //mwk2.speed = 0.1f;

        CharacterController gkcc = girlGameKov.GetComponent<CharacterController>();
        gkcc.enabled = false;
        gkcc.enabled = true;

        //GirlGameController ggc = girlCanvas.transform.GetComponent<GirlGameController>();
        ggc.enabled = true;

        yield return Timing.WaitForSeconds(1f);

        IEnumerator<float> handler = hs.mount();

        yield return Timing.WaitUntilDone(handler);

        hs.setDes(Horse.transform.position + Horse.transform.forward * -40f);

        yield return Timing.WaitForSeconds(5f);
        Horse.transform.parent.gameObject.SetActive(false);
        Nose.SetActive(false);

        //unlock all open doors
        lockUnlockAllDoors(false);
        registerAsUsed();

        yield break;

    }

    void debugGirlGame()
    {

        pcc.StopToWalk();

        girlCanvas.SetActive(true);

        //yield return 0;
        //girlCanvas.GetComponent<GirlGameController>().setKovalevPositionToInitialPosition();

        MovementWithKeyboard2D mwk2 = girlGameKov.GetComponent<MovementWithKeyboard2D>();

        mwk2.speed = 0.05f;

        //For movement to left
        mwk2.scriptInput = -1;
        //yield return Timing.WaitForSeconds(4f);

        mwk2.scriptInput = 0;
        mwk2.speed = 0.1f;

        CharacterController gkcc = girlGameKov.GetComponent<CharacterController>();
        gkcc.enabled = false;
        gkcc.enabled = true;

        GirlGameController ggc = girlCanvas.transform.GetComponent<GirlGameController>();
        ggc.enabled = true;
    }


    //Sets girl position to suitable place which is on navmesh and outside of camera but near of player
    //TODO make it static in vckrs
    public void setGirlPosition()
    {

        Vckrs.setPositionToOutsideOfCameraAndOnNavmesh(girl, player.transform.position, 100, CharGameController.getMainCameraComponent(), 30, 50);

        ////Set girl position
        ////Vector3 girlPosition =player.transform.position+  Vector3.Normalize(Horse.transform.position - player.transform.position) * 20;

        //Vector3 positionLimit1 = Vckrs.generateRandomPositionOnCircle(player.transform.position, 30f);
        //Vector3 positionLimit2= Vckrs.generateRandomPositionOnCircle(player.transform.position, 50f);
        //Vector3 position = Vector3.Lerp(positionLimit1, positionLimit2, Random.Range(0, 1));


        //bool onNavmesh = false;
        //bool outsideOfCam = false;

        //int trial = 100000;


        //while ((!onNavmesh || !outsideOfCam) && trial>0)
        //{
        //    //Cast girl position to navmesh and check it is on navmesh
        //    NavMeshHit nmh;
        //    if ((NavMesh.SamplePosition(position, out nmh, 2f, girl.GetComponent<NavMeshAgent>().areaMask)))
        //    {
        //        position = nmh.position;
        //        onNavmesh = true;
        //    }else
        //    {
        //        onNavmesh = false;
        //    }

        //    //Check girl is outisde of camera
        //    outsideOfCam=!Vckrs.canCameraSeeObject(position, CharGameController.getCamera().GetComponent<Camera>());
            
    
        //    trial--;
        //}

        //if (trial <= 0)
        //{
        //    Debug.Log("Couldn't find appropiate position for girl");
        //}else
        //{
        //    Debug.Log("Found appropiate position for girl");
        //}

        //girl.transform.position = position;

    }

    public void noseGoneLost()
    {
        Timing.RunCoroutine(_noseGoneLost());
    }

    IEnumerator<float> _noseGoneLost()
    {

        DayAndNightCycle danc = CharGameController.getSun().GetComponent<DayAndNightCycle>();
        danc.makeDay();

        sc.callSubtitleWithIndex(4);
        while (subtitle.text != "")
        {
            yield return 0;
        }


    }

    void lockUnlockAllDoors(bool lockDoor)
    {
      

        if (lockDoor)
        {
            activeDoors = OpenDoorLoad.getAllActiveDoors();
        }
        else if (activeDoors == null) return;


        Debug.Log("Active door number is " + activeDoors.Count);

        foreach(OpenDoorLoad door in activeDoors)
        {
            if (lockDoor)
            {
                door.Lock();
                Debug.Log("All doors is locked");
            }
            else
            {
                Debug.Log("All doors is unlocekd");
                door.Unlock();
                activeDoors = null;
            }
        }

        

    }

    public override void gameIsUsed()
    {
        base.gameIsUsed();
        Destroy(gameObject);
        
    }

    //[ContextMenu("Girl game test")]
    //public void girlGameTest() {

    //    Timing.RunCoroutine(_girlGameTest());

    //}

    //IEnumerator<float> _girlGameTest()
    //{
    //    girlCanvas.SetActive(true);

    //    yield return 0;

    //    GirlGameController ggc = girlCanvas.GetComponent<GirlGameController>();
    //Timing.RunCoroutine(ggc.moveKovalevToMiddle(0.05f));
    //}

}
