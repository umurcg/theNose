using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//This script is special for horse carier.
//It triggers camera to fly to map view
//And it force to carier to go destinations via agent.

public class HorseScript : MonoBehaviour, IClickAction {

    public GameObject[] aims;
    public bool debugButton=false;
    public GameObject passenger;
    public float mountTime=5f;
    NavMeshAgent nma;
    

	// Use this for initialization
	void Awake() {  
        nma = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {

        
	
	}

    IEnumerator<float> _mount()
    {

        GameObject wayPoints = transform.parent.GetChild(1).GetChild(0).GetChild(2).gameObject;
        myTween mt = wayPoints.GetComponent<myTween>();

        if (passenger == null)
        {
            print("Passenfer is null");
            yield break;
        }
       
        PlayerComponentController pcc = passenger.GetComponent<PlayerComponentController>();
        NavMeshAgent nmaPas = passenger.GetComponent<NavMeshAgent>();
        if (nmaPas)
        {
              nmaPas.enabled = false;
        }
        if (pcc)
             pcc.StopToWalk();
        

        Timing.RunCoroutine(mt._tweenMEC(passenger, 2f));
        float t = mountTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return 0;

        }
        passenger.transform.parent = transform;
        FlyCameraBetween fcb = Camera.main.gameObject.GetComponent<FlyCameraBetween>();
        if (fcb)
        {
            fcb.fly();
        }


    }

    public void Action()
    {

        Timing.RunCoroutine(_mount());
   
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

            }

            debugButton = false;
        }
    }
}
