using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KovalevHomeGameController : GameController {
    public GameObject  Door, Ivan, Dolap, Paper, HandR, StartingPoint;

    public GameObject armChair, handMirror,  handMirrorBig, letterToSend, letterRecieved ,tableChair, police,policeNose ,headAttachNose;
   

    //Text charText;
    UnityEngine.AI.NavMeshAgent IvanAgent;
    AlwaysLookTo IvanAlt;
    KeySlideCompletely ksc;

    public enum kovalevHomeScene {KovalevLoosesHisNose,Church,Dream,JustExploring, None };
    public kovalevHomeScene khs;



    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
                      

        sc = GetComponent<SubtitleCaller>();
        IvanAgent = Ivan.GetComponent<UnityEngine.AI.NavMeshAgent>();
        IvanAlt = Ivan.GetComponent<AlwaysLookTo>();

        
        if (GlobalController.Instance == null)
        {

            callWakeUp();
        }
        else
        {

            if (khs == kovalevHomeScene.JustExploring)
            {
                Door.GetComponent<OpenDoorLoad>().Unlock();
                return;
            }

            //First check game objects
            if (GlobalController.Instance.isGameControllerIsUsedSceneNameAndGameObjectName("AtolyeSculpturerGame")/*true*/|| khs==kovalevHomeScene.Dream)
            {
                khs = kovalevHomeScene.Dream;
                Timing.RunCoroutine( comingFromSculpturer());
                return;
            }

            //If user coming for first time and didn't go church 
                if ((!GlobalController.isScnListContains(GlobalController.Scenes.KovalevHouse) && !GlobalController.isScnListContains(GlobalController.Scenes.Church) && khs!=kovalevHomeScene.Church)  || khs==kovalevHomeScene.KovalevLoosesHisNose)
            {
                khs = kovalevHomeScene.KovalevLoosesHisNose;
                //Debug.Log("calling wake up");
                callWakeUp();
                return;
            } //If previous 2nd scene is church
            else if( (GlobalController.Instance.sceneList.Count>=2 &&  GlobalController.Instance.sceneList[GlobalController.Instance.sceneList.Count - 2] == (int)GlobalController.Scenes.Church)|| khs==kovalevHomeScene.Church )
            {
                khs = kovalevHomeScene.Church;
                //Debug.Log("Nose is FOUUUUND!");
                Timing.RunCoroutine(noseIsFound());
                //Timing.RunCoroutine(_finishedRecievedLetter());
            }
            //PLayer comes here for just fun. Just open the door and leave everythink else normal
            else
            {
                khs = kovalevHomeScene.JustExploring;
                Door.GetComponent<OpenDoorLoad>().Unlock();
            }
           
        }
    }

    public override void Start()
    {
        base.Start();

        //Set player as kovalev
        GameObject character = CharGameController.setCharacter("Kovalev");
        CameraFollower cf= CharGameController.getCamera().GetComponent<CameraFollower>();
        Camera cam = CharGameController.getCamera().GetComponent<Camera>();
        cf.updateTarget();
        cf.fixRelativeToDefault();
        cam.orthographicSize = 10;

        //After changing character you should update all pcc is subtitles
        SubtitleController[] scs = GetComponents<SubtitleController>();
        foreach(SubtitleController sc in scs)
        {
            sc.updatePCC();
        }

        updateCharacterVariables();

        //I have to update omponents of cursor image script
        CharGameController.getOwner().GetComponent<CursorImageScript>().updatePlayerVariables();

        //Update chair subject
        tableChair.GetComponent<WalkLookAnim>().changeSubject(character);
        armChair.GetComponent<WalkLookAnim>().changeSubject(character);

    }

    // Update is called once per frame
    void Update () {
	
	}

    //These functions are called in scene that Kovalev founds that his nose is lost :/

    public void callWakeUp()
    {
        Timing.RunCoroutine(_WakeUp());
    }

    IEnumerator<float> _WakeUp()
    {
        if (player == null) player = CharGameController.getActiveCharacter();

        CharGameController.movePlayer(Vector3.zero);
     

        //wait one frame
        yield return 0;
        

        Debug.Log("wake up");

        //deactivate Handmirror
        ActivateAnotherObject.Disable(handMirror);

        playerNma.enabled = false;

        CameraFollower cf = CharGameController.getCamera().GetComponent<CameraFollower>();
        cf.updateTarget();
        cf.fixRelativeToDefault();        
        
        //Set model of kovalev as with pijamas assuming kovalev is 1st element and kovalevwithpijamas is 2nd     
        player.transform.GetChild(1).gameObject.SetActive(false);
        player.transform.GetChild(2).gameObject.SetActive(true);

        //Move player to starting position
        CharGameController.movePlayer(StartingPoint.transform.position);
        player.transform.rotation = StartingPoint.transform.rotation;

        if (!playerAnim)
        {
            
            yield return 0;
            
        }

        Debug.Log("Lie animation");

        playerAnim.SetTrigger("Lie");



        pcc.StopToWalk();

        player.GetComponent<CharacterController>().enabled = false;

        yield return Timing.WaitForSeconds(2f);

        //BasicCharAnimations bca = player.GetComponent<BasicCharAnimations>();
        //bca.enabled = false;

        playerAnim.SetTrigger("GetUp");



        //yield return 0;

        //UnityEditor.EditorApplication.isPaused = true;


        //yield return 0;

        yield return Timing.WaitForSeconds(3f);


        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs._Tween(player, player.transform.position - player.transform.right * 1f - player.transform.up * 1.7f, 1.3f));
        yield return Timing.WaitUntilDone(handler);

        //player.transform.position = player.transform.position - player.transform.right * 1f - player.transform.up * 1.7f;



        //Destroy(KovAgent);
        Vector3 posOnNavMesh;
        if (Vckrs.findNearestPositionOnNavMesh(player.transform.position, playerNma.areaMask, 2f, out posOnNavMesh))
        {
            player.transform.position = posOnNavMesh;
        }else
        {
            Debug.Log("Coudn't found position on navmesh");
        }
        //yield return 0;
        //KovAgent = player.AddComponent<NavMeshAgent>();

        yield return Timing.WaitForSeconds(0.5f);

        playerNma.enabled = true;

        pcc.ContinueToWalk();
        

    }



    public void callIvan()
    {
        Timing.RunCoroutine(_callIvan());
    }

    public IEnumerator<float> _callIvan()
    {
        subtitle.text = "";
        pcc.StopToWalk();
        print("callIvan");
        IEnumerator<float> handler;
        //charText.text = "-Kovalev: Aman Tanrım!";

        //Timing.WaitForSeconds(2f);

        sc.callSubtitleWithIndex(0);

        while (subtitle.text != "")
        {
            //print(charText.text);
            yield return 0;
        }

        Ivan.SetActive(true);
        IvanAgent.SetDestination(player.transform.position + Vector3.forward * 2);
        
        handler = Timing.RunCoroutine(Vckrs.waitUntilStop(Ivan, 0.005f));

        yield return Timing.WaitUntilDone(handler);

        IvanAlt.enabled = true;

        Timing.RunCoroutine(Vckrs._lookTo(player, Ivan.transform.position - player.transform.position, 1f));

        sc.callSubtitleWithIndex(1);
        while (subtitle.text != "")
        {
            //print(charText.text);
            yield return 0;
        }

        playerNma.speed = 1.5f;
        handler = Timing.RunCoroutine(Vckrs._pace(player, player.transform.position, player.transform.position+7*Vector3.right));
        playerNma.speed = 3f;

        sc.callSubtitleWithIndex(2);
        while (subtitle.text != "")
        {
            yield return 0;
        }

        Timing.KillCoroutines(handler);
        playerNma.Stop();

        sc.callSubtitleWithIndex(3);
        while (subtitle.text != "")
        {
            yield return 0;
        }

        IvanAgent.SetDestination(Door.transform.position);
        Vckrs.ActivateAnotherObject(Dolap);
        IvanAlt.enabled = false;


        handler = Timing.RunCoroutine(Vckrs.waitUntilStop(Ivan, 0.005f));
        yield return Timing.WaitUntilDone(handler);

        Ivan.SetActive(false);

        pcc.ContinueToWalk();


    }

    public void callIvanComesWithPaper()
    {
        Timing.RunCoroutine(_IvanComesWithPaper());
    }

    public IEnumerator<float> _IvanComesWithPaper()
    {

        Debug.Log("Ivan comes with paper");
        IEnumerator<float> handler;

        Dolap.tag = "Untagged";
        //Vckrs.DisableAnotherObject(Dolap);

        Ivan.SetActive(true);
        Paper.SetActive(true);
        IvanAgent.SetDestination(player.transform.position + Vector3.forward * 3);

        handler = Timing.RunCoroutine(Vckrs.waitUntilStop(Ivan,0.0005f));
        yield return Timing.WaitUntilDone(handler);

        IvanAlt.enabled = true;
        Vckrs.ActivateAnotherObject(Ivan);
        
    }

    public void KovalevPickedPaper()
    {
        if (GlobalController.isScnListContains(GlobalController.Scenes.Church)) return;

        Ivan.transform.tag = "Untagged";

        //Vckrs.DisableAnotherObject(Ivan);
        Ivan.GetComponent<BasicCharAnimations>().enabled = true;
        IvanAlt.enabled = true;

        Paper.SetActive(false);
        CharGameController.getObjectOfHand("paper", CharGameController.hand.RightHand).SetActive(true);
        playerAnim.SetBool("RightHandAtFace", true);
        Vckrs.ActivateAnotherObject(Door);
        OpenDoorLoad od = Door.GetComponent<OpenDoorLoad>();
        od.otherCanOpen = false;
        od.Unlock();

        pcc.StopToWalk();
        sc.callSubtitleWithIndex(21);



        //ksc.disableWC();


    }

    //The functions are called in scene that officer brings kovalev nose to home.

    IEnumerator<float> noseIsFound()
    {
        //Wait for one frame to initilizing
        yield return 0;

        sc.startAutomatic = true;

        updateCharacterVariables();

        //Stop player
        pcc.StopToWalk();

        //Enable hand mirror
        handMirror.SetActive(true);

        //Lock cafe chair
        tableChair.GetComponent<WalkLookAnim>().lockSit = true;

        //CharGameController.movePlayer(armChair.transform.position);


        //Make kovalev sit to chair
        
        WalkLookAnim wla = armChair.GetComponent<WalkLookAnim>();

        playerAnim.speed = 1000;
        wla.lockSit = true;
        Timing.WaitUntilDone(Timing.RunCoroutine(wla._sit(true)));

        playerAnim.speed = 1;

        sc.callSubtitleWithIndex(4);
        while (getSubt().text != "") yield return 0;

        //TODO Slap game

        

        //Wait for the player activate hand mirror
        while (!handMirrorBig.activeSelf) yield return 0;

        yield return Timing.WaitForSeconds(5f);

        //Deactive big mirror
        handMirrorBig.SetActive(false);

        sc.callSubtitleWithIndex(5);
        while (getSubt().text != "") yield return 0;

        sc.callSubtitleWithIndexTime(0);
        while (getNarSubt().text != "") yield return 0;

        //Player get up
        wla.getUp();
        wla.lockSit = false;

        WalkLookAnim tcWLA = tableChair.GetComponent<WalkLookAnim>();

        //Wait kovalev for sit 
        while (!tcWLA.isSitting()) yield return 0;

        letterToSend.SetActive(true);


        yield break;
    }

    public void finishedWriting()
    {
        Timing.RunCoroutine(_finishedWriting());
    }

    IEnumerator<float> _finishedWriting()
    {
        //Paper.SetActive(true);

        letterToSend.SetActive(false);

        sc.callSubtitleWithIndex(7);
        while (getSubt().text != "") yield return 0;

        yield return Timing.WaitForSeconds(3f);

        Ivan.transform.LookAt(player.transform.position);
        IvanAlt.enabled = transform;        
        Ivan.transform.tag = "ActiveObject";
        

        //call ivan
        Ivan.SetActive(true);
        //Ivan position is back of chair
        IvanAgent.SetDestination(tableChair.transform.position + Vector3.right * 5);

        sc.callSubtitleWithIndex(8);
        while (getSubt().text != "") yield return 0;

        WalkLookAnim tcWLA = tableChair.GetComponent<WalkLookAnim>();
        tcWLA.getUp();

        yield break;
    }

    public void giveLetterToIvan()
    {
        Timing.RunCoroutine(_giveLetterToIvan());
    }

    IEnumerator<float> _giveLetterToIvan()
    {
        //Paper.SetActive(false);

        //Check is in right point in story
        //if (!GlobalController.isScnListContains(GlobalController.Scenes.Church)) yield break;

        sc.callSubtitleWithIndex(9);
        while (getSubt().text != "") yield return 0;


        IvanAlt.enabled = false;

        IvanAgent.SetDestination(Door.transform.position);
   

        handlerHolder =  Timing.RunCoroutine(Vckrs.waitUntilStop(Ivan));
        yield return Timing.WaitUntilDone(handlerHolder);

        Ivan.SetActive(false);

        Door.GetComponent<OpenDoorLoad>().closeDoor();

        blackScreen.script.fadeOut();

        yield return Timing.WaitForSeconds(1.5f);

        //Pace while wating ivan
        handlerHolder= Timing.RunCoroutine(Vckrs._pace(player, player.transform.position, player.transform.position - Vector3.forward * 5));

        yield return Timing.WaitForSeconds(1.5f);

        blackScreen.script.fadeIn();

        yield return Timing.WaitForSeconds(5f);
        
        Ivan.SetActive(true);
        Ivan.tag = "Untagged";

        IvanAgent.SetDestination(Vector3.Lerp(Ivan.transform.position, player.transform.position, 0.7f));


        Timing.KillCoroutines(handlerHolder);
        Timing.RunCoroutine(Vckrs._lookTo(player, Ivan.transform.position, 1f));
        
        sc.callSubtitleWithIndex(10);

        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Ivan));
        yield return Timing.WaitUntilDone(handlerHolder);

        while (subtitle.text != "") yield return 0;

        letterRecieved.SetActive(true);

        
    }

    public void finishedRecievedLetter()
    {
        Timing.RunCoroutine(_finishedRecievedLetter());
    }

    IEnumerator<float> _finishedRecievedLetter()
    {
        letterRecieved.SetActive(false);

        //Wait for some time
        yield return Timing.WaitForSeconds(3f);

        sc.callSubtitleWithIndex(11);
        while (subtitle.text != "") yield return 0;

        //Rang door

        AudioSource source = GetComponent<AudioSource>();
        source.Play();

        //while (source.isPlaying) yield return 0;

        yield return Timing.WaitForSeconds(3f);


        //Door is rang, Ivan goes to door
        handlerHolder = Timing.RunCoroutine(Vckrs._setDestination(Ivan, Door.transform.position));
        yield return Timing.WaitUntilDone(handlerHolder);

        Ivan.SetActive(false);

        yield return Timing.WaitForSeconds(3f);

        Ivan.transform.LookAt(player.transform);
        Ivan.SetActive(true);

        sc.callSubtitleWithIndex(12);
        while (subtitle.text != "") yield return 0;

        yield return Timing.WaitUntilDone(Timing.RunCoroutine(Vckrs._lookTo(Ivan, Door, 1f)));

        Ivan.SetActive(false);

        yield return Timing.WaitForSeconds(3f);

        //Police comes in 
        police.SetActive(true);

        Timing.RunCoroutine(Vckrs._lookTo(player, police, 1f));

        handlerHolder = Timing.RunCoroutine(Vckrs._setDestination(police, player.transform.position+Vector3.forward*3));
        yield return Timing.WaitUntilDone(handlerHolder);

        sc.callSubtitleWithIndex(13);
        while (subtitle.text != "") yield return 0;

        //swap

        policeNose.SetActive(false);
        CharGameController.getHand(CharGameController.hand.RightHand).transform.GetChild(1).gameObject.SetActive(true);

        sc.callSubtitleWithIndex(14);
        while (subtitle.text != "") yield return 0;

        handlerHolder = Timing.RunCoroutine(Vckrs._setDestination(police, Door.transform.position ));
        yield return Timing.WaitUntilDone(handlerHolder);

        police.SetActive(false);

        sc.callSubtitleWithIndex(15);
        while (subtitle.text != "") yield return 0;

        sc.callSubtitleWithIndexTime(1);
        while (narSubtitle.text != "") yield return 0;

        sc.callSubtitleWithIndex(17);
        while (subtitle.text != "") yield return 0;

        headAttachNose.SetActive(true);

        yield break;
    }

    public void finishAttachGame()
    {
        Timing.RunCoroutine(_finishAttachGame());
    }

    IEnumerator<float> _finishAttachGame()
    {
        headAttachNose.SetActive(false);
        
        sc.callSubtitleWithIndex(18);
        while (subtitle.text != "") yield return 0;

        Ivan.transform.LookAt(player.transform.position);
        Ivan.SetActive(true);
        IvanAgent.SetDestination(Vector3.Lerp(Ivan.transform.position, player.transform.position, 0.7f));
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Ivan));
        yield return Timing.WaitUntilDone(handlerHolder);

        sc.callSubtitleWithIndex(19);
        while (subtitle.text != "") yield return 0;

        Door.GetComponent<OpenDoorLoad>().Unlock();
        yield break;
    }


    IEnumerator<float> comingFromSculpturer()
    {
        yield return 0;
        Debug.Log("sCLUPTUREEEEEEER");


        playerNma.enabled = false;

        //Move player to starting position
        CharGameController.movePlayer(StartingPoint.transform.position);
        player.transform.rotation = StartingPoint.transform.rotation;

        //if (!playerAnim) yield return 0;

        playerAnim.SetTrigger("Lie");

        pcc.StopToWalk();

        //MAKE DAY
        CharGameController.getSun().GetComponent<DayAndNightCycle>().makeDay();

        yield return Timing.WaitForSeconds(2f);
        //player.GetComponent<CharacterController>().enabled = false;


        playerAnim.SetTrigger("GetUp");

        yield return Timing.WaitForSeconds(2f);


        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs._Tween(player, player.transform.position - player.transform.right * 1f - player.transform.up * 1.7f, 1.3f));
        yield return Timing.WaitUntilDone(handler);


        //Destroy(KovAgent);
        Vector3 posOnNavMesh;
        if (Vckrs.findNearestPositionOnNavMesh(player.transform.position, playerNma.areaMask, 2f, out posOnNavMesh))
        {
            player.transform.position = posOnNavMesh;
        }
        else
        {
            Debug.Log("Coudn't found position on navmesh");
        }
        //yield return 0;
        //KovAgent = player.AddComponent<NavMeshAgent>();

        playerNma.enabled = true;

        yield return Timing.WaitForSeconds(0.5f);

        sc.callSubtitleWithIndex(20);

        playerNma.enabled = true;

        Door.GetComponent<OpenDoorLoad>().Unlock();
        
        yield break;
    }

    public void ivanAction()
    {
        if (khs == kovalevHomeScene.Church)
        {
            giveLetterToIvan();
       
        }
        else if(khs==kovalevHomeScene.KovalevLoosesHisNose)
        {
            KovalevPickedPaper();
        }
    }

}
