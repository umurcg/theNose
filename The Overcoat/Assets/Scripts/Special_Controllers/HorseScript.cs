using UnityEngine;
using System.Collections;

//This script is special for horse carier.
//It triggers camera to fly to map view
//And it force to carier to go destinations via agent.

public class HorseScript : MonoBehaviour, IClickAction {

    public GameObject[] aims;
    public bool debugButton=false;
    public GameObject passenger;
    NavMeshAgent nma;
    

	// Use this for initialization
	void Awake() {  
        nma = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {





	
	}

    public void Action()
    {
        FlyCameraBetween fcb = Camera.main.gameObject.GetComponent<FlyCameraBetween>();
        if (fcb)
        {
            fcb.fly();
        }

    }

    public void setDes(Vector3 pos)
    {
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(pos, out myNavHit, 100, nma.areaMask))
        {
            nma.SetDestination(myNavHit.position);
          
        }

        debugButton = false;

    }

    public void goToAim(int index)
    {
        if (index < aims.Length)
        {
            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(aims[index].transform.position, out myNavHit, 100, nma.areaMask))
            {
                nma.SetDestination(myNavHit.position);
                if (passenger != null)
                {
                    PlayerComponentController pcc = passenger.GetComponent<PlayerComponentController>();
                    NavMeshAgent nmaPas = passenger.GetComponent<NavMeshAgent>();
                    if (nmaPas)
                    {
                        nmaPas.enabled = false;
                    }
                    if (pcc)
                        pcc.StopToWalk();
                    passenger.transform.parent = transform;
                    
                }


            }

            debugButton = false;
        }
    }
}
