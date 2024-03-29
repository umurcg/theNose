﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.SceneManagement;

//This script deals with cheats
//For now cheats only avaible on editor mode but in future they maybe in build too ;)

public class CheatScript : MonoBehaviour {

    
    CharacterControllerKeyboard cck;
    BirdController bc;

    //Numbers
    KeyCode[] numbers = {
         KeyCode.Alpha0,
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
        };

    // Use this for initialization
    void Start () {
        //No character so disable cheat script
        //if (CharGameController.getActiveCharacter() == null)
        //{
        //    enabled = false;
        //    return;
        //}
        updateMembers();

	}
	
	// Update is called once per frame
	void Update () {

        if (!Debug.isDebugBuild) Destroy(this);

        //if (Application.isEditor)
        //{
            if (Input.GetKeyDown(KeyCode.T)) Timing.RunCoroutine(setScaleDuringPlay());
            if (Input.GetKeyDown(KeyCode.C)) Timing.RunCoroutine(setSpeedDuringPlay());
            if (Input.GetKeyDown(KeyCode.N)) loadNextLevel();
            if (Input.GetKeyDown(KeyCode.J)) takeScreenShot();
            if (Input.GetKeyDown(KeyCode.O)) changeCameraType();
            if (Input.GetKeyDown(KeyCode.R)) continueToWalk();
            if (Input.GetKeyDown(KeyCode.K)) openAllDoors();

    }


    void changeCameraType()
    {
        if (CharGameController.getCameraType() == CharGameController.cameraType.Ortographic)
        {

            CharGameController.setCameraType(CharGameController.cameraType.Perspective);
        }
        else
        {
            CharGameController.setCameraType(CharGameController.cameraType.Ortographic);
        }

    }

    void loadNextLevel()
    {

        //Debug.Log("Laoding next level");
        //There is exception at one scene where ivan throws nose. In that scene, scene actualyy is not changed but it fades out and fades in so
        //This if condition checks that player is in that scene and calls a specific function making effect like scene change
        if (GlobalController.getPreviousScene()==GlobalController.Scenes.IvanHouse)
        {
            //Debug.Log("Previous scene is ivanhous");
            //Bridge game controller have owner objects having floor tag so we find it with that tag
            GameObject[] flootObjects=GameObject.FindGameObjectsWithTag("Floor");
            BridgeGameController bgc=null;
            foreach (GameObject floorObj in flootObjects)
            {
                BridgeGameController localBgc=floorObj.GetComponent<BridgeGameController>();
                if (localBgc) bgc = localBgc;
            }

            

            if (bgc) {
                Debug.Log("Triggering change scen from bgc");
                Timing.RunCoroutine(bgc.GetComponent<BridgeGameController>()._changeScene());
                return;
            }else
            {
                Debug.Log("Couldny found bgc");
            }

            
        }

        //Debug.Log("door count is "+OpenDoorLoad.doors.Count);

        if (OpenDoorLoad.doors.Count == 1)
        {
            //Debug.Log("Door count is 1");
  

            Dictionary<int, OpenDoorLoad> doors = OpenDoorLoad.doors;
            foreach (KeyValuePair<int, OpenDoorLoad> door in doors)
            {
                //Debug.Log("Door name is " + door.Value.Scene);
                door.Value.debugLoad=true;
                return;
            }
        }
        else  if (OpenDoorLoad.numberOfActiveDoors() > 0)
            {
                //Debug.Log("Active door count is bigger then 0");
                GlobalController.Scenes nextScene= GlobalController.Instance.getNextScene();

                List<OpenDoorLoad> activeDoors = OpenDoorLoad.getAllActiveDoors();

                foreach (OpenDoorLoad door in activeDoors) {
                if (door.Scene == nextScene)
                {
                    door.debugLoad = true;
                    return;
                }

                }

            //Debug.Log("But none of them is next scene so just loading next scene");
            GlobalController.Instance.loadNextScene();

        } else{

            Debug.Log("Loading next scene directly");
            GlobalController.Instance.loadNextScene();
        }
    }

    void OnEnable()
    {
        //Tell our 'registerToSceneList' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += updateMembers;
    }

    void OnDisable()
    {
        //Tell our 'registerToSceneList' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= updateMembers;
    }

    //This function register creates scene list if it is not exist.
    //After that it registers scene to sceneList
    public void updateMembers(Scene scene, LoadSceneMode mode)
    {
        updateMembers();


    }

    public void updateMembers()
    {
        GameObject player = CharGameController.getActiveCharacter();
        if (player)
        {
            cck = player.GetComponent<CharacterControllerKeyboard>();
            bc = player.GetComponent<BirdController>();
        }
    }

    //This enables user to set time scale. It is for debugging. But also it can be a cheat in futuer;)
    IEnumerator<float> setScaleDuringPlay()
    {
        yield return 0;

        //Wait for input
        while (Input.anyKeyDown == false)
        {
            //Debug.Log("Waiting for a number");
            yield return 0;
        }

        float timeScale = 0;

        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKey(numbers[i]))
            {
                //Debug.Log("You pressed a number which is" + numbers[i].ToString());
                timeScale += 10 * (i );

            }
        }

        //Wait for one frame 
        yield return 0;

        //Wait for input
        while (Input.anyKeyDown == false)
        {
            yield return 0;
            //Debug.Log("Waiting for a number");
        }


        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKey(numbers[i]))
            {
                timeScale += (i);
                //Debug.Log("You pressed a number which is" + numbers[i].ToString());
            }

        }

        //Debug.Log("Your time scale is " + timeScale);
        if (timeScale > 0) Time.timeScale = timeScale;

        yield break;
    }
    

    //This enables user to set speed. It is for debugging. But also it can be a cheat in futuer;)
    IEnumerator<float> setSpeedDuringPlay()
    {
        yield return 0;

        //Wait for input
        while (Input.anyKeyDown == false)
        {
            //Debug.Log("Waiting for a number");
            yield return 0;
        }

        float localSpeed = 0;



        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKey(numbers[i]))
            {
                //Debug.Log("You pressed a number which is" + numbers[i].ToString());
                localSpeed += 10 * (i );


            }
        }


        //Wait for one frame 
        yield return 0;

        //Wait for input
        while (Input.anyKeyDown == false)
        {
            yield return 0;
            //Debug.Log("Waiting for a number");
        }


        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKey(numbers[i]))
            {
                localSpeed += (i );
                //Debug.Log("You pressed a number which is" + numbers[i].ToString());
            }

        }

        //Debug.Log("Your speed is " + localSpeed);
        if (localSpeed > 0)
        {
            if (cck)
            {
                cck.speed = localSpeed;
            }
            else if(bc) {
                bc.speed = localSpeed;
            }
        }

        yield break;
    }

    void takeScreenShot()
    {

        string directory = "_"+System.DateTime.Now.ToString() + ".jpg";
        ScreenCapture.CaptureScreenshot(directory,4);
        Debug.Log("Screen shot is saved to " + directory);
    }

    void continueToWalk()
    {
        CharGameController.getActiveCharacter().GetComponent<PlayerComponentController>().ContinueToWalk();
    }

    void openAllDoors()
    {
        OpenDoorLoad.openAllDoors();
    }

}
