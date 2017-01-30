using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;
//using CinemaDirector;

//public GameObject HorseObject;
//public GameObject Kovalev;
//public GameObject Nose;
//public GameObject CSub;
//public GameObject NSub;
//public GameObject Obstacles;


public class NoseEncounterGameController : MonoBehaviour {
    public GameObject Horse, Player, Nose, CSub, NSub, Obstacles, HorseAimOne, SubHolder, newspaper, Girlgame, LightObj, HorseAimTwo;
    
    public float walkTolerance = 0.01f;
    Text CharSubt, NarSub;
    EnterTrigger NoseEt;
    RunRandomlyFromObject rrfo;
    SubtitleCaller sc;
    GameObject girl;
    GameObject girlCanvas;
    PlayerComponentController pcc;
    MountCarier mc;



    //public GameObject IvanSubtitleHolder;
    //SubtitleCaller ivanSc;

    // Use this for initialization
    void Start () {
        Player = CharGameController.getActiveCharacter();

        if (Player == null)
        {
            gameObject.SetActive(false);
            this.enabled = false;
            return;
        }

        if (Player.name != "Kovalev")
        {
            gameObject.SetActive(false);
            this.enabled = false;
            
            return;
        }

        CharSubt = CSub.GetComponent<Text>();
        NarSub = NSub.GetComponent<Text>();
        rrfo= Nose.GetComponent<RunRandomlyFromObject>();
        sc = SubHolder.GetComponent<SubtitleCaller>();
        girl = Girlgame.transform.GetChild(0).gameObject;
        girlCanvas = Girlgame.transform.GetChild(1).gameObject;
        pcc = Player.GetComponent<PlayerComponentController>();
        mc = Nose.GetComponent<MountCarier>();

        //Timing.RunCoroutine(_noseCatched());
        //noseEnter();
    }




    // Update is called once per frame
    void Update () {

        

        if (rrfo.catched)
        {
            print("chatched game started");
        }

	}



    //Kovalev first city scene
    public IEnumerator<float> _noseCatched()
    {
        sc.callSubtitle();
        while (CharSubt.text!="")
        {
            yield return 0;
        }

        pcc.StopToWalk();
        girl.SetActive(true);
        NavMeshAgent nmaGirls = girl.GetComponent<NavMeshAgent>();
        nmaGirls.SetDestination(girl.transform.position + girl.transform.forward * 20);
       
        
        //IEnumerator<float> lightHnadler= Timing.RunCoroutine(Vckrs._setLightIntensity(LightObj, 0.5f, 0));
        IEnumerator<float> girlWalkHandler = Timing.RunCoroutine(Vckrs.waitUntilStop(girl, 0f));

        yield return Timing.WaitUntilDone(girlWalkHandler);

        Timing.RunCoroutine(Vckrs._lookTo(Player,girl.transform.position-Player.transform.position, 2f));

        girlCanvas.SetActive(true);
        yield return Timing.WaitForSeconds(0.5f);

        GameObject girlGameKov = girlCanvas.transform.GetChild(0).GetChild(0).gameObject;

        MovementWithKeyboard2D mwk2 = girlGameKov.GetComponent<MovementWithKeyboard2D>();
        mwk2.speed = 0.05f;
        mwk2.scriptInput = -1;
        yield return Timing.WaitForSeconds(4f);
        mwk2.scriptInput = -0;
        mwk2.speed = 0.1f;

        CharacterController gkcc= girlGameKov.GetComponent<CharacterController>();
        gkcc.enabled = false;
        gkcc.enabled = true;

        GirlGameController ggc = girlCanvas.transform.GetChild(0).GetComponent<GirlGameController>();
        ggc.enabled = true;

        yield return Timing.WaitForSeconds(1f);

        IEnumerator<float> handler= Timing.RunCoroutine(mc._mount());

        yield return Timing.WaitUntilDone(handler);

        HorseFreeze hf = Horse.GetComponent<HorseFreeze>();
        hf.release();


        NavMeshAgent nma = Horse.GetComponent<NavMeshAgent>();
        nma.SetDestination(HorseAimTwo.transform.position);

        yield return Timing.WaitForSeconds(5f);
        Horse.transform.parent.gameObject.SetActive(false);

    }


    public IEnumerator<float> _noseGoneLost()
    {
        sc.callSubtitle();
        while (CharSubt.text != "")
        {
            yield return 0;
        }


    }

    public void noseEnter()
    {
        GetComponent<EnterTrigger>().enabled = false;
        Timing.RunCoroutine(_noseEnter());
    }

    public IEnumerator<float> _noseEnter()
    {
       
        HorseFreeze hf = Horse.GetComponent<HorseFreeze>();
        hf.release();


        pcc.StopToWalk();

       
        IEnumerator<float> mountHandler = Timing.RunCoroutine(mc._mount());

        yield return Timing.WaitUntilDone(mountHandler);

        //print("Done");

        //yield return Timing.WaitForSeconds(1000000000000000f);

        NavMeshAgent nma = Horse.GetComponent<NavMeshAgent>();
        nma.SetDestination(HorseAimOne.transform.position);

        CharSubt.text = "Kovalev: Nasıll!?!?";

        Vector3 horsePosition = Horse.transform.position;
        yield return Timing.WaitForSeconds(0.5f);
        while (Vector3.Distance(horsePosition, Horse.transform.position) > walkTolerance)
        {
            horsePosition = Horse.transform.position;
            yield return 0;
        }
        hf.freeze();
        yield return Timing.WaitForSeconds(0.5f);

        IEnumerator<float> unmountHandler =Timing.RunCoroutine(mc._unmount());
        yield return Timing.WaitUntilDone(unmountHandler);


        yield return Timing.WaitForSeconds(0.5f);


        //pace
      
        sc.callSubtitle();

        NavMeshAgent nmaNose = Nose.GetComponent<NavMeshAgent>();
        nmaNose.enabled = true;
       
        rrfo.enabled = true;
        nmaNose.Resume();
        nmaNose.speed = 3;
        Vector3 aim =Nose.transform.position+ Nose.transform.forward*4;

        //print(aim);
        nmaNose.SetDestination(aim);


        newspaper.SetActive(true);


        while (CharSubt.text != "")
        {
            yield return 0;
        }

        yield return Timing.WaitForSeconds(8f);

        sc.callSubtitle();
        while (CharSubt.text != "")
        {
            yield return 0;
        }

        //pace finish;
    

        yield return Timing.WaitForSeconds(8f);

        sc.callSubtitleTime();
   
    }
}
