using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KovalevHomeGameController : GameController {
    public GameObject CharSubt, Door, Ivan, Dolap, Paper, HandR, StartingPoint;
    

   

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

            if (!GlobalController.Instance.sceneList.Contains(SceneManager.GetActiveScene().buildIndex))
            {
                Debug.Log("calling wake up");
                callWakeUp();
                return;
            }
            Debug.Log("Build index in sceneList");
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void callWakeUp()
    {
        Timing.RunCoroutine(_WakeUp());
    }

    IEnumerator<float> _WakeUp()
    {
        Debug.Log("wake up");

        //Set player as kovalev
        GameObject character=CharGameController.setCharacter("Kovalev");
        updateCharacterVariables();
        
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

}
