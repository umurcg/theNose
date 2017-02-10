﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//City includes lots of scenes and games. So when this script starts it looks player's sceneList and decide how to initilize city.
public class CityGameController : MonoBehaviour {

    public GameObject berberShop;
    
	// Use this for initialization
	void Awake () {


        if (GlobalController.Instance != null)
        {
            //Scene list
            List<int> sceneList = GlobalController.Instance.sceneList;
            //Debug.Log("SceneList lenght is " + sceneList.Count);
            //foreach(int i in sceneList)
            //{
            //    Debug.Log(i);
            //}

            if (sceneList.Count > 0)
            {
                //Get last index of sceneList
                int lastSceneIndex = sceneList[sceneList.Count - 1];

                switch (lastSceneIndex)
                {
                    ////If player coming from main menu
                    //case (int)GlobalController.Scenes.MainMenu:
                    //    //Start with berber shop scene
                    //    berberShopScene();
                    //    break;
                    //default:
                    //    Debug.Log("Unused scene index");
                    //    break;

                }
            }
            else
            {
                //If sceneList is empty, it means user comes from main menu. So berber shop must be initilizesd
                //Debug.Log("Starting berber shop");
                berberShopScene();
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void berberShopScene()
    {
        CharGameController.deactivateAllCharacters();
        berberShop.SetActive(true);
    }
}