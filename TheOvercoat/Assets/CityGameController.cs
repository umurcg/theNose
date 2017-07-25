﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//City includes lots of scenes and games. So when this script starts it looks player's sceneList and decide how to initilize city.
public class CityGameController : MonoBehaviour {
    public GameObject berberShop, Crowd, bar, jokeGroup, bridge, fruitStad,butcher,cat, ivanCityController, maze;
    public Cancanfly ccf;
    public DenizEfeGameController degc;
    public DrunkManGameSceneController dmgsc;
    public Sevval sev;
    public GameObject[] exclusiceObjectsForIvanScene;
    public GameObject lookAtMeNowTrigger, NoseGame;
    public GameObject SingerCafe;
    public GameObject friendTellsChurch;
    public GameObject churchBirdPosition;
    public GameObject outroScene;
    public GameObject deadManGameController;
    public GameObject trailer;
    public GameObject horseCariers;
    public GameObject horseBots;
    public FogGameController fogGAME;
    public GameObject streetAreas;
    //public AudioClip innerSpeechMusic;
    public TutorailCanvas tutorial;
    public Terrain terrain;
    public float baseMapDistance = 3000;
    
    

	// Awake function is for registering current scene. It sets scene according to storyline
	void Awake () {

        if (trailer.activeSelf)
        {
            enabled = false;
            return;
        }


        terrain.basemapDistance = baseMapDistance;

        if (GlobalController.Instance != null)
        {
            //Scene list
            List<int> sceneList = GlobalController.Instance.sceneList;
            //Debug.Log("SceneList lenght is " + sceneList.Count);
            //foreach (int i in sceneList)
            //{
            //    Debug.Log(i);
            //}


            if (CharGameController.cgc != null)
            {
                //In city player hould have enabled bird eye view script. so enable it you fucking idiot
                BirdsEyeView bev = CharGameController.getCamera().GetComponent<BirdsEyeView>();
                bev.enabled = true;

            }

            if (checkSideStories()) return;

            if (sceneList.Count > 1)
            {
                //Get last index of sceneList
                int lastSceneIndex = sceneList[sceneList.Count - 1];

                //Debug.Log("last scene is " + (GlobalController.Scenes)lastSceneIndex);

                //If last scene is city then make last scene previous scene
                if (lastSceneIndex == (int)GlobalController.Scenes.City)
                {
                

                   lastSceneIndex= sceneList[sceneList.Count - 2];
                }


                switch (lastSceneIndex)
                {
                    //If player coming from main menu
                    case (int)GlobalController.Scenes.MainMenu:
                        Debug.Log("It is impossible. How can you come from main menu?");
                        break;
                    case (int)GlobalController.Scenes.IvanHouse:
                        comingFromIvanHouse();
                        
                        break;

                    case (int)GlobalController.Scenes.KovalevHouse:
                        Debug.Log("comingFromKoavlevHouse");
                        comingFromKovalevHouse();
                                              
                        break;

                    case (int)GlobalController.Scenes.Doctor:
                        comingFromDoctor();
                        break;
                    
                    case (int)GlobalController.Scenes.Church:

                        comingFromChurch();
                        break;

                    case (int)GlobalController.Scenes.Newspaper:
                        //Debug.Log("comingfrom newspaper");
                        comingFromNewspaper();

                        break;

                    case (int)GlobalController.Scenes.PoliceStation:
                        Debug.Log("comingfrom police station");
                        comingFromPoliceStation();

                        break;


                    case (int)GlobalController.Scenes.City:
                        
                        //Look for previous scene, if it is ivan house than it must be call singerCafeScene
                        if (sceneList.Count > 1 && sceneList[sceneList.Count - 2] == (int)GlobalController.Scenes.IvanHouse)
                            singerCafeScene();

                        break;

                    default:
                        Debug.Log("Unused scene index. Index is "+lastSceneIndex);
                        break;

                }
            }
            else
            {
                //If sceneList is empty, it means user comes from main menu. So berber shop must be initilizesd
                //Debug.Log("Starting berber shop");
                berberShopScene();
            }

            //Games only can be activated with registered game controllers
            if (GlobalController.Instance.usedGameControllers.Count > 0)
            {
                if (GlobalController.Instance.isGameControllerIsUsedSceneNameAndGameObjectName("PoliceStationPrisoner_Mahkum")
                    && !GlobalController.Instance.isGameControllerIsUsedSceneNameAndGameObjectName("PrisonerGameDeadMan")
                    )
                {

                    Debug.Log("Prisoner Gmae");
                    //Activate hut 
                    OpenDoorLoad prsionerDoor=OpenDoorLoad.getDoorSciptWithScene(GlobalController.Scenes.PrisonerGame);
                    prsionerDoor.Unlock();
                }
            }

        }else
        {
            Debug.Log("No global controller instace!!!!!!!!!!!");
        }


        GameObject player = CharGameController.getActiveCharacter();

        //For side missions that can Kovalev play 
        if (player!=null && player.name == "Kovalev") activateKovalevStories();


    }

    void OnDestroy()
    {
        //On any other scenes bird eye view should be disabled
        BirdsEyeView bev = CharGameController.getCamera().GetComponent<BirdsEyeView>();
        bev.enabled = false;
    }



	// Update is called once per frame
	void Update () {
	
	}

    void berberShopScene()
    {
        
        Debug.Log("Berber shop");
        CharGameController.deactivateAllCharacters();
        CharGameController.deactivateCamera();
        berberShop.GetComponent<GameController>().isDisabledAtStart = false;
        berberShop.GetComponent<GameController>().activateController();
    }

    void comingFromIvanHouse()
    {
        //Debug.Log("Coming from ivan house");

        //Debug.Log("Coming From Ivan house");
        //Crowd.GetComponent<GameController>().isDisabledAtStart = false;
        //girty.GetComponent<GameController>().isDisabledAtStart = false;
        //bar.GetComponent<GameController>().isDisabledAtStart = false;
        //jokeGroup.GetComponent<GameController>().isDisabledAtStart = false;
        //fruitStad.GetComponent<GameController>().isDisabledAtStart = false;
        //bridge.GetComponent<GameController>().isDisabledAtStart = false;
        //cat.GetComponent<GameController>().isDisabledAtStart = false;

        //Unlock Ivan home door
        OpenDoorLoad.doors[1].Unlock();

        ////Set active character to Ivan just in case
        CharGameController.setCharacter("Ivan");

        ivanCityController.GetComponent<GameController>().activateController();
        Crowd.GetComponent<GameController>().activateController();
        //girty.GetComponent<GameController>().activateController();
        bar.GetComponent<GameController>().activateController();
        jokeGroup.GetComponent<GameController>().activateController();
        fruitStad.GetComponent<GameController>().activateController();
        butcher.tag = "ActiveObject";
        bridge.GetComponent<GameController>().activateController();
        cat.GetComponent<GameController>().activateController();

        CharGameController.getObjectOfHand("nosePackage", CharGameController.hand.LeftHand).SetActive(true);

        //Set ivan animator if you cheat
        GameObject player = CharGameController.getActiveCharacter();
        player.GetComponent<Animator>().SetTrigger("GetOffContinue");
        player.GetComponent<Animator>().SetBool("GetOffBed", false);
        //player.GetComponent<PlayerComponentController>().ContinueToWalk();

        horseCariers.SetActive(false);

        //Activae policemans
        foreach(GameObject police in exclusiceObjectsForIvanScene)
        {
            police.SetActive(true);
        }

        //Map tutorial
        tutorial.startTutorial(TutorailCanvas.Tutorials.Map);

    }


    void comingFromKovalevHouse()
    {
        CharGameController.setCharacter("Kovalev");
        CameraFollower cf = CharGameController.getCamera().GetComponent<CameraFollower>();
        cf.updateTarget();
        cf.fixRelativeToDefault();

        //If scene list contains church then it means kovalev must go to doctor else it should meet with nose
        if (GlobalController.isScnListContains(GlobalController.Scenes.Church))
        {
            Debug.Log("Going doctor");

            dmgsc.activateController();

            OpenDoorLoad doctoDoor=OpenDoorLoad.getDoorSciptWithScene(GlobalController.Scenes.Doctor);
            OpenDoorLoad kovaevDoor= OpenDoorLoad.getDoorSciptWithScene(GlobalController.Scenes.KovalevHouse);

            doctoDoor.Unlock();
            kovaevDoor.Unlock();


            //CharGameController.coverKovalevsFace();
            //activate nose in hand
            CharGameController.getHand(CharGameController.hand.RightHand).transform.GetChild(1).gameObject.SetActive(true);

            clearStreets();

        }
        else
        {
            //Unlock police door and kovalev door
            OpenDoorLoad.doors[2].Unlock();
            OpenDoorLoad.doors[3].Unlock();

            //Kovalev should cover his hands
            CharGameController.coverKovalevsFace();

            //Set all cll characters for looking at kovalev
            GameObject characterObj = CharGameController.getActiveCharacter();
            GameObject spawnedTrigger = (GameObject)Instantiate(lookAtMeNowTrigger, characterObj.transform);
            spawnedTrigger.transform.localPosition = Vector3.zero;


            if(NoseGame!=null)
            Timing.RunCoroutine(ActivateIn(NoseGame, 10f/*,characterObj*/));
        }
    }

    void comingFromPoliceStation()
    {
        //Unlock police door and kovalev door
        OpenDoorLoad.doors[2].Unlock();
        OpenDoorLoad.doors[3].Unlock();
        OpenDoorLoad.doors[4].Unlock();

        //Force kovalev to cover his face
        CharGameController.coverKovalevsFace();
    }

    void comingFromNewspaper()
    {
        //Girty handles its activation itself.

        //Unlock newspaper home door
        OpenDoorLoad.doors[2].Unlock();
        OpenDoorLoad.doors[3].Unlock();
        OpenDoorLoad.doors[4].Unlock();

        //If coming from newspaper second time that player gave girty to newspaper 
        //So lets instantiate churchteller
        if (GlobalController.countSceneInList(GlobalController.Scenes.Newspaper) == 2)
        {
            //GameObject spawnedCT = Instantiate(friendTellsChurch);
            //GameObject player = CharGameController.getActiveCharacter();
            //spawnedCT.transform.position = player.transform.position + player.transform.forward * 5;

            ////Set subtitles of scs of churchteller
            ////Set char subtitles
            //SubtitleController[] scs = spawnedCT.GetComponents<SubtitleController>();
            //foreach (SubtitleController sc in scs)
            //{
            //    sc.setCharSubtitle();
            //}

            ////Activate game controller scripot
            //spawnedCT.GetComponent<ChurchTellerGameController>().enabled = true;

            //Unlock church door

            Debug.Log("Coming from newspaper second time");

            friendTellsChurch.GetComponent<GameController>().isDisabledAtStart = false;
            friendTellsChurch.GetComponent<GameController>().activateController();  

            OpenDoorLoad.doors[5].Unlock();

        }

    }

    void comingFromChurch()
    {
        Debug.Log("Coming from church");
        //OpenDoorLoad.openAllTheVisitedDoors();

        //If active character is bird then set its position to churchTop
        GameObject player = CharGameController.getActiveCharacter();
        if (player.transform.name == "Bird")
        {
            Timing.RunCoroutine(moveBirdAfterOneFrame());

            //////CameraFollower cf = CharGameController.getCamera().GetComponent<CameraFollower>();
            //////cf.updateRelative();

            maze.GetComponent<GameController>().activateController();

            //Set bird as landed
            BirdLandingScript bls = player.GetComponent<BirdLandingScript>();
            bls.setAsLandedBaked(true);

            tutorial.startTutorial(TutorailCanvas.Tutorials.Bird);

            horseCariers.SetActive(false);

        }

        //Enable fog game
        fogGAME.activateController();

    }

    IEnumerator<float> moveBirdAfterOneFrame()
    {
        yield return 0;
        CharGameController.movePlayer(churchBirdPosition.transform.position);
    }

    void comingFromDoctor()
    {

        //If kovalev talked with doctor
        if (/*GlobalController.Instance.isGameControllerIsUsedSceneNameAndGameObjectName("DoctorGameController")*/true) {
            Debug.Log("Coming from doctor");

            //Deactivate player and camera
            CharGameController.deactivateAllCharacters();
            CharGameController.deactivateCamera();

            //Disable enter scene controller script. Dont call deactivate method because it also deactivates kovalev camera and ivan which we need;
            //For that we first call activate method enterSceneGameController and then disable it
            EnterSceneGameController esgc = berberShop.GetComponent<EnterSceneGameController>();
            esgc.activateController();
            esgc.enabled = false;

            //Activate outro scene
            outroScene.GetComponent<OutroGameController>().activateController();

            Crowd.GetComponent<GameController>().activateController();
        }
        else
        {
            OpenDoorLoad.getDoorSciptWithScene(GlobalController.Scenes.Doctor).Unlock();
            OpenDoorLoad.getDoorSciptWithScene(GlobalController.Scenes.KovalevHouse).Unlock();
        }
    }

    void singerCafeScene()
    {
        Debug.Log("Singer cafe");
        SingerCafe.GetComponent<GameController>().activateController();
    }

    IEnumerator<float> ActivateIn(GameObject obj, float delay/*, GameObject player*/)
    {
        yield return Timing.WaitForSeconds(delay);
        //obj.transform.position = player.transform.position+player.transform.forward*40;
        obj.SetActive(true);
        yield break;

    }
    

    void foundDeadman()
    {
 
        CharGameController.getSun().GetComponent<DayAndNightCycle>().makeNight();

        clearStreets();

        SculpturerCatchesKovalev sck = deadManGameController.GetComponent<SculpturerCatchesKovalev>();
        sck.activateController();

        Timing.RunCoroutine(sck.callFirstSubtitle());
    }

    //This function checks for gamecontrollers. It is called before checking last scenes. So if you have a gamecontroller
    //that must interrupt normal story check in here
    //If it finds a game controller returns true else returns false
    bool checkSideStories()
    {
        if (GlobalController.Instance.isGameControllerIsUsedSceneNameAndGameObjectName("PrisonerGameDeadMan") && !GlobalController.Instance.isGameControllerIsUsedSceneNameAndGameObjectName("AtolyeSculpturerGame"))
        {
            foundDeadman();
            OpenDoorLoad.getDoorSciptWithScene(GlobalController.Scenes.KovalevHouse).Unlock();
            return true;
        }

        return false;   
    }
 

    void activateKovalevStories()
    {
        bool isNight = CharGameController.getSun().GetComponent<DayAndNightCycle>().isNight;

        if (isNight)
        {
            //Ali game
        }
        else
        {

            sev.activateController();
            degc.activateController();
            ccf.activateController();
            //Reyhan game
        }
    }

    void clearStreets()
    {
        //Reduce to spawnedObjects to 20
        SpawnBotsOnNavMeshRandomly[] spawnScripts = streetAreas.GetComponents<SpawnBotsOnNavMeshRandomly>();
        foreach (SpawnBotsOnNavMeshRandomly script in spawnScripts)
        {
            script.destroyAllBots();
        }

        horseCariers.SetActive(false);
        if (horseBots != null) horseBots.SetActive(false);
    }

}
