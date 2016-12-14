using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class PolicePatrol : MonoBehaviour {
    public float walkRadius = 5f;
    public float waitBetweenWalks;
    

    
    SubtitleCaller sc;
    WalkInsideSphere wis;
    float timer = 0;
    NavMeshAgent nma;
    Vector3 center;
    Vector3 prevPos=Vector3.zero;
    // Use this for initialization
	void Start () {
        nma = GetComponent<NavMeshAgent>();
        center = transform.position;
        wis = GetComponent<WalkInsideSphere>();
        sc = GetComponent<SubtitleCaller>();
        

    }
	
	// Update is called once per frame
	void Update () {


	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            wis.enabled = false;
            nma.Stop();
            Timing.RunCoroutine(_catch(col.transform.gameObject));
        }
    }

    IEnumerator<float> _catch(GameObject go)
    {
        sc.callSubtitleWithIndex(0);
        PlayerComponentController pcc = go.GetComponent<PlayerComponentController>();
        if (pcc != null)
            pcc.StopToWalk();
        nma.Resume();
        nma.SetDestination(go.transform.position + go.transform.forward * 2);
        yield return Timing.WaitUntilDone(Vckrs.waitUntilStop(gameObject, 0));
        Timing.RunCoroutine(Vckrs._lookTo(gameObject, go.transform.position-gameObject.transform.position, 2f));
        while (SubtitleFade.subtitles["CharacterSubtitle"].text != "")
        {
            yield return 0;
        }

        sc.callSubtitleWithIndex(1);

     }


}
