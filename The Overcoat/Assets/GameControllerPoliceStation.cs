using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;
public class GameControllerPoliceStation : MonoBehaviour {
    public GameObject trigger, Kovalev, Gaurd, CharSubtitle;

    EnterTrigger et;
    PlayerComponentController pcc;
    SubtitleCaller sc;
    Text sub;
    public bool debug;
    // Use this for initialization
	void Start () {
        et = trigger.GetComponent<EnterTrigger>();
        pcc = Kovalev.GetComponent<PlayerComponentController>();
        sc = GetComponent<SubtitleCaller>();
        sub = CharSubtitle.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        //if (et.enter)
        //{
          
        //    et.enabled = false;
        //    et.enter = false;
        //    Timing.RunCoroutine(_guardStopsKovalev());
        //}
        //if (debug)
        //{
        //    debug = false;
        //    Timing.RunCoroutine(_guardStopsKovalev());
        //}
    }

    public void callGuardStopsKovalev() {
        Timing.RunCoroutine(_guardStopsKovalev());
    }

    IEnumerator<float> _guardStopsKovalev()
    {
        Gaurd.GetComponent<ClickExternalCallSubtitle>().Destroy();

        trigger.SetActive(false);
        pcc.StopToWalk();
        NavMeshAgent navKov = Kovalev.GetComponent<NavMeshAgent>();
        navKov.Stop();


        sc.callSubtitleWithIndex(0);

        yield return Timing.WaitForSeconds(2f);
        NavMeshAgent guardnNma = Gaurd.GetComponent<NavMeshAgent>();
        guardnNma.Resume();
        guardnNma.SetDestination(Vector3.Lerp(Gaurd.transform.position, Kovalev.transform.position, 0.7f));
       
        Timing.RunCoroutine(Vckrs._lookTo(Kovalev, Gaurd.transform.position, 0.5f));

        yield return Timing.WaitForSeconds(0.5f);
        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs.waitUntilStop(Gaurd, 0f));
        yield return Timing.WaitUntilDone(handler);

        Timing.RunCoroutine(Vckrs._lookTo(Gaurd, Kovalev.transform.position-Gaurd.transform.position, 0.5f));

        while (sub.text != "")
        {
           
            yield return 0;
        }

        sc.callSubtitleWithIndex(1);
        while(true)
        yield return 0;
        
    }
}
