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

//TODO kovaev position when canvas is activated

public class NoseEncounterGameController : GameController {
    public GameObject Horse, Nose, trash, newspaper, sun, Girlgame, girl,girlCanvas, girlGameKov/*, Obstacles, HorseAimOne, SubHolder,  Girlgame, LightObj, HorseAimTwo*/;

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

    // Use this for initialization
    public override void Start () {
        base.Start();

        initilasePosition();

        hs = Horse.GetComponent<HorseScript>();
        noseCC = new characterComponents(Nose);


        startNoseGame();
        //noseCatched();
    }


    void initilasePosition()
    {
        Vector3 pos = player.transform.position + player.transform.forward * 50;
        NavMeshHit nmh;
        if(NavMesh.SamplePosition(pos, out nmh, 400f, Horse.GetComponent<NavMeshAgent>().areaMask))
        {
            transform.position = nmh.position;
        }else
        {
             Debug.Log("Failed to initilize");
        }

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

        //hs.release();
        handlerHolder= hs.setDes(player.transform.position + Vector3.forward * 20);
        pcc.StopToWalk();

        lookAtObject = Nose;

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

        //Timing.RunCoroutine(Vckrs._lookTo(player, Nose, 1));

        newspaper.SetActive(true);
        Nose.transform.parent = transform;

        lookAtObject = null;

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

        enbaleTrashObjs(true);

    }

    public void enbaleTrashObjs(bool b) {
        
        for(int i = 0; i < trash.transform.childCount; i++)
        {
            if (b)
            {
                ActivateAnotherObject.Activate(trash.transform.GetChild(i).gameObject);
            }
            else
            {
                ActivateAnotherObject.Disable(trash.transform.GetChild(i).gameObject);
            }
        }

    }


    public void noseCatched()
    {
        Timing.RunCoroutine(_noseCatched());
    }

    IEnumerator<float> _noseCatched()
    {
        newspaper.SetActive(false);
        enbaleTrashObjs(false);
        Timing.RunCoroutine(Vckrs._lookTo(Nose,player,1f));

        pcc.StopToWalk();
        sc.callSubtitleWithIndex(2);
        while (subtitle.text != "")
        {
            yield return 0;
        }

        //Set girl position
        Vector3 girlPosition =player.transform.position+  Vector3.Normalize(Horse.transform.position - player.transform.position) * 20;
        //CAst girl position to navmesh
        NavMeshHit nmh;
        if ((NavMesh.SamplePosition(girlPosition, out nmh, 30f, girl.GetComponent<NavMeshAgent>().areaMask)))
        {
            girlPosition=nmh.position;
        }

        girl.transform.position = girlPosition;

        girl.SetActive(true);

        lookAtObject = girl;

        subtitle.text = "Kovalev: Aman tanrım! Bu nasıl bir güzellik";
        
        NavMeshAgent nmaGirls = girl.GetComponent<NavMeshAgent>();
        nmaGirls.Resume();
        nmaGirls.SetDestination(player.transform.position + player.transform.forward * 5);

        DayAndNightCycle danc = sun.GetComponent<DayAndNightCycle>();
        danc.makeNight();

        //IEnumerator<float> lightHnadler = Timing.RunCoroutine(Vckrs._setLightIntensity(LightObj, 0.5f, 0));
        IEnumerator<float> girlWalkHandler = Timing.RunCoroutine(Vckrs.waitUntilStop(girl, 0f));

        yield return Timing.WaitUntilDone(girlWalkHandler);

        subtitle.text = "";
        lookAtObject = null;
        //Timing.RunCoroutine(Vckrs._lookTo(player, girl.transform.position - player.transform.position, 2f));
        

        girlCanvas.SetActive(true);

        yield return 0;
        girlCanvas.GetComponent<GirlGameController>().setKovalevPositionToInitialPosition();

        //yield return Timing.WaitForSeconds(0.5f);


        //GameObject girlGameKov = girlCanvas.transform.GetChild(0).GetChild(0).gameObject;
        //girlGameKov.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(300, 0, 0));
        

        MovementWithKeyboard2D mwk2 = girlGameKov.GetComponent<MovementWithKeyboard2D>();
        mwk2.speed = 0.05f;

        //For movement to left
        mwk2.scriptInput = -1;
        yield return Timing.WaitForSeconds(4f);

        mwk2.scriptInput = 0;
        mwk2.speed = 0.1f;

        CharacterController gkcc = girlGameKov.GetComponent<CharacterController>();
        gkcc.enabled = false;
        gkcc.enabled = true;

        GirlGameController ggc = girlCanvas.transform.GetComponent<GirlGameController>();
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

        DayAndNightCycle danc = sun.GetComponent<DayAndNightCycle>();
        danc.makeDay();

        sc.callSubtitleWithIndex(3);
        while (subtitle.text != "")
        {
            yield return 0;
        }


    }




}
