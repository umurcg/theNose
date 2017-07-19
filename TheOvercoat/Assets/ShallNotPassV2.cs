using MovementEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShallNotPassV2 : MonoBehaviour {

    SubtitleCaller sc;
    NavMeshAgent nma;
    public float waitBeforeMove = 3;
    public float moveDistance = 5f;
    PlayerComponentController pcc;
    GameObject player;

    // Use this for initialization
    void Start () {
        sc = GetComponent<SubtitleCaller>();
        nma = CharGameController.getActiveCharacter().GetComponent<NavMeshAgent>();
        player = nma.gameObject;
        pcc = player.GetComponent<PlayerComponentController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject==player)
        {
            
            sc.callSubtitleWithIndexTime(0);

            Timing.RunCoroutine(goBack());

        }

    }

    IEnumerator<float> goBack()
    {
        

        pcc.StopToWalk();
              
        yield return Timing.WaitForSeconds(waitBeforeMove);

        Vector3 movePos = player.transform.position - player.transform.forward * moveDistance;
        Vckrs.findNearestPositionOnNavMesh(movePos, nma.areaMask, moveDistance, out movePos);

        nma.isStopped = false;
        nma.SetDestination(movePos);

        yield return Timing.WaitUntilDone(Timing.RunCoroutine(Vckrs.waitUntilStop(gameObject)));

        pcc.ContinueToWalk();

    }

}
