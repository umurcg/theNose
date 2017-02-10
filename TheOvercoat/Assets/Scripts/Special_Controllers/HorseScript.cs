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


    public enum animType { Bool, Trigger };
    public animType AnimType = animType.Bool;
    public string animationName = "Sit";
    GameObject wayPoints;
    myTween mt;

    HorseFreeze hf;

    //Check is at dest
    Vector3 destination;
    bool checkDest = false;

    //Debug
    //public bool unmount;
    // Use this for initialization
    void Awake() {  
        nma = GetComponent<NavMeshAgent>();
        wayPoints = transform.parent.GetChild(1).GetChild(0).GetChild(2).gameObject;
        mt = wayPoints.GetComponent<myTween>();
        hf = GetComponent<HorseFreeze>();
    }
	
	// Update is called once per frame
	void Update () {

        //if (unmount)
        //{
        //    unmount = false;
        //    Timing.RunCoroutine(_unMount());
        //}

        if (checkDest)
        {
            //print("checkin");
            if (Vector3.Distance(transform.position, destination) < 1)
            {
                nma.Stop();
                checkDest = false;
                callUnmount();
            }
        }
	
	}

    public void callUnmount()
    {
        Timing.RunCoroutine(_unMount());

    }

    IEnumerator<float> _unMount()
    {
        hf.freeze();
        if (passenger == null)
        {
            passenger = CharGameController.getActiveCharacter();
            //print("Passenfer is null");
            //yield break;
        }


        Animator anim = passenger.GetComponent<Animator>();
        if (anim)
            if (AnimType == animType.Bool)
            {
                anim.SetBool(animationName, false);
            }
            else
            {
                anim.SetTrigger(animationName);
            }


        mt.reverse = true;
        IEnumerator<float> handle = Timing.RunCoroutine(mt._tweenMEC(passenger, 2f));
        yield return Timing.WaitUntilDone(handle);

        PlayerComponentController pcc = passenger.GetComponent<PlayerComponentController>();
        NavMeshAgent nmaPas = passenger.GetComponent<NavMeshAgent>();
        if (nmaPas)
            nmaPas.enabled = true;
        
        if (pcc)
            pcc.ContinueToWalk();
        passenger.transform.parent= Camera.main.transform.parent;

    }

    IEnumerator<float> _mount()
    {
         if (passenger == null)
        {
            passenger = CharGameController.getActiveCharacter();
            //print("Passenfer is null");
            //yield break;
        }


        PlayerComponentController pcc = passenger.GetComponent<PlayerComponentController>();
        NavMeshAgent nmaPas = passenger.GetComponent<NavMeshAgent>();
        if (nmaPas)
        {
              nmaPas.enabled = false;
        }
        if (pcc)
             pcc.StopToWalk();

        mt.reverse = false;
        IEnumerator<float> handle = Timing.RunCoroutine(mt._tweenMEC(passenger, 2f));
        yield return Timing.WaitUntilDone(handle);
        
     
        Animator anim = passenger.GetComponent<Animator>();
        if(anim)
        if (AnimType == animType.Bool)
        {
                anim.SetBool(animationName, true);
        }else
        {
                anim.SetTrigger(animationName);
        }

        yield return Timing.WaitForSeconds(2);


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
        hf.release();
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(pos, out myNavHit, 100, nma.areaMask))
        {
            nma.SetDestination(myNavHit.position);
          
        }

        debugButton = false;

    }

    public void goToAim(int index)
    {
        hf.release();
        print("Button pressed");
        if (index < aims.Length)
        {
            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(aims[index].transform.position, out myNavHit, 100, nma.areaMask))
            {
                nma.Resume();
                nma.SetDestination(myNavHit.position);
                destination = myNavHit.position;
                checkDest = true;
            }

            debugButton = false;
        }
    }
}
