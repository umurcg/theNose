using UnityEngine;
using System.Collections;


public class FollowSmoothly : MonoBehaviour {

    public bool usingNavmesh;
    public GameObject target;
    public float speed;
    public float offset;

    NavMeshAgent nma;

	// Use this for initialization
	void Start () {

        if (usingNavmesh)
        {
            nma = GetComponent<NavMeshAgent>();
            nma.speed = speed;

        }
	    
	}
	
	// Update is called once per frame
	void Update () {

        if (target == null) return;

        Vector3 aim = target.transform.position - transform.forward * offset;

        if (usingNavmesh)
        {
            if (Vector3.Distance(transform.position, aim) > 0.01f)
            {
                nma.SetDestination(aim);
            }
        }
        else
        {
      
            if (Vector3.Distance(transform.position, aim) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, aim, Time.deltaTime * speed);
            }
        }
	}
}
