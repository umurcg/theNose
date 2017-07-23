using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.AI;
using UnityEngine.UI;

public class ReyhanCityController : GameController, IClickAction {


    public bool debug = false;
    public HorseScript hs;
    public GameObject reyhanGameObject;
    public GameObject horseDest;


    //D'stance between characters when they sit
    public float sitPoisitonOffset;
    GameObject sitPosition;
    public GameObject girty;
    NavMeshAgent reyhanAgent;
    public GameObject renc;

    public GameObject canvas3D;
    public GameObject canvas2D;
    public GameObject padlock;
    public GameObject buttonPrefab;
    Camera maincam;


    GameObject spawnedLock;
    GameObject takeItButton;
    GameObject leaveItButton;

    public override void Start()
    {
        base.Start();

        bool firstGC = GlobalController.Instance.isGameControllerIsUsedSceneNameAndGameObjectName("NewsPaperR.");
        bool secondGC = GlobalController.Instance.isGameControllerIsUsedSceneNameAndGameObjectName(generateIDWithEpisodeID());

        if (firstGC && !secondGC || debug)
        {
            activateController();
            Debug.Log("ACTIVATING REYHAN");
        }
        else
        {
            deactivateController();
            return;
        }

        //Change way point at each mount so characters wont sit top of each other.
        //It is a bad design but still it works ;)
        //sitPosition = hs.wayPoints[0].transform.GetChild(hs.wayPoints[0].transform.childCount - 1).gameObject;

        maincam = CharGameController.getMainCameraComponent();

        reyhanAgent = gameObject.GetComponent<NavMeshAgent>();

        Timing.RunCoroutine(goToCarier());




    }


    IEnumerator<float> goToCarier()
    {
        yield return 0;

        sc.callSubtitleWithIndex(0);

        hs.gameObject.tag = "Untagged";
        while (subtitle.text != "") yield return 0;



        hs.passenger = gameObject;

        //Change sit positiion object pos
        //sitPosition.transform.position = sitPosition.transform.position - sitPosition.transform.right * sitPoisitonOffset/2;

        hs.animationName = "SitPosition";

        hs.isPassengerPlayer = false;
        handlerHolder= hs.mount();

        

        yield return Timing.WaitUntilDone(handlerHolder);

        if(hs.wayPoints.Length == 2){

            GameObject usedWaypoint = hs.nearestWayPoint();
            GameObject[] waypoints = hs.wayPoints;


            if (waypoints[0] == usedWaypoint)
            {
                hs.wayPoints = new GameObject[] { waypoints[1] };
            }
            else
            {
                hs.wayPoints = new GameObject[] { waypoints[0] };
            }
        }

        BroadcastOnClick bc = hs.gameObject.AddComponent<BroadcastOnClick>();
        bc.destroyAfterBC = true;
        bc.reciever = gameObject;
        bc.message = "forcePlayerToCarier";

        hs.gameObject.tag = "ActiveObject";




    }

    public void forcePlayerToCarier()
    {
        Timing.RunCoroutine(_forcePlayerToCarier());


  
    }

    IEnumerator<float> _forcePlayerToCarier()
    {

        Debug.Log("Force player to carier");

        hs.gameObject.tag = "Untagged";

        hs.animationName = "Sit";

        //Change sit positiion object pos
        //sitPosition.transform.position = sitPosition.transform.position + sitPosition.transform.right * sitPoisitonOffset/5;

        hs.passenger = player;
        handlerHolder = hs.mount();

        yield return Timing.WaitUntilDone(handlerHolder);

        hs.setDes(horseDest.transform.position,false);

        sc.callSubtitleWithIndexTime(0);
        while (sc.getActiveController().enabled) yield return 0;

        yield return Timing.WaitUntilDone(Timing.RunCoroutine(Vckrs.waitUntilStop(hs.gameObject)));

        hs.isPassengerPlayer = true;
        yield return Timing.WaitUntilDone(hs.unmount());

        hs.passenger = gameObject;

        hs.animationName = "SitPosition";

        hs.isPassengerPlayer = false;

        yield return Timing.WaitUntilDone(hs.unmount());

        yield return 0;

        Vector3 randomPos = Vckrs.generateRandomPositionOnCircle(hs.gameObject.transform.position, 100);

        characterComponents horseCC = new characterComponents(hs.gameObject);

        Vckrs.findNearestPositionOnNavMesh(randomPos, horseCC.navmashagent.areaMask, 100, out randomPos);

        hs.setDes(randomPos);


        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs.destroyWhenOutScreen(hs.transform.parent.gameObject));

        //Timing.RunCoroutine(Vckrs._lookTo(gameObject, player,1f));
        AlwaysLookTo alt = gameObject.AddComponent<AlwaysLookTo>();
        alt.aim = player;
        
        Timing.RunCoroutine(Vckrs._lookTo(player, gameObject, 1f));
        
        sc.callSubtitleWithIndex(2);

        yield return Timing.WaitUntilDone(handler);

        Destroy(hs);

        yield break;

    }


    // Update is called once per frame
    void Update () {
	
	}

    public override void deactivateController()
    {
        base.deactivateController();
        gameObject.SetActive(false);
        //girty.SetActive(false);
    }

    public override void activateController()
    {
        base.activateController();
        gameObject.SetActive(true);
        girty.SetActive(false);
        reyhanGameObject.SetActive(true);

    }

    [ContextMenu ("key is found")]
    public void keyIsfound()
    {
        Debug.Log("KEEEEY IS FOUUUND");
        renc.transform.parent = null;
        renc.transform.position = transform.position + transform.forward * 3;
        renc.transform.LookAt(transform);
        transform.LookAt(renc.transform);

        //remove after look scripts.
        AlwaysLookTo[] alts = GetComponents<AlwaysLookTo>();

        foreach (AlwaysLookTo alt in alts) Destroy(alt);


        renc.SetActive(true);

        gameObject.tag = "ActiveObject";
    }

    IEnumerator<float> reyhanLeavesKovalev()
    {
        gameObject.tag = "Untagged";

    

        sc.callSubtitleWithIndex(3);

        while (sc.getCurrentSubtIndex() < 2) yield return 0;

        Timing.RunCoroutine(Vckrs._lookTo(gameObject, player, 1f));
        Timing.RunCoroutine(Vckrs._lookTo(renc, player, 1f));

        while (subtitle.text != "") yield return 0;

        if (!reyhanAgent.enabled) reyhanAgent.enabled = true; 

        Vector3 randomPos = Vckrs.generateRandomPositionOnCircle(transform.position, 100f);
        Vckrs.findNearestPositionOnNavMesh(randomPos, reyhanAgent.areaMask, 100f, out randomPos);
        
        NavMeshAgent rencNma = renc.GetComponent<NavMeshAgent>();
        rencNma.SetDestination(randomPos + Vector3.forward * 3);
        reyhanAgent.SetDestination(randomPos);

        
        reyhanAgent.isStopped = false;

        yield return Timing.WaitForSeconds(7.5f);

       

        pcc.ContinueToWalk();

        Vector3 initialPos = player.transform.position;

        while (Vector3.Distance(initialPos, player.transform.position) < 30) yield return 0;

       

        Debug.Log("Find lock");
        //Find lock on the floor

        pcc.StopToWalk();
        sc.callSubtitleWithIndex(4);
        while (subtitle.text != "") yield return 0;

        ////Register game controller
        //registerAsUsed();

        findLock();
      

       

        yield break;
        
    }

    [ContextMenu ("Register game controller")]
    void register()
    {
        registerAsUsed();
    }

    [ContextMenu ("find lock")]
     void findLock() {


  

        spawnedLock = Instantiate(padlock, canvas3D.transform) as GameObject;

        spawnedLock.transform.position = maincam.ScreenToWorldPoint(Vckrs.centerOfScreen());
        spawnedLock.transform.localScale = Vector3.one * 7;

        takeItButton = Instantiate(buttonPrefab, canvas2D.transform) as GameObject;
        leaveItButton = Instantiate(buttonPrefab, canvas2D.transform) as GameObject;

        takeItButton.transform.position = Vckrs.screenRatioToPosition(0.75f, 0.3f);
        leaveItButton.transform.position = Vckrs.screenRatioToPosition(0.25f, 0.3f);

        takeItButton.GetComponent<DynamicLanguageTexts>().textID = 19;
        leaveItButton.GetComponent<DynamicLanguageTexts>().textID = 18;

        Button takeBut = takeItButton.GetComponent<Button>();
        Button leaveBut = leaveItButton.GetComponent<Button>();

        takeBut.onClick.AddListener(takeIt);
        leaveBut.onClick.AddListener(leaveIt);


    }

    public void takeIt()
    {
        Debug.Log("TAKE İT");

        Destroy(spawnedLock);
        Destroy(takeItButton);
        Destroy(leaveItButton);

        GameObject hang=player.transform.Find("Armature/Torso/Chest/Neck/lock").gameObject;
        hang.SetActive(true);

        Destroy(renc);
        Destroy(gameObject);

    }

    public void leaveIt()
    {
        Debug.Log("Leave it");

        Destroy(spawnedLock);
        Destroy(takeItButton);
        Destroy(leaveItButton);

        Destroy(renc);
        Destroy(gameObject);

    }

    public override void Action()
    {
        base.Action();
        Timing.RunCoroutine(reyhanLeavesKovalev());
    }

    public override void gameIsUsed()
    {
        base.gameIsUsed();
        deactivateController();
    }

}
