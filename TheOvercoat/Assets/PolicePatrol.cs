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
    NavMeshAgent nma;
    Vector3 center;
    Vector3 prevPos=Vector3.zero;
    PlayerComponentController pcc;

    // Use this for initialization
    void Start () {
        nma = GetComponent<NavMeshAgent>();
        center = transform.position;
        wis = GetComponent<WalkInsideSphere>();
        sc = GetComponent<SubtitleCaller>();
        pcc = GetComponent<PlayerComponentController>();

        //Enable right hand forward bool for light
        GetComponent<Animator>().SetBool("RightHandForward",true);

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

        //nma.Resume();
        //nma.SetDestination(go.transform.position + go.transform.forward * 2);
        //yield return Timing.WaitUntilDone(Vckrs.waitUntilStop(gameObject, 0));
        Timing.RunCoroutine(Vckrs._lookTo(gameObject, go.transform.position-gameObject.transform.position, 2f));
        while (SubtitleFade.subtitles["CharacterSubtitle"].text != "")
        {
            yield return 0;
        }

        sc.callSubtitleWithIndex(1);

        while (SubtitleFade.subtitles["CharacterSubtitle"].text != "")
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
