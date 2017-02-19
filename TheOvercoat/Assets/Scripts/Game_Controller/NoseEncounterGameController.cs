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


public class NoseEncounterGameController : GameController {
    public GameObject Horse, Nose, newspaper, sun, Girlgame/*, Obstacles, HorseAimOne, SubHolder,  Girlgame, LightObj, HorseAimTwo*/;

    HorseScript hs;
    characterComponents noseCC;


    public float walkTolerance = 0.01f;
    EnterTrigger NoseEt;
    RunRandomlyFromObject rrfo;
    GameObject girl;
    GameObject girlCanvas;
    MountCarier mc;




    //public GameObject IvanSubtitleHolder;
    //SubtitleCaller ivanSc;

    // Use this for initialization
    public override void Start () {
        base.Start();

        hs = Horse.GetComponent<HorseScript>();
        noseCC = new characterComponents(Nose);

        rrfo = Nose.GetComponent<RunRandomlyFromObject>();

        girl = Girlgame.transform.GetChild(0).gameObject;
        girlCanvas = Girlgame.transform.GetChild(1).gameObject;
        //mc = Nose.GetComponent<MountCarier>();


        //startNoseGame();
        //noseCatched();
    }




    // Update is called once per frame
    void Update () {
              

        //if (rrfo.catched)
        //{
        //    print("chatched game started");
        //}

	}

    public void startNoseGame()
    {     
        Timing.RunCoroutine(_start());
    }


    IEnumerator<float> _start()
    {

        //hs.release();
        handlerHolder= hs.setDes(player.transform.position + Vector3.forward * 10);
        pcc.StopToWalk();

        
        SubtitleFade.subtitles["CharacterSubtitle"].text= "Kovalev: Nasıll!?!?";

        yield return Timing.WaitUntilDone(handlerHolder);

        hs.unmount();

        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Nose));
        yield return Timing.WaitUntilDone(handlerHolder);



        Vector3 pos = Vckrs.generateRandomPositionOnCircle(Nose.transform.position, 10f);
        NavMeshHit nmh;
        if((NavMesh.SamplePosition(pos, out nmh, 15f, noseCC.navmashagent.areaMask))){

            noseCC.navmashagent.SetDestination(nmh.position);
        }else
        {
            Debug.Log("Couldnt found");
            noseCC.navmashagent.SetDestination(pos);
        }

   
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Nose));
        yield return Timing.WaitUntilDone(handlerHolder);

        newspaper.SetActive(true);
        Nose.transform.parent = transform;



        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "") yield return 0;

        yield return Timing.WaitForSeconds(3f);

        sc.callSubtitleWithIndex(1);
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

 

    }



    public void noseCatched()
    {
        Timing.RunCoroutine(_noseCatched());
    }

    IEnumerator<float> _noseCatched()
    {
        pcc.StopToWalk();
        sc.callSubtitleWithIndex(2);
        while (subtitle.text != "")
        {
            yield return 0;
        }


        girl.SetActive(true);
        NavMeshAgent nmaGirls = girl.GetComponent<NavMeshAgent>();
        nmaGirls.Resume();
        nmaGirls.SetDestination(player.transform.position + player.transform.forward * 5);

        DayAndNightCycle danc = sun.GetComponent<DayAndNightCycle>();
        danc.makeNight();

        //IEnumerator<float> lightHnadler = Timing.RunCoroutine(Vckrs._setLightIntensity(LightObj, 0.5f, 0));
        IEnumerator<float> girlWalkHandler = Timing.RunCoroutine(Vckrs.waitUntilStop(girl, 0f));

        yield return Timing.WaitUntilDone(girlWalkHandler);

        Timing.RunCoroutine(Vckrs._lookTo(player, girl.transform.position - player.transform.position, 2f));

        girlCanvas.SetActive(true);
        yield return Timing.WaitForSeconds(0.5f);

        GameObject girlGameKov = girlCanvas.transform.GetChild(0).GetChild(0).gameObject;

        MovementWithKeyboard2D mwk2 = girlGameKov.GetComponent<MovementWithKeyboard2D>();
        mwk2.speed = 0.05f;
        mwk2.scriptInput = -1;
        yield return Timing.WaitForSeconds(4f);
        mwk2.scriptInput = -0;
        mwk2.speed = 0.1f;

        CharacterController gkcc = girlGameKov.GetComponent<CharacterController>();
        gkcc.enabled = false;
        gkcc.enabled = true;

        GirlGameController ggc = girlCanvas.transform.GetChild(0).GetComponent<GirlGameController>();
        ggc.enabled = true;

        yield return Timing.WaitForSeconds(1f);

        IEnumerator<float> handler = hs.mount();

        yield return Timing.WaitUntilDone(handler);

        hs.setDes(Horse.transform.position + Horse.transform.forward * -40f);

        yield return Timing.WaitForSeconds(5f);
        Horse.transform.parent.gameObject.SetActive(false);

    }

    public void noseGoneLost()
    {
        Timing.RunCoroutine(_noseGoneLost());
    }

    IEnumerator<float> _noseGoneLost()
    {
        sc.callSubtitleWithIndex(3);
        while (subtitle.text != "")
        {
            yield return 0;
        }


    }




}
