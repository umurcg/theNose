using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//City includes lots of scenes and games. So when this script starts it looks player's sceneList and decide how to initilize city.
public class CityGameController : MonoBehaviour {

    public GameObject berberShop, Crowd, girty, bar, jokeGroup, bridge, fruitStad, cat;
    
	// Use this for initialization
	void Awake () {


        if (GlobalController.Instance != null)
        {
            //Scene list
            List<int> sceneList = GlobalController.Instance.sceneList;
            Debug.Log("SceneList lenght is " + sceneList.Count);
            foreach (int i in sceneList)
            {
                Debug.Log(i);
            }

            if (sceneList.Count > 0)
            {
                //Get last index of sceneList
                int lastSceneIndex = sceneList[sceneList.Count - 1];

                switch (lastSceneIndex)
                {
                    //If player coming from main menu
                    case (int)GlobalController.Scenes.MainMenu:
                        Debug.Log("It is impossible. How can you come from main menu?");
                        break;
                    case (int)GlobalController.Scenes.IvanHouse:
                        comingFromIvanHouse();

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
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void berberShopScene()
    {
        //Debug.Log("Berber shop");
        CharGameController.deactivateAllCharacters();
        berberShop.GetComponent<GameController>().isDisabledAtStart = false;
        //berberShop.GetComponent<GameController>().activateController();
    }

    void comingFromIvanHouse()
    {

        //Debug.Log("Coming From Ivan house");
        //Crowd.GetComponent<GameController>().isDisabledAtStart = false;
        //girty.GetComponent<GameController>().isDisabledAtStart = false;
        //bar.GetComponent<GameController>().isDisabledAtStart = false;
        //jokeGroup.GetComponent<GameController>().isDisabledAtStart = false;
        //fruitStad.GetComponent<GameController>().isDisabledAtStart = false;
        //bridge.GetComponent<GameController>().isDisabledAtStart = false;
        //cat.GetComponent<GameController>().isDisabledAtStart = false;

        Crowd.GetComponent<GameController>().activateController();
        girty.GetComponent<GameController>().activateController();
        bar.GetComponent<GameController>().activateController();
        jokeGroup.GetComponent<GameController>().activateController();
        fruitStad.GetComponent<GameController>().activateController();
        bridge.GetComponent<GameController>().activateController();
        cat.GetComponent<GameController>().activateController();

    }
}
