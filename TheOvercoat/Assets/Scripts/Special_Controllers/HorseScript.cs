using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using System;
using UnityEngine.UI;

//This script is special for horse carier.
//It triggers camera to fly to map view
//And it force to carier to go destinations via agent.
//Passenger can be player or actor
//If you not specify passenger it will automatically get player as passenger

public class HorseScript : MonoBehaviour,IClickAction, IClickActionDifferentPos {
        
    public GameObject passenger;
    NavMeshAgent nma;

    SubtitleCaller sc;
    
    //For mounting animation an walking
    public enum animType { Bool, Trigger };
    public animType AnimType = animType.Bool;
    public string animationName = "Sit";
    public GameObject[] wayPoints;
    myTween mt;

    //Check this if passenger is not player
    public bool isPassengerPlayer=true;

    public GameObject carierFront;
    public GameObject carierBack;
    Rigidbody carierBackcc;
    Rigidbody carierFrontcc;
    Rigidbody cc;

    CharacterControllerKeyboard cck;

    public GameObject canvas;

    //Check is at dest
    Vector3 destination;
    bool checkDest = false;

    public GameObject horseRoad;

    //Debug
    public bool mountDebug;
    public bool unmountDebug;

    GameObject mainCanvas;
    public GameObject carierChoices;
    public GameObject unmountButton;

    GameObject spawnedCarierChoice;
    GameObject spawnedUnmountButton;

    IEnumerator<float> setDestHandler;
    

    // Use this for initialization
    void Awake() {  
        nma = GetComponent<NavMeshAgent>();
        //mt = wayPoints.GetComponent<myTween>();
        cc = GetComponent<Rigidbody>();
        carierBackcc = carierBack.GetComponent<Rigidbody>();
        carierFrontcc = carierFront.GetComponent<Rigidbody>();
        sc = GetComponent<SubtitleCaller>();
        cck = GetComponent<CharacterControllerKeyboard>();
        mainCanvas = GameObject.FindGameObjectWithTag("Canvas");
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


    void unmountButtonFunc()
    {
        cck.enabled = false;
        Destroy(spawnedUnmountButton);
        unmount();
        freeze();
    }

    //Mount and unmount is working
    public IEnumerator<float> unmount()
    {
        return Timing.RunCoroutine(_unMount());

    }

    IEnumerator<float> _unMount()
    {
        freeze();

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

        //Enable basic animation ctonroller if exists
        BasicCharAnimations bca = passenger.GetComponent<BasicCharAnimations>();
        if (bca) bca.enabled = true;

        if (!mt) mt=nearestMT();
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
            passenger.transform.parent = CharGameController.getOwner().transform ;
            passenger = null;
        }
        else
        {
            passenger.transform.parent = null;
        }

        if (bca) bca.enabled = true;
        //passenger.GetComponent<BasicCharAnimations>().enabled = true;

        //Update camera follower
        //CharGameController.getCamera().GetComponent<CameraFollower>().updateTarget();

        yield break;
    }

    public IEnumerator<float> mount()
    {
       return Timing.RunCoroutine(_mount());
    }

    IEnumerator<float> _mount()
    {
        if(isPassengerPlayer) passenger= CharGameController.getActiveCharacter(); 

        

        PlayerComponentController pcc = passenger.GetComponent<PlayerComponentController>();
        NavMeshAgent nmaPas = passenger.GetComponent<NavMeshAgent>();

        if (nmaPas)
        {
            if (nmaPas.isOnNavMesh) 
                nmaPas.Stop();
             nmaPas.enabled = false;
        }

        if (pcc)
             pcc.StopToWalk();

        mt = nearestMT();

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

        //If passenger is player then ask him wether or not he wants to control.
        sc.callSubtitleWithIndex(0);

        passenger.GetComponent<BasicCharAnimations>().enabled = false;

        Text subtitle=SubtitleFade.subtitles["CharacterSubtitle"];
        while (subtitle.text != "") yield return 0;

        spawnedCarierChoice=Instantiate(carierChoices, canvas.transform) as GameObject;
        RectTransform rt = spawnedCarierChoice.GetComponent<RectTransform>();
        rt.position = new Vector2(Screen.width / 2 + rt.rect.width / 2, Screen.height * 1 / 3);

        Button[] buttons = spawnedCarierChoice.GetComponentsInChildren<Button>();
        Button button1 = buttons[0];
        Button button2 = buttons[1];

        button1.onClick.AddListener(letUserDrive);
        button2.onClick.AddListener(autoControl);

    }

    void letUserDrive()
    {
        cck.enabled = true;
        Destroy(spawnedCarierChoice);

        spawnedUnmountButton = Instantiate(unmountButton, mainCanvas.transform) as GameObject;
        RectTransform rt = spawnedUnmountButton.GetComponent<RectTransform>();
        rt.position = new Vector2(Screen.width *4/5 + rt.rect.width / 2, Screen.height * 4/5);

        Button button1 = spawnedUnmountButton.GetComponent<Button>();
        button1.onClick.AddListener(unmountButtonFunc);

        releaseForUserController();

    }

    void autoControl()
    {
        BirdsEyeView bev = Camera.main.GetComponent<BirdsEyeView>();
        bev.messageReciever = gameObject;
        bev.goToBirdEye(true);
        Destroy(spawnedCarierChoice);
    }


    public void Action()
    {
        //If passenger is not null which means it is not for player, iclick action can't call mount function
        if (passenger != null) return;
        Timing.RunCoroutine(_mount());
   
    }

    public IEnumerator<float>  setDes(Vector3 pos, bool unmountOnFinish=true)
    {
        Debug.Log("Set des");
        setDestHandler =Timing.RunCoroutine(_setDes(pos,unmountOnFinish));
        return setDestHandler;
       
    }

    public IEnumerator<float> setDesAndUnmount(Vector3 pos)
    {
        Debug.Log("Set des");
        setDestHandler = Timing.RunCoroutine(_setDes(pos, true));
        return setDestHandler;

    }

    IEnumerator<float> _setDes(Vector3 pos, bool unmountOnFinish=true)
    {

        GameObject mainCam = CharGameController.getCamera();
        //CameraFollower cf = mainCam.GetComponent<CameraFollower>();

        GameObject activePlayer = CharGameController.getActiveCharacter();

        //GameObject orgTarget=null;
        
        //if (passenger == activePlayer) {
        //    //Set aim of camera follower to owner
        //    orgTarget = cf.target;
        //    cf.changeTarget(gameObject);
        //}


        releaseForNavMeshController();
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


        //if (passenger == activePlayer)
        //{
        //    cf.changeTarget(orgTarget);

        //}

        if(unmountOnFinish)
            unmount();  
        yield break;
        

    }

    
    public void freeze()
    {
        //Debug.Log("freeze");
        cc.constraints = RigidbodyConstraints.FreezeAll;
        carierBackcc.constraints = RigidbodyConstraints.FreezeAll;
        carierFrontcc.constraints = RigidbodyConstraints.FreezeAll;
        nma.enabled = false;
    }
    public void releaseForNavMeshController()
    {
        //Debug.Log("release");
        cc.constraints = RigidbodyConstraints.None;
        carierBackcc.constraints = RigidbodyConstraints.None;
        carierFrontcc.constraints = RigidbodyConstraints.None;
        nma.enabled = enabled;
    }

    public void releaseForUserController()
    {
        carierBackcc.constraints = RigidbodyConstraints.None;
        carierFrontcc.constraints = RigidbodyConstraints.None;
    }

    public Vector3 giveMePosition()
    {
        //Debug.Log(wayPoints.transform.childCount);
        return nearestWayPoint().transform.GetChild(0).transform.position;
    }


    GameObject nearestWayPoint()
    {
        GameObject wayPoint=null;
        float minDistance = Mathf.Infinity;

        foreach(GameObject wp in wayPoints)
        {
            GameObject child = wp.transform.GetChild(0).gameObject;
            float dist = 0;

            if (passenger == null)
            {
                GameObject player = CharGameController.getActiveCharacter();
                dist = Vector3.Distance(player.transform.position, child.transform.position);
            }
            else
            {
                dist = Vector3.Distance(passenger.transform.position, child.transform.position);
            }
            if (dist < minDistance)
            {
                minDistance = dist;
                wayPoint = wp;
            }
        }

        return wayPoint;

    }

    myTween nearestMT()
    {
        return nearestWayPoint().GetComponent<myTween>();
    }

}
