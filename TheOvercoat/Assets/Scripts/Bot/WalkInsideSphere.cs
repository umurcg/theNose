using UnityEngine;
using System.Collections;

public class WalkInsideSphere : MonoBehaviour {
    public float walkRadius = 5f;
    public float waitBetweenWalks;

    float timer = -5;
    float threashold;
    NavMeshAgent nma;
    Vector3 center;
    Vector3 prevPos = Vector3.zero;

    public float navMeshSampleRadius = 0f;

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
               
                nma.SetDestination(getPosOnNavmesh());
                threashold = 0.5f;
                timer = -1;
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
}
