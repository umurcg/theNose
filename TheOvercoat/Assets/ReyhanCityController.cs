using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class ReyhanCityController : GameController {


    public bool debug = false;
    public HorseScript hs;
    public GameObject reyhanGameObject;

    //D'stance between characters when they sit
    public float sitPoisitonOffset;
    GameObject sitPosition;

    public override void Start()
    {
        base.Start();

        bool firstGC = GlobalController.Instance.isGameControllerIsUsedSceneNameAndGameObjectName("NewsPaperreyhan");
        bool secondGC = GlobalController.Instance.isGameControllerIsUsedSceneNameAndGameObjectName(generateIDWithEpisodeID());

        if(firstGC && !secondGC || debug)
        {
            activateController();
        }
        else
        {
            deactivateController();
        }

        //Change way point at each mount so characters wont sit top of each other.
        //It is a bad design but still it works ;)
        sitPosition = hs.wayPoints.transform.GetChild(hs.wayPoints.transform.childCount - 1).gameObject;

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
        sitPosition.transform.position = sitPosition.transform.position - sitPosition.transform.right * sitPoisitonOffset/2;

        hs.animationName = "SitPosition";

        hs.isPassengerPlayer = false;
        handlerHolder= hs.mount();

        yield return Timing.WaitUntilDone(handlerHolder);


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
        sitPosition.transform.position = sitPosition.transform.position + sitPosition.transform.right * sitPoisitonOffset/5;

        hs.passenger = player;
        handlerHolder = hs.mount();

        yield return Timing.WaitUntilDone(handlerHolder);

        hs.setDes(reyhanGameObject.transform.position);

        sc.callSubtitleWithIndexTime(0);
        while (sc.getActiveController().enabled) yield return 0;

        yield break;

    }


    // Update is called once per frame
    void Update () {
	
	}

    public override void deactivateController()
    {
        base.deactivateController();
        gameObject.SetActive(false);
    }

    public override void activateController()
    {
        base.activateController();
        gameObject.SetActive(true);

    }

}
