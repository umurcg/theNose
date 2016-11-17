using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;
using CinemaDirector;

//public GameObject HorseObject;
//public GameObject Kovalev;
//public GameObject Nose;
//public GameObject CSub;
//public GameObject NSub;
//public GameObject Obstacles;


public class City2GameController : MonoBehaviour {
    public GameObject Horse, Kovalev, Nose, CSub, NSub, Obstacles, noseEnterTrigger, HorseAimOne, SubHolder, NarCS;
    public float walkTolerance = 0.01f;
    Text CharSubt, NarSub;
    EnterTrigger NoseEt;

    // Use this for initialization
    void Start () {
        NoseEt = noseEnterTrigger.GetComponent<EnterTrigger>();
        CharSubt = CSub.GetComponent<Text>();
        NarSub = NSub.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        if (NoseEt.enter)
        {
            
            noseEnterTrigger.SetActive(false);
            NoseEt.enter = false;
            Timing.RunCoroutine(_noseEnter());
        }

	}

    public IEnumerator<float> _noseEnter()
    {
       
        HorseFreeze hf = Horse.GetComponent<HorseFreeze>();
        hf.release();

        PlayerComponentController pcc = Kovalev.GetComponent<PlayerComponentController>();
        pcc.StopToWalk();

        MountCarier mc = Nose.GetComponent<MountCarier>();
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

        mc.unmount();

        Pacing pac = Kovalev.GetComponent<Pacing>();
        pac.enabled = true;

        SubtitleCaller sc = SubHolder.GetComponent<SubtitleCaller>();
        sc.callSubtitle();

        NavMeshAgent nmaNose = Nose.GetComponent<NavMeshAgent>();
        nmaNose.enabled = true;
        RunRandomlyFromObject rrfo = Nose.GetComponent<RunRandomlyFromObject>();
        rrfo.enabled = true;

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

        pac.finishPacing();

        yield return Timing.WaitForSeconds(8f);

        Cutscene cs = NarCS.GetComponent<Cutscene>();
        cs.Play();
   
    }
}
