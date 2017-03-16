using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class WalkInsideSphere : MonoBehaviour {
    public float walkRadius = 5f;
    public float waitBetweenWalks;

    float timer = -5;
    float threashold;
    NavMeshAgent nma;
    Vector3 center;
    Vector3 prevPos = Vector3.zero;

    public float navMeshSampleRadius = 0f;

    public Vector3 randomPosition = Vector3.zero;
    IEnumerator<float> randomPosHandler;

    // Use this for initialization
    void Start () {
        nma = GetComponent<NavMeshAgent>();
        center = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //print(timer);
        if (timer > 0)
            timer -= Time.deltaTime;

        if (threashold > 0)
            threashold -= Time.deltaTime;

        if ((prevPos == Vector3.zero || Vector3.Distance(prevPos, transform.position) == 0) &&  threashold<=0)
        {
            threashold = 0;
            if (timer == -1)
            {
                timer = waitBetweenWalks;
            }
            
            if(timer<0)
            {
                if (randomPosHandler == null && randomPosition == Vector3.zero)
                {
                  randomPosHandler=  Timing.RunCoroutine(tryToGetPosOnNavmesh());
                }
                else if (randomPosition != Vector3.zero)
                {
                    nma.SetDestination(randomPosition);
                    threashold = 0.5f;
                    timer = -1;
                    randomPosition = Vector3.zero;
                }
            }

        }

        prevPos = transform.position;
    }

    Vector3 getPosOnNavmesh()
    {
        Vector3 randomPos = Random.insideUnitSphere * walkRadius + center;
        if (navMeshSampleRadius == 0) return randomPos;
        

        //Cast navmeshpos
        NavMeshHit nmh;
        if (NavMesh.SamplePosition(randomPos, out nmh, navMeshSampleRadius, nma.areaMask))
        {
            return nmh.position;
        }else
        {
            return randomPos;
        }

    }

    void onDisable()
    {
        if (randomPosHandler != null)
        {
            Timing.KillCoroutines(randomPosHandler);
            randomPosHandler = null;
        }
    }

    IEnumerator<float> tryToGetPosOnNavmesh()
    {
        Vector3 randomPos = Random.insideUnitSphere * walkRadius + center;
        if (navMeshSampleRadius == 0)
        {
            randomPosition = randomPos;
            randomPosHandler = null;
            yield break;
        }


        Vector3 foundPos = Vector3.zero;

        while (foundPos == Vector3.zero)
        {
            //Cast navmeshpos
            NavMeshHit nmh;
            if (NavMesh.SamplePosition(randomPos, out nmh, navMeshSampleRadius, nma.areaMask))
            {
                foundPos = nmh.position;
            }

            yield return 0;
        }


        randomPosition = foundPos;
        randomPosHandler = null;
        yield break;

    }




}
