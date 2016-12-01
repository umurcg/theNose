using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class KovalevHomeGameController : MonoBehaviour {
    public GameObject Kovalev, CharSubt, Door, Ivan, Dolap, Paper, DoorFrame, HandR;
    

    SubtitleCaller sc;
    PlayerComponentController pcc;
    Animator KovAnimator;
   
    NavMeshAgent KovAgent;
    Text charText;
    NavMeshAgent IvanAgent;
    AlwaysLookTo IvanAlt;

    KeySlideCompletely ksc;

    // Use this for initialization
    void Start()
    {
        //CharGameController.setCharacter("Nose");

        pcc = Kovalev.GetComponent<PlayerComponentController>();
        KovAnimator = Kovalev.GetComponent<Animator>();
        KovAgent = Kovalev.GetComponent<NavMeshAgent>();
        charText = CharSubt.GetComponent<Text>();
        sc = GetComponent<SubtitleCaller>();
        IvanAgent = Ivan.GetComponent<NavMeshAgent>();
        IvanAlt = Ivan.GetComponent<AlwaysLookTo>();
        ksc = Door.GetComponent<KeySlideCompletely>();

        callWakeUp();

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

        KovAnimator.SetTrigger("Lie");
   
        PlayerComponentController pcc = Kovalev.GetComponent<PlayerComponentController>();
        pcc.StopToWalk();



        KovAnimator.SetTrigger("GetUp");

        yield return Timing.WaitForSeconds(2f);

        IEnumerator<float> handler =Timing.RunCoroutine(Vckrs._Tween(Kovalev, Kovalev.transform.position - Kovalev.transform.right * 1.5f, 0.5f));
        yield return Timing.WaitUntilDone(handler);

        NavMeshAgent KovAgent = Kovalev.GetComponent<NavMeshAgent>();
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
        IvanAgent.SetDestination(Kovalev.transform.position + Vector3.forward * 5);
        
        handler = Timing.RunCoroutine(Vckrs.waitUntilStop(Ivan, 0.005f));

        yield return Timing.WaitUntilDone(handler);

        IvanAlt.enabled = true;

        sc.callSubtitleWithIndex(1);
        while (charText.text != "")
        {
            //print(charText.text);
            yield return 0;
        }

        KovAgent.speed = 1.5f;
        handler = Timing.RunCoroutine(Vckrs._pace(Kovalev, Kovalev.transform.position, Kovalev.transform.position+3*Vector3.right));
        KovAgent.speed = 3f;

        sc.callSubtitleWithIndex(2);
        while (charText.text != "")
        {
            yield return 0;
        }

        Timing.KillCoroutines(handler);
        KovAgent.Stop();

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
        IEnumerator<float> handler;

        Vckrs.DisableAnotherObject(Dolap);

        Ivan.SetActive(true);
        Paper.SetActive(true);
        IvanAgent.SetDestination(Kovalev.transform.position + Vector3.forward * 3);

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
        KovAnimator.SetBool("RightHandAtFace", true);
        Vckrs.ActivateAnotherObject(Door);
        DoorFrame.GetComponent<ChangeSceneFromDoor>().enabled = true;

        ksc.disableWC();


    }

}
