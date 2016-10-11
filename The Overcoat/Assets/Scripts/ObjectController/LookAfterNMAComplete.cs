using UnityEngine;
using System.Collections;

public class LookAfterNMAComplete : MonoBehaviour {

    NavMeshAgent nma;
    public GameObject[] aims;
    bool didntRotate = true;
    
    
    //This funciton rotates object to aim when navmeshagent reaches to dest.
    //LerpLookTo script mus be attached.

	// Use this for initialization
	void Start () {

        nma = GetComponent<NavMeshAgent>();
        

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator LookFinishNma(int index)

    {
        didntRotate = true;
        while (didntRotate)
        {


            if (nma.pathPending)
            {
                if (nma.remainingDistance <= nma.stoppingDistance)
                {
                    if (!nma.hasPath || nma.velocity.sqrMagnitude == 0f)
                    {
                       
                    }
                }
            }

        }

        yield return null;
    }


    
}
