using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public GameObject DynamicButton;
    public GameObject momentsSubMenu;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void start()
    {
       SceneManager.LoadScene((int)GlobalController.Instance.fullGameSceneList[0]);
    }

    public void load()
    {

        GlobalController.Instance.LoadData();

        List<int> scenes = GlobalController.Instance.sceneList;
        Debug.Log("Load");

        for (int i = 0; i < 4; i++)
        {
            HideUnhideButton hub = transform.GetChild(i).GetComponent<HideUnhideButton>();
            if (!hub)   return;
            hub.deactivate();
        }

        //Destroy all dynamic buttons if they exist
        for (int i = 0; i < momentsSubMenu.transform.childCount; i++)
        {
            Destroy(momentsSubMenu.transform.GetChild(i).gameObject);

        }


        //Container of dynamic button
        float height = momentsSubMenu.GetComponent<RectTransform>().rect.height;
        float spaceBetweenButtons = height / (float)(scenes.Count + 1);

        for (int i = 0; i < scenes.Count; i++)
        {
            string button="";
          
            for (int j = 0; j < i+1; j++)
            {

                button += j.ToString();
                
            }

            var newButton = (Transform)Instantiate(DynamicButton, momentsSubMenu.transform);
            GameObject newButtonGO = newButton.gameObject;
            newButtonGO.SetActive(false);

            if (newButtonGO.GetComponent<RectTransform>().childCount == 0)
            {
                Debug.Log("Button doesn't have text");
                return;
            }

            //Set text
            Text buttonText = newButton.transform.GetChild(0).GetComponent<Text>();
            buttonText.text = button;

            //Set position
            RectTransform buttonTransform = newButtonGO.GetComponent<RectTransform>();
            buttonTransform.localPosition = new Vector2(0,spaceBetweenButtons/2+spaceBetweenButtons*(i+1)-height/2);

            //FadeIN
            HideUnhideButton hub = newButton.GetComponent<HideUnhideButton>();
            if(hub) hub.activate();

            Debug.Log(button);
        }

        //Return Button
        var returnButton = (Transform)Instantiate(DynamicButton, momentsSubMenu.transform);
        GameObject returnButtonGO = returnButton.gameObject;
        returnButtonGO.SetActive(false);
        returnButton.transform.GetChild(0).GetComponent<Text>().text="Return";
        RectTransform returnbuttonTransform = returnButtonGO.GetComponent<RectTransform>();
        returnbuttonTransform.localPosition = new Vector2(0, spaceBetweenButtons / 2 - height/2);
        returnButton.GetComponent<HideUnhideButton>().activate();

        //Adding event
        returnButtonGO.GetComponent<Button>().onClick.AddListener(delegate () { returnEvent(); });
    }

    public void returnEvent()
    {
        //Hide all dynamic buttons
        for(int i = 0; i < momentsSubMenu.transform.childCount; i++)
        {
            momentsSubMenu.transform.GetChild(i).GetComponent<HideUnhideButton>().deactivate();
            
        }


        for (int i = 0; i < 4; i++)
        {
            HideUnhideButton hub = transform.GetChild(i).GetComponent<HideUnhideButton>();
            if (!hub) return;
            hub.activate();
        }
    }

    public void exit()
    {
        Debug.Log("Quiting");
        Application.Quit();
    }

    public void loadLastLevel()
    {

    }
}
