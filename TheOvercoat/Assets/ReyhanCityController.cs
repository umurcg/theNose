using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.AI;

public class ReyhanCityController : GameController {


    public bool debug = false;
    public HorseScript hs;
    public GameObject reyhanGameObject;

    //D'stance between characters when they sit
    public float sitPoisitonOffset;
    GameObject sitPosition;
    public GameObject girty;
    NavMeshAgent reyhanAgent;

    public override void Start()
    {
        base.Start();

        bool firstGC = GlobalController.Instance.isGameControllerIsUsedSceneNameAndGameObjectName("NewsPaperR.");
        bool secondGC = GlobalController.Instance.isGameControllerIsUsedSceneNameAndGameObjectName(generateIDWithEpisodeID());

        if(firstGC && !secondGC || debug)
        {
            activateController();
            Debug.Log("ACTIVATING REYHAN");
        }
        else
        {
            deactivateController();
        }

        //Change way point at each mount so characters wont sit top of each other.
        //It is a bad design but still it works ;)
        //sitPosition = hs.wayPoints[0].transform.GetChild(hs.wayPoints[0].transform.childCount - 1).gameObject;

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

        hs.gameObject.tag = "Untagged";

        hs.animationName = "Sit";

        //Change sit positiion object pos
        //sitPosition.transform.position = sitPosition.transform.position + sitPosition.transform.right * sitPoisitonOffset/5;

        hs.passenger = player;
        handlerHolder = hs.mount();

        yield return Timing.WaitUntilDone(handlerHolder);

        hs.setDes(reyhanGameObject.transform.position,false);

        sc.callSubtitleWithIndexTime(0);
        while (sc.getActiveController().enabled) yield return 0;

        yield return Timing.WaitUntilDone(Timing.RunCoroutine(Vckrs.waitUntilStop(hs.gameObject)));

        yield return Timing.WaitUntilDone(hs.unmount());

        hs.passenger = gameObject;

        hs.animationName = "SitPosition";

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

}
