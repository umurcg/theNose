using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.AI;

[RequireComponent (typeof(UnityEngine.AI.NavMeshAgent))]
public class MoveRandomlyOnNavMesh : MonoBehaviour {

    public float radius;
    public float speed=3f;
    public float talkSomeoneProbibility = 3;
    public float talkDuration = 10f;
    UnityEngine.AI.NavMeshAgent nma;

    bool walking=false;
    bool talkingWithSomeone = false;
   

    // Use this for initialization
    void Awake () {

        nma = GetComponent<UnityEngine.AI.NavMeshAgent>();
        nma.speed = speed;
        talkSomeoneProbibility = Mathf.Clamp(talkSomeoneProbibility, 0, 100);
       
    }
	
	// Update is called once per frame
	void Update () {
       
        //If not walking then create new destination
        if(!walking && !talkingWithSomeone)
        {
            //Debug.Log("Setting new position "+Vckrs.nameTagLayer(gameObject));
            //Get position on circle
            Vector3 randomPos= Vckrs.generateRandomPositionOnCircle(transform.position, radius);

            //Cast it to position on navmesh
            Vector3 castedPos;
            if(Vckrs.findNearestPositionOnNavMesh(randomPos, nma.areaMask, radius, out castedPos)){
               Timing.RunCoroutine(_walkToPoint(castedPos));
            }

        }
	}




    IEnumerator<float> _walkToPoint(Vector3 pos)
    {
        walking = true;
        while (!nma.isOnNavMesh)
        {
            saveMe();
            yield return 0;
        }

        //Debug.Log("starting walking " + Vckrs.nameTagLayer(gameObject));

        //walking = true;
        nma.Resume();
        nma.SetDestination(pos);

        yield return Timing.WaitUntilDone(Timing.RunCoroutine(Vckrs.waitUntilStop(gameObject)));



        //walkCoruitine = null;

        //Debug.Log("finisihin walking " + Vckrs.nameTagLayer(gameObject));

        walking = false;

        yield break;

    }

    bool saveMe()
    {
        //Debug.Log("Saving bot " + Vckrs.nameTagLayer(gameObject));
        Vector3 castedPos;
        if (Vckrs.findNearestPositionOnNavMesh(transform.position, nma.areaMask, radius, out castedPos)) {
            castedPos = transform.position;
            return true;
        }

        //Debug.Log("trying to save " + gameObject.name);

        return false;


    }

    IEnumerator<float> letsTalk(MoveRandomlyOnNavMesh stranger)
    {
        //If already talking with someone, it can be me ;) just give up from talk
        if (stranger.isTalkingWithSomeone()) yield break;

        NavMeshAgent strangerNma = stranger.gameObject.GetComponent<NavMeshAgent>();
        talkWithMe(true);

        //Stop both of them
        strangerNma.isStopped = true;
        nma.isStopped = true;

        //Look at each other
        Timing.RunCoroutine(Vckrs._lookTo(gameObject, stranger.gameObject, 1f));
        Timing.RunCoroutine(Vckrs._lookTo(stranger.gameObject,gameObject ,1f));

        Debug.Log("starting to talk");

        yield return Timing.WaitForSeconds(talkDuration);

        Debug.Log("finishiny to talk");

        strangerNma.isStopped = false;
        nma.isStopped = false;

        yield break;
    }

    public bool isTalkingWithSomeone()
    {
        return talkingWithSomeone;
    }

    public void talkWithMe(bool talk)
    {
        talkingWithSomeone = talk;
    }
    
    //TODO talkWithEachOther
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Bot")
    //    {
    //        MoveRandomlyOnNavMesh mron = other.GetComponent<MoveRandomlyOnNavMesh>();

    //        if (!mron) return;

    //        float randomNumber = Random.Range(0, 100);
    //        if (randomNumber < talkSomeoneProbibility)
    //        {
    //            Debug.Log("lets talk about sex");
    //            //Talk with each other
    //            Timing.RunCoroutine(letsTalk(mron));
    //        }

    //    }
    //}

}
