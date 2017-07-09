using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class PolicePatrol : MonoBehaviour {
    public float walkRadius = 5f;
    public float waitBetweenWalks;

    //This message will be called when police catches player
    public GameObject catchMessageObject;
    public string catchMessage;
    
    SubtitleCaller sc;
    WalkInsideSphere wis;
    float timer = 0;
    UnityEngine.AI.NavMeshAgent nma;
    Vector3 center;
    Vector3 prevPos=Vector3.zero;
    PlayerComponentController pcc;
    Text subtitle;

    // Use this for initialization
    void Start () {
        nma = GetComponent<UnityEngine.AI.NavMeshAgent>();
        center = transform.position;
        wis = GetComponent<WalkInsideSphere>();
        sc = GetComponent<SubtitleCaller>();
        pcc = GetComponent<PlayerComponentController>();

        //Enable right hand forward bool for light
        GetComponent<Animator>().SetBool("RightHandForward",true);
        subtitle = SubtitleFade.subtitles["CharacterSubtitle"];
    }
	
	// Update is called once per frame
	void Update () {


	}

    void OnTriggerEnter(Collider col)
    {
        //if (col.tag == "Player")
        //{
        //    wis.enabled = false;
        //    nma.Stop();
        //    Timing.RunCoroutine(_catch(col.transform.gameObject));
        //}
    }

    public void catchFun(GameObject obj){

        Timing.RunCoroutine(_catch(obj));
    }

    IEnumerator<float> _catch(GameObject go)
    {
        yield return 0;

        PlayerComponentController  pcc = go.GetComponent<PlayerComponentController>();

        if (pcc) pcc.StopToWalk();

        //Debug.Log("CATCH");
        sc.callSubtitleWithIndex(0);


        gameObject.GetComponent<WalkInsideSphere>().enabled = false;


        nma.Resume();
        nma.SetDestination(go.transform.position + go.transform.forward * 2);
        IEnumerator<float> handler =Timing.RunCoroutine(Vckrs.waitUntilStop(gameObject, 0));
        yield return Timing.WaitUntilDone(handler);

        Timing.RunCoroutine(Vckrs._lookTo(gameObject, go.transform.position-gameObject.transform.position, 2f));

        while (subtitle.text != "")
        {
            yield return 0;
        }

        sc.callSubtitleWithIndex(1);

        while (subtitle.text != "")
        {
            yield return 0;
        }
        
        if (catchMessageObject != null) catchMessageObject.SendMessage(catchMessage, gameObject);
    }


    public void Resume()
    {
        if(pcc!=null)  pcc.ContinueToWalk();
        nma.Resume();
        wis.enabled = true;
    }
}
