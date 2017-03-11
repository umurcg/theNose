using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using System;

//This script is special for horse carier.
//It triggers camera to fly to map view
//And it force to carier to go destinations via agent.
//Passenger can be player or actor
//If you not specify passenger it will automatically get player as passenger

public class HorseScript : MonoBehaviour,IClickAction, IClickActionDifferentPos {
    


    public GameObject passenger;
    NavMeshAgent nma;
    
    //For mounting animation an walking
    public enum animType { Bool, Trigger };
    public animType AnimType = animType.Bool;
    public string animationName = "Sit";
    GameObject wayPoints;
    myTween mt;


    public GameObject carierFront;
    public GameObject carierBack;
    Rigidbody carierBackcc;
    Rigidbody carierFrontcc;
    Rigidbody cc;

    //Check is at dest
    Vector3 destination;
    bool checkDest = false;

    //Debug
    public bool mountDebug;
    public bool unmountDebug;

    IEnumerator<float> setDestHandler;

    // Use this for initialization
    void Awake() {  
        nma = GetComponent<NavMeshAgent>();
        wayPoints = transform.parent.GetChild(1).GetChild(0).GetChild(2).gameObject;
        mt = wayPoints.GetComponent<myTween>();
        cc = GetComponent<Rigidbody>();
        carierBackcc = carierBack.GetComponent<Rigidbody>();
        carierFrontcc = carierFront.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        if (unmountDebug)
        {
            unmountDebug = false;
            Timing.RunCoroutine(_unMount());
        }

        if (mountDebug)
        {
            mountDebug = false;
            Timing.RunCoroutine(_mount());
        }

        if (checkDest)
        {
            if (Vector3.Distance(transform.position, destination) < 0.1f)
            {
                nma.Stop();
                checkDest = false;
                unmount();
            }
        }
	
	}

    //Mount and unmount is working
    public IEnumerator<float> unmount()
    {
        return Timing.RunCoroutine(_unMount());

    }

    IEnumerator<float> _unMount()
    {
        bool isPassengerPlayer = false;
        freeze();
        if (passenger != null)
        {

            //passenger = CharGameController.getActiveCharacter();
            isPassengerPlayer = true;
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

        if (isPassengerPlayer)
        {
            passenger.transform.parent = Camera.main.transform.parent;
        }
        else
        {
            passenger.transform.parent = null;
        }
        passenger.GetComponent<BasicCharAnimations>().enabled = true;
    }

    public IEnumerator<float> mount()
    {
       return Timing.RunCoroutine(_mount());
    }

    IEnumerator<float> _mount()
    {
        bool isPassengerPlayer = false;
         if (passenger == null || !passenger.activeSelf)
        {
            passenger = CharGameController.getActiveCharacter();
            isPassengerPlayer = true;
        }


        PlayerComponentController pcc = passenger.GetComponent<PlayerComponentController>();
        NavMeshAgent nmaPas = passenger.GetComponent<NavMeshAgent>();
        if (nmaPas)
        {
            if (nmaPas.isOnNavMesh) ;
                nmaPas.Stop();
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



        passenger.transform.parent = carierBack.transform;
        if (!isPassengerPlayer)
        {        
            yield break;
        }


        //If passenger is player open map view
        //passenger.transform.parent.parent = transform;
        BirdsEyeView bev = Camera.main.GetComponent<BirdsEyeView>();
        bev.messageReciever = gameObject;
        bev.goToBirdEye();


        passenger.GetComponent<BasicCharAnimations>().enabled = false;

        //FlyCameraBetween fcb = Camera.main.gameObject.GetComponent<FlyCameraBetween>();
        //if (fcb)
        //{
        //    fcb.fly();
        //}


    }

    public void Action()
    {
        //If passenger is not null which means it is not for player, iclick action can't call mount function
        if (passenger != null) return;
        Timing.RunCoroutine(_mount());
   
    }

    public IEnumerator<float>  setDes(Vector3 pos)
    {
        Debug.Log("Set des");
        setDestHandler =Timing.RunCoroutine(_setDes(pos));
        return setDestHandler;
       
    }

    IEnumerator<float> _setDes(Vector3 pos)
    {

        GameObject mainCam = CharGameController.getCamera();
        CameraFollower cf = mainCam.GetComponent<CameraFollower>();

        GameObject activePlayer = CharGameController.getActiveCharacter();

        GameObject orgTarget=null;
        if (passenger == activePlayer) {
            //Set aim of camera follower to owner
            orgTarget = cf.target;
            cf.changeTarget(gameObject);
        }


        release();
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(pos, out myNavHit, 100, nma.areaMask))
        {
            nma.SetDestination(myNavHit.position);

        }
        else
        {
            Debug.Log("Couldn't sample aim");
        }

        IEnumerator<float> walkHandler = Timing.RunCoroutine(Vckrs.waitUntilStop(gameObject));
        yield return Timing.WaitUntilDone(walkHandler);

        freeze();


        if (passenger == activePlayer)
        {
            cf.changeTarget(orgTarget);

        }

        unmount();
        yield break;
        

    }

    ////For points on mesh.
    ////TODO calculate points with mouse click so this will be removeed
    //public void goToAim(int index)
    //{
    //    release();
    //    //print("Button pressed");
    //    if (index < aims.Length)
    //    {
    //        NavMeshHit myNavHit;
    //        if (NavMesh.SamplePosition(aims[index].transform.position, out myNavHit, 100, nma.areaMask))
    //        {
    //            nma.Resume();
    //            nma.SetDestination(myNavHit.position);
    //            destination = myNavHit.position;
    //            checkDest = true;
    //        }

    //        //debugButton = false;
    //    }
    //}


    public void freeze()
    {
        Debug.Log("freeze");
        cc.constraints = RigidbodyConstraints.FreezeAll;
        carierBackcc.constraints = RigidbodyConstraints.FreezeAll;
        carierFrontcc.constraints = RigidbodyConstraints.FreezeAll;
        nma.enabled = false;
    }
    public void release()
    {
        Debug.Log("release");
        cc.constraints = RigidbodyConstraints.None;
        carierBackcc.constraints = RigidbodyConstraints.None;
        carierFrontcc.constraints = RigidbodyConstraints.None;
        nma.enabled = enabled;
    }

    public Vector3 giveMePosition()
    {
        return wayPoints.transform.GetChild(0).transform.position;
    }
}
