using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//City includes lots of scenes and games. So when this script starts it looks player's sceneList and decide how to initilize city.
public class CityGameController : MonoBehaviour {

    public GameObject berberShop, Crowd, girty, bar, jokeGroup, bridge, fruitStad, cat;
    public GameObject[] ivanScenePolice;
    public GameObject lookAtMeNowTrigger, NoseGame;
    public GameObject SingerCafe;
    
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

                    case (int)GlobalController.Scenes.KovalevHouse:
                        Debug.Log("comingFromKoavlevHouse");
                        comingFromKovalevHouse();

                      

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
        }else
        {
            Debug.Log("No global controller instace!!!!!!!!!!!");
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void berberShopScene()
    {
        Debug.Log("Berber shop");
        CharGameController.deactivateAllCharacters();
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
        OpenDoorLoad.doors[1].playerCanOpen = true;

        Crowd.GetComponent<GameController>().activateController();
        //girty.GetComponent<GameController>().activateController();
        bar.GetComponent<GameController>().activateController();
        jokeGroup.GetComponent<GameController>().activateController();
        fruitStad.GetComponent<GameController>().activateController();
        bridge.GetComponent<GameController>().activateController();
        cat.GetComponent<GameController>().activateController();

        //Activae policemans
        foreach(GameObject police in ivanScenePolice)
        {
            police.SetActive(true);
        }

    }


    void comingFromKovalevHouse()
    {

        //Unlock police door and kovalev door
        OpenDoorLoad.doors[2].playerCanOpen = true;
        OpenDoorLoad.doors[3].playerCanOpen = true;

        //Set all cll characters for looking at kovalev
        GameObject characterObj = CharGameController.getActiveCharacter();
        GameObject spawnedTrigger=(GameObject)Instantiate(lookAtMeNowTrigger, characterObj.transform);
        spawnedTrigger.transform.localPosition = Vector3.zero;
        
        Timing.RunCoroutine(ActivateIn(NoseGame, 10f/*,characterObj*/));
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
}
