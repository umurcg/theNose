using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using MovementEffects;

public class MainMenu : MonoBehaviour {
    public GameObject DynamicButton;
    public GameObject momentsButton;
    public GameObject momentsSubMenu;
    public GameObject continueButton;
    public GameObject settingsButton, subsettingsMenu;
    public GameObject exitButton;
    public GameObject newgameButton;

    public GameObject[] mainMenuButtons;
    public TextAsset sceneDescriptions;

    static string momentsImageDirectory="Assets/Textures/MomentsImages/";

    //CharacterController cc;

    void Awake()
    {
        mainMenuButtons = new GameObject[] { continueButton, exitButton, settingsButton, momentsButton, newgameButton };
               
    }

    // Use this for initialization
    void Start () {

        ////Disable main player character controller
        //cc= CharGameController.getActiveCharacter().GetComponent<CharacterController>();
        //cc.enabled = false;

        //Debug.Log(Application.persistentDataPath);
        if (/*!GlobalController.Instance.LoadData()*/true) //For now it is always disabled.
        {



            //TODO Load data here
            //If loaded scene list is empty then there shouldn't be any moment, so disable the continue and moments button
            if (GlobalController.Instance.sceneList.Count == 0)
            {
                continueButton.GetComponent<Button>().interactable = false;
                momentsButton.GetComponent<Button>().interactable = false;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void startButton()
    {
        GlobalController.Instance.clearSceneList();
       SceneManager.LoadScene((int)GlobalController.Instance.fullGameSceneList[0]);
    }

    public void settings()
    {
        hideUnhideMainButtons(true);
        HideUnhideButton[] hubs = subsettingsMenu.GetComponentsInChildren< HideUnhideButton >();
        //Debug.Log(hubs.Length);
        foreach(HideUnhideButton hub in hubs)
        {
            hub.activate();
        }

    }

    //This functions loads data from data file and creates buttons for each episode that user explored so far.
    public void openMoments()
    {               
        //Get the scene list
        List<int> scenes = GlobalController.Instance.sceneList;
        Debug.Log("Load");

        //Hide main menu buttons
        hideUnhideMainButtons(true);

        //Destroy all dynamic buttons if they exist
        for (int i = 0; i < momentsSubMenu.transform.childCount; i++)
        {
            Destroy(momentsSubMenu.transform.GetChild(i).gameObject);

        }
        
        //Create dynamic buttons

        //Container of dynamic button
        float height = momentsSubMenu.GetComponent<RectTransform>().rect.height;
        float spaceBetweenButtons = height / (float)(scenes.Count + 1);

        for (int i = 0; i < scenes.Count; i++)
        {
            string button = "";

            for (int j = 0; j < i + 1; j++)
            {

                button += j.ToString();

            }

            var newButton = Instantiate(DynamicButton/*, momentsSubMenu.transform*/) as GameObject;
            newButton.transform.SetParent(momentsSubMenu.transform);

            newButton.SetActive(false);

            if (newButton.GetComponent<RectTransform>().childCount == 0)
            {
                Debug.Log("Button doesn't have text");
                return;
            }

            //Set image of button and text
            fillDynamicButton(newButton,i);

            ////Set text
            //Text buttonText = newButton.transform.GetChild(0).GetComponent<Text>();
            //buttonText.text = button;

            //Set position
            //RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
            //buttonTransform.localPosition = new Vector2(0,spaceBetweenButtons/2+spaceBetweenButtons*(i+1)-height/2);

            //FadeIN
            HideUnhideButton hub = newButton.GetComponent<HideUnhideButton>();
            if(hub) hub.activate();

            //Debug.Log(button);
        }

        //Create return Button
        var returnButton = Instantiate(DynamicButton/*, momentsSubMenu.transform*/) as GameObject;
        returnButton.transform.SetParent(momentsSubMenu.transform);
        GameObject returnButtonGO = returnButton.gameObject;
        returnButtonGO.SetActive(false);
        returnButton.transform.GetChild(0).GetComponent<Text>().text="Return";
        RectTransform returnbuttonTransform = returnButtonGO.GetComponent<RectTransform>();
        returnbuttonTransform.localPosition = new Vector2(0, spaceBetweenButtons / 2 - height/2);
        returnButton.GetComponent<HideUnhideButton>().activate();

        //Adding event to return button
        returnButtonGO.GetComponent<Button>().onClick.AddListener(delegate () { returnEvent(); });
    }

    //This loads a moments
    public void loadScene(GlobalController.Scenes scene, int episodeID)
    {
        int totalSceneListCount=GlobalController.Instance.sceneList.Count;
        int sceneInt = (int)scene;

        int numberOfSceneThatShouldBeRemoved = totalSceneListCount - episodeID;
        //Should set int scene list while user chooses the moment that he wants.
        Debug.Log("scene int "+episodeID+" numberOfSceneThatShouldBeRemoved "+ numberOfSceneThatShouldBeRemoved);
        for (int i = 0; i < numberOfSceneThatShouldBeRemoved; i++)
        {

            GlobalController.Instance.sceneList.RemoveAt(GlobalController.Instance.sceneList.Count-1);
        }


        Timing.RunCoroutine(_loadScene(scene));
    }

    public IEnumerator<float> _loadScene(GlobalController.Scenes scene)
    {
        IEnumerator<float> handler= Timing.RunCoroutine(blackScreen.script.fadeOut());
        yield return Timing.WaitUntilDone(handler);
        Debug.Log("Loading " + scene.ToString() + " with id of " + (int)scene);

        //Before loading active camera. pliiiiz

        CharGameController.getCamera().SetActive(true);


        SceneManager.LoadScene((int)scene);
    }

    public void returnEvent()
    {
        //Hide all dynamic buttons
        for(int i = 0; i < momentsSubMenu.transform.childCount; i++)
        {
            momentsSubMenu.transform.GetChild(i).GetComponent<HideUnhideButton>().deactivate();
            
        }

        hideUnhideMainButtons(false);
    }

    public void exit()
    {
        Debug.Log("Quiting");
        Application.Quit();
    }

    public void loadLastLevel()
    {
        GlobalController.Instance.LoadData();
        List<int> sceneList = GlobalController.Instance.sceneList;
        SceneManager.LoadScene(sceneList[sceneList.Count-1]);
    }

    void hideUnhideMainButtons(bool hide)
    {
            foreach(GameObject button in mainMenuButtons)
            {

                HideUnhideButton hub = button.GetComponent<HideUnhideButton>();
                if (!hub) return;
                if (hide)
                {
                    hub.deactivate();
                }else
                {
                    hub.activate();
                }
            
            }

    }

    //Fills dynamic button with image and changes its text to scene name
    //Also assigns listener for button
    void fillDynamicButton(GameObject button, int episodeID)
    {
        string[] descriptionLines = sceneDescriptions.text.Split('\n');

        
        Image buttonImage = button.GetComponent<Image>();
        Text buttonText = button.GetComponentInChildren<Text>();
        Sprite momentImage=(Sprite) AssetDatabase.LoadAssetAtPath(momentsImageDirectory + episodeID+".jpg", typeof(Sprite));
        if (!momentImage) Debug.Log("Couldn't find momentImage");
                
        buttonImage.sprite = momentImage;
        buttonText.text = descriptionLines[episodeID];

        button.GetComponent<Button>().onClick.AddListener(delegate () { loadScene((GlobalController.Scenes)GlobalController.Instance.sceneList[episodeID],episodeID); });

    }

    
    

    //void OnDisable()
    //{
    //    Debug.Log("On disable function");
    //    cc.enabled = true;
    //}
}
