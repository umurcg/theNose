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
               
                nma.SetDestination(Random.insideUnitSphere * walkRadius + center);
                threashold = 0.5f;
                timer = -1;
            }

        }

        prevPos = transform.position;
    }
}
