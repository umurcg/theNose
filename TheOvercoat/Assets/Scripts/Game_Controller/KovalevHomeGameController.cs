using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KovalevHomeGameController : GameController {
    public GameObject CharSubt, Door, Ivan, Dolap, Paper, HandR, StartingPoint;

    public GameObject armChair, handMirror,  handMirrorBig, letterToSend, letterRecieved ,tableChair, police, headAttachNose;
   

    Text charText;
    NavMeshAgent IvanAgent;
    AlwaysLookTo IvanAlt;
    KeySlideCompletely ksc;


    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
                      
        charText = CharSubt.GetComponent<Text>();
        sc = GetComponent<SubtitleCaller>();
        IvanAgent = Ivan.GetComponent<NavMeshAgent>();
        IvanAlt = Ivan.GetComponent<AlwaysLookTo>();

        
        if (GlobalController.Instance == null)
        {

            callWakeUp();
        }
        else
        {
            //If user coming for first time and didn't go church 
            if (!GlobalController.isScnListContains(GlobalController.Scenes.KovalevHouse)
                && !GlobalController.isScnListContains(GlobalController.Scenes.Church)
                )
            {
                //Debug.Log("calling wake up");
                callWakeUp();
                return;
            } //If previous 2nd scene is church
            else if (GlobalController.Instance.sceneList.Count>=2 &&  GlobalController.Instance.sceneList[GlobalController.Instance.sceneList.Count - 2] == (int)GlobalController.Scenes.Church)
            {
                //Debug.Log("Nose is FOUUUUND!");
                Timing.RunCoroutine(noseIsFound());
                //Timing.RunCoroutine(_finishedRecievedLetter());
            }
           
        }
    }

    public override void Start()
    {
        base.Start();

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
        //wait one frame
        yield return 0;

        Debug.Log("wake up");

        //Set player as kovalev
        GameObject character=CharGameController.setCharacter("Kovalev");
        updateCharacterVariables();

        CameraFollower cf = CharGameController.getCamera().GetComponent<CameraFollower>();
        cf.updateTarget();
        cf.fixRelativeToDefault();        
        
        //Set model of kovalev as with pijamas assuming kovalev is 1st element and kovalevwithpijamas is 2nd     
        player.transform.GetChild(1).gameObject.SetActive(false);
        player.transform.GetChild(2).gameObject.SetActive(true);

        //Move player to starting position
        CharGameController.movePlayer(StartingPoint.transform.position);

        if (!playerAnim) yield return 0;

        playerAnim.SetTrigger("Lie");
   
        PlayerComponentController pcc = player.GetComponent<PlayerComponentController>();
        if(pcc)
        pcc.StopToWalk();

        player.GetComponent<CharacterController>().enabled = false;


        playerAnim.SetTrigger("GetUp");

        yield return Timing.WaitForSeconds(2f);

        IEnumerator<float> handler =Timing.RunCoroutine(Vckrs._Tween(player, player.transform.position - player.transform.right * 1f-player.transform.up*0.5f, 0.5f));
        yield return Timing.WaitUntilDone(handler);

        NavMeshAgent KovAgent = player.GetComponent<NavMeshAgent>();
        KovAgent.enabled = true;
        pcc.ContinueToWalk();
    }

    public void callIvan()
    {
        Timing.RunCoroutine(_callIvan());
    }

    public IEnumerator<float> _callIvan()
    {
        print("callIvan");
        IEnumerator<float> handler;
        //charText.text = "-Kovalev: Aman Tanrım!";

        //Timing.WaitForSeconds(2f);

        sc.callSubtitleWithIndex(0);

        while (charText.text != "")
        {
            //print(charText.text);
            yield return 0;
        }

        Ivan.SetActive(true);
        IvanAgent.SetDestination(player.transform.position + Vector3.forward * 5);
        
        handler = Timing.RunCoroutine(Vckrs.waitUntilStop(Ivan, 0.005f));

        yield return Timing.WaitUntilDone(handler);

        IvanAlt.enabled = true;

        Timing.RunCoroutine(Vckrs._lookTo(player, Ivan.transform.position - player.transform.position, 1f));

        sc.callSubtitleWithIndex(1);
        while (charText.text != "")
        {
            //print(charText.text);
            yield return 0;
        }

        playerNma.speed = 1.5f;
        handler = Timing.RunCoroutine(Vckrs._pace(player, player.transform.position, player.transform.position+7*Vector3.right));
        playerNma.speed = 3f;

        sc.callSubtitleWithIndex(2);
        while (charText.text != "")
        {
            yield return 0;
        }

        Timing.KillCoroutines(handler);
        playerNma.Stop();

        sc.callSubtitleWithIndex(3);
        while (charText.text != "")
        {
            yield return 0;
        }

        IvanAgent.SetDestination(Door.transform.position);
        Vckrs.ActivateAnotherObject(Dolap);
        IvanAlt.enabled = false;


        handler = Timing.RunCoroutine(Vckrs.waitUntilStop(Ivan, 0.005f));
        yield return Timing.WaitUntilDone(handler);

        Ivan.SetActive(false);


    }

    public void callIvanComesWithPaper()
    {
        Timing.RunCoroutine(_IvanComesWithPaper());
    }

    public IEnumerator<float> _IvanComesWithPaper()
    {

        Debug.Log("Ivan comes with paper");
        IEnumerator<float> handler;

        Vckrs.DisableAnotherObject(Dolap);

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

        Vckrs.DisableAnotherObject(Ivan);
        Ivan.GetComponent<BasicCharAnimations>().enabled = true;
        IvanAlt.enabled = true;

        Paper.transform.SetParent(HandR.transform);
        Paper.transform.localPosition=new Vector3(-0.475f, 0.008f, -0.044f);
        playerAnim.SetBool("RightHandAtFace", true);
        Vckrs.ActivateAnotherObject(Door);
        OpenDoorLoad od = Door.GetComponent<OpenDoorLoad>();
        od.otherCanOpen = false;
        od.playerCanOpen = true;



        //ksc.disableWC();


    }

    //The functions are called in scene that officer brings kovalev nose to home.

    IEnumerator<float> noseIsFound()
    {
        //Wait for one frame to initilizing
        yield return 0;

        updateCharacterVariables();

        //Stop player
        pcc.StopToWalk();

        //Enable hand mirror
        handMirror.SetActive(true);

        //Make kovalev sit to chair
        WalkLookAnim wla = armChair.GetComponent<WalkLookAnim>();
        wla.lockSit = true;
        Timing.RunCoroutine(wla._sit(true));

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

        WalkLookAnim tcWLA = tableChair.GetComponent<WalkLookAnim>();

        //Wait kovalet for sit 
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
        Paper.SetActive(true);

        letterToSend.SetActive(false);

        sc.callSubtitleWithIndex(7);
        while (getSubt().text != "") yield return 0;

        yield return Timing.WaitForSeconds(3f);

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
        Paper.SetActive(false);

        //Check is in right point in story
        if (!GlobalController.isScnListContains(GlobalController.Scenes.Church)) yield break;

        sc.callSubtitleWithIndex(9);
        while (getSubt().text != "") yield return 0;


        IvanAlt.enabled = false;

        IvanAgent.SetDestination(Door.transform.position);

        handlerHolder =  Timing.RunCoroutine(Vckrs.waitUntilStop(Ivan));
        yield return Timing.WaitUntilDone(handlerHolder);

        Ivan.SetActive(false);

        blackScreen.script.fadeOut();

        yield return Timing.WaitForSeconds(1.5f);

        //Pace while wating ivan
        handlerHolder= Timing.RunCoroutine(Vckrs._pace(player, player.transform.position, player.transform.position - Vector3.forward * 5));

        yield return Timing.WaitForSeconds(1.5f);

        blackScreen.script.fadeIn();

        yield return Timing.WaitForSeconds(5f);
        
        Ivan.SetActive(true);
        IvanAgent.SetDestination(Vector3.Lerp(Ivan.transform.position, player.transform.position, 0.7f));

        Timing.KillCoroutines(handlerHolder);

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

        //Door is rang, Ivan goes to door
        handlerHolder = Timing.RunCoroutine(Vckrs._setDestination(Ivan, Door.transform.position));
        yield return Timing.WaitUntilDone(handlerHolder);

        Ivan.SetActive(false);

        yield return Timing.WaitForSeconds(3f);

        Ivan.SetActive(true);

        sc.callSubtitleWithIndex(12);
        while (subtitle.text != "") yield return 0;

        //Police comes in 
        police.SetActive(true);
        Ivan.SetActive(false);

        handlerHolder = Timing.RunCoroutine(Vckrs._setDestination(police, player.transform.position+Vector3.forward*3));
        yield return Timing.WaitUntilDone(handlerHolder);

        sc.callSubtitleWithIndex(13);
        while (subtitle.text != "") yield return 0;

        handlerHolder = Timing.RunCoroutine(Vckrs._setDestination(police, Door.transform.position ));
        yield return Timing.WaitUntilDone(handlerHolder);

        police.SetActive(false);

        sc.callSubtitleWithIndex(14);
        while (subtitle.text != "") yield return 0;

        sc.callSubtitleWithIndexTime(1);
        while (narSubtitle.text != "") yield return 0;

        sc.callSubtitleWithIndex(16);
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
        
        sc.callSubtitleWithIndex(17);
        while (subtitle.text != "") yield return 0;

        Ivan.SetActive(true);
        IvanAgent.SetDestination(Vector3.Lerp(Ivan.transform.position, player.transform.position, 0.7f));
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Ivan));
        yield return Timing.WaitUntilDone(handlerHolder);

        sc.callSubtitleWithIndex(18);
        while (subtitle.text != "") yield return 0;

        Door.GetComponent<OpenDoorLoad>().playerCanOpen = true;

        yield break;
    }
}
