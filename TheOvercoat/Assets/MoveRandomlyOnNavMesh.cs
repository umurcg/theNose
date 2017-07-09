using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

[RequireComponent (typeof(UnityEngine.AI.NavMeshAgent))]
public class MoveRandomlyOnNavMesh : MonoBehaviour {

    public float radius;
    public float speed=3f;
    UnityEngine.AI.NavMeshAgent nma;

    //bool walking=false;
    IEnumerator<float> walkCoruitine;

    // Use this for initialization
    void Awake () {

        nma = GetComponent<UnityEngine.AI.NavMeshAgent>();

        //if (!nma)
        //{
        //    //Debug.Log("One of required components is empty");
        //    enabled = false;
        //}

        nma.speed = speed;
       
    }
	
	// Update is called once per frame
	void Update () {
       
        if(walkCoruitine==null)
        {
            //Get position on circle
            Vector3 randomPos= Vckrs.generateRandomPositionOnCircle(transform.position, radius);

            //Cast it to position on navmesh
            Vector3 castedPos;
            if(Vckrs.findNearestPositionOnNavMesh(randomPos, nma.areaMask, radius, out castedPos)){
               walkCoruitine= Timing.RunCoroutine(_walkToPoint(castedPos));
            }

        }
	}




    IEnumerator<float> _walkToPoint(Vector3 pos)
    {
        while (!nma.isOnNavMesh)
        {
            saveMe();
            yield return 0;
        }

        //walking = true;
        nma.Resume();
        nma.SetDestination(pos);

        Timing.WaitUntilDone(Timing.RunCoroutine(Vckrs.waitUntilStop(gameObject)));

        walkCoruitine = null;

        yield break;

    }

    bool saveMe()
    {
        Vector3 castedPos;
        if (Vckrs.findNearestPositionOnNavMesh(transform.position, nma.areaMask, radius, out castedPos)) {
            castedPos = transform.position;
            return true;
        }

        Debug.Log("trying to save " + gameObject.name);

        return false;


    }


}
