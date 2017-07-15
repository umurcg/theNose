using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class DrunkManGameSceneController : GameController {

    public GameObject drunkManActor;
    public GameObject drunkManGame;
    

    [HideInInspector]
    Animator drunkManAnim;  
    UnityEngine.AI.NavMeshAgent drunkNMA;

    characterComponents dm;

    public float timeBetweenShows = 10f;
    public float timeShows = 4f;
    public float minDistanceToPlayer = 10f;
    //Timer showTimer;

    public GameObject candlePrefab;
    public GameObject allGameObjects;

    bool playerIsNear = false;

    Light pointLight;

    public GameObject[] gameObjects;

    IEnumerator<float> hideAndSeekHandler;


    //These positions are saved at encounter function. 
    //So when player lost game we wiil use them to restart game.
    [HideInInspector]
    public Vector3 kovalevEncPos, drunkManEncPos;

    // Use this for initialization
    public override void Start () {
        base.Start();

        drunkNMA = drunkManActor.GetComponent<UnityEngine.AI.NavMeshAgent>();
        //showTimer = new Timer(timeBetweenShows);

        pointLight = drunkManActor.GetComponentInChildren<Light>();
        drunkManAnim = drunkManActor.GetComponent<Animator>();
        drunkManActor.GetComponent<AlwaysLook>().aimObject = player;

        dm = new characterComponents(drunkManGame);

        hideAndSeekHandler = Timing.RunCoroutine(_hideAndSeek());
        //Timing.RunCoroutine(encounter());


    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator<float> _hideAndSeek()
    {
        yield return Timing.WaitForSeconds(timeBetweenShows);

        while (true)
        {
            Vector3 generatedPos=Vector3.zero;
            bool okayPos = false;

            while (!okayPos)
            {

                while (!Vckrs.generateRandomVisiblePosition(CharGameController.getCamera().GetComponent<Camera>(), "Floor", out generatedPos))
                {
                    //Debug.Log("Cant find position");
                    yield return 0;
                }

                //Debug.Log("Found pos");

                Vckrs.findNearestPositionOnNavMesh(generatedPos, drunkNMA.areaMask, 20f, out generatedPos);

                if (Vector3.Distance(drunkManActor.transform.position, player.transform.position) > minDistanceToPlayer) okayPos = true;

            }

            drunkManActor.transform.position = generatedPos;


            //Debug.Log("Fading in");

            drunkManActor.SetActive(true);

            IEnumerator<float> fadeHandler =  Timing.RunCoroutine(Vckrs._fadeObjectIn(drunkManActor, 1f, true));
            IEnumerator<float> lightHandler = Timing.RunCoroutine(Vckrs._changeLight(3f, pointLight, 5));

            
            yield return Timing.WaitUntilDone(fadeHandler);
            yield return Timing.WaitUntilDone(lightHandler);

            //yield return Timing.WaitForSeconds(timeShows);
            float timer = timeShows;
            while (timer >= 0&& playerIsNear)
            {
                timer -= Time.deltaTime;
                yield return 0;
            }

            fadeHandler = Timing.RunCoroutine(Vckrs._fadeObjectOut(drunkManActor, 1f, true));
            lightHandler=Timing.RunCoroutine(Vckrs._changeLight(3f, pointLight, 0));

            yield return Timing.WaitUntilDone(fadeHandler);
            yield return Timing.WaitUntilDone(lightHandler);

            drunkManActor.SetActive(false);

            yield return Timing.WaitForSeconds(timeBetweenShows);

            playerIsNear = false;

        }
    }


    public void playerCame()
    {
        playerIsNear = true;
        //Debug.Log("player is neaaaaaaaaar");
    }

    public void attack()
    {
        Timing.KillCoroutines(hideAndSeekHandler);
        Timing.RunCoroutine(_attack());
    }

    IEnumerator<float> _attack()
    {
        IEnumerator<float> fadeHandler = null;
        IEnumerator<float> lightHandler = null;

        if (drunkManActor.activeSelf)
        {
            fadeHandler = Timing.RunCoroutine(Vckrs._fadeObjectOut(drunkManActor, 1f, true));
            lightHandler = Timing.RunCoroutine(Vckrs._changeLight(3f, pointLight, 0));

            yield return Timing.WaitUntilDone(fadeHandler);
            yield return Timing.WaitUntilDone(lightHandler);

            drunkManActor.SetActive(false);
        }

        pcc.StopToWalk();

        Vector3 pos = player.transform.position - player.transform.forward * 0.2f;

        //Vckrs.testPosition(pos);

        drunkManActor.GetComponent<TriggerAnimationRepeatadly>().enabled = false;
        drunkManActor.SetActive(true);
        drunkManActor.transform.position = pos;
        

        fadeHandler = Timing.RunCoroutine(Vckrs._fadeObjectIn(drunkManActor, 1f, true));
        lightHandler = Timing.RunCoroutine(Vckrs._changeLight(3f, pointLight, 5));

        drunkManAnim.SetTrigger("hit");

        //yield return Timing.WaitUntilDone(fadeHandler);
        //yield return Timing.WaitUntilDone(lightHandler);

        while (!drunkManAnim.GetCurrentAnimatorStateInfo(0).IsName("hit") || drunkManAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f)
        {
            yield return 0;
        }

        blackScreen.script.setAsBlack();

        candlePrefab=Instantiate(candlePrefab,transform) as GameObject;
        candlePrefab.transform.position = player.transform.position + player.transform.right * 2;

        yield return Timing.WaitForSeconds(3f);

        playerAnim.SetTrigger("getUpFromFloor");
        playerAnim.speed = 0;

        drunkManActor.SetActive(false);

        //Lock doctor door
        OpenDoorLoad.getDoorSciptWithScene(GlobalController.Scenes.Doctor).Lock();
        OpenDoorLoad.getDoorSciptWithScene(GlobalController.Scenes.KovalevHouse).Lock();

        IEnumerator<float> bshandler= blackScreen.script.fadeIn();
        yield return Timing.WaitUntilDone(bshandler);

        yield return Timing.WaitForSeconds(2f);

        playerAnim.speed = 1;

        Timing.WaitForSeconds(2f);

        pcc.ContinueToWalk();

        dm.player.SetActive(true);
        allGameObjects.SetActive(true);
        GetComponent<ObjectSpawnerContinously>().enabled = true;

        yield break;
    }

    public void encounter()
    {
        Timing.RunCoroutine(_encounter());
    }

    IEnumerator<float> _encounter()
    {
        yield return 0;

        kovalevEncPos = player.transform.position;
        drunkManEncPos = dm.player.transform.position;
        
        pcc.StopToWalk();

        Timing.RunCoroutine(Vckrs._lookTo(player, dm.player, 1f));
        
        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "") yield return 0;

        List<GameObject> bottles = GetComponent<ObjectSpawnerContinously>().getSpawnedObjects();

        GameObject nearestBottle = Vckrs.findNearestObjectToPos(player.transform.position, bottles);

        //RockScript rs = nearestBottle.GetComponent<RockScript>();
        //rs.reciever = gameObject;

        CollectableObjectV2 colObj = nearestBottle.GetComponent<CollectableObjectV2>();

        colObj.cursorTexture = null;

        //IEnumerator<float> handler= Timing.RunCoroutine(CollectableObject.goAndCollectObject(playerNma, nearestBottle, Vector3.zero));
        IEnumerator<float> handler = Timing.RunCoroutine(colObj._goAndCollectObject(playerNma));
        yield return Timing.WaitUntilDone(handler);

        handler= Timing.RunCoroutine(Vckrs._lookTo(player, dm.player, 1f));
        yield return Timing.WaitUntilDone(handler);

        colObj.unCollect();

        ShootWithBottle swb = GetComponent<ShootWithBottle>();
        handler=Timing.RunCoroutine(swb.shoot(dm.player.transform.position, 30f,nearestBottle));
        yield return Timing.WaitUntilDone(handler);

        
        yield return Timing.WaitForSeconds(1.5f);

        Timing.RunCoroutine(startGame());

        yield break;
    }

    //public void damageEnemy()
    //{
    //    //Debug.Log("Damage enemt");
    //    Timing.RunCoroutine(startGame());

    //}
    
    IEnumerator<float> startGame()
    {
        sc.callSubtitleWithIndex(1);
        while (subtitle.text != "") yield return 0;

        GetComponent<ShootWithBottle>().enabled = true;
        GetComponent<DrunkManGameController>().enabled = true;

        dm.player.GetComponent<DrunkManAI>().enabled = true;
        dm.player.GetComponent<CanSeeYou>().enabled = true;
        
    
        yield break;
    }


    public override void activateController()
    {
        base.activateController();
        gameObject.SetActive(true);
        
        
    }

    public override void deactivateController()
    {
        base.deactivateController();
        gameObject.SetActive(false);
    }
}
