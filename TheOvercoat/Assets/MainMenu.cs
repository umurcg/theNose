using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

using MovementEffects;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour {
    public GameObject DynamicButton;
    public GameObject momentsButton;
    public GameObject momentsSubMenu;
    public GameObject continueButton;
    public GameObject settingsButton, subsettingsMenu;
    public GameObject exitButton;
    public GameObject newgameButton;

    public GameObject menuCamera;

    public GameObject[] mainMenuButtons;
    public TextAsset sceneDescriptions;
    public TextAsset sceneCharacters;

    static string momentsImageDirectory="MomentsImages/";

    public GameObject askPrompt;

    public GameObject cameraTypePrompt;
    public TextAsset continuePropmtText;
 
    GameObject spawnedAskPrompt;

    //CharacterController cc;

    void Awake()
    {
        mainMenuButtons = new GameObject[] { continueButton, exitButton, settingsButton, /*momentsButton,*/ newgameButton };
               
    }

    // Use this for initialization
    void Start () {
        //TextAsset asdasdasd = Resources.Load("Text") as TextAsset;
        //if (asdasdasd) { Debug.Log("Found text"); } else { Debug.Log("No text"); }

        ////Disable main player character controller
        //cc= CharGameController.getActiveCharacter().GetComponent<CharacterController>();
        //cc.enabled = false;

        //Debug.Log(Application.persistentDataPath);
        if (/*!GlobalController.Instance.LoadData() */true) //For now it is always disabled.
        {

            //Debug.Log("Loaaaaaaaaaaaading");

            //TODO Load data here
            //If loaded scene list is empty then there shouldn't be any moment, so disable the continue and moments button
            //if (GlobalController.Instance.sceneList.Count == 0)
            //{
            //    continueButton.GetComponent<Button>().interactable = false;
            //    momentsButton.GetComponent<Button>().interactable = false;
            //}


            if (!GlobalController.Instance.isSaveDataAvaible())
            {
                continueButton.GetComponent<Button>().interactable = false;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {


    }

    public void startButton()
    {
        if (GlobalController.Instance.isSaveDataAvaible())
        {
            //ask overriding data
            spawnedAskPrompt = Instantiate(askPrompt);
            spawnedAskPrompt.transform.parent = transform;
            spawnedAskPrompt.transform.position = Vckrs.centerOfScreen();
            EnableDisableUI edui = spawnedAskPrompt.GetComponent<EnableDisableUI>();
            edui.activate();

            AskPrompt prompt = spawnedAskPrompt.GetComponent<AskPrompt>();
            prompt.setPromptText(Vckrs.getStringAccordingToLanguage((Language)GlobalController.Instance.getLangueSetting(), continuePropmtText));

            prompt.assignYesFunctionalities(new UnityAction[] { activateCameraPrompt });
            prompt.assignNoFunctionalities(new UnityAction[] { unhideMainButtons });

            hideMainButtons();

            //prompt.yesButton.GetComponent<Button>().onClick.AddListener(newGame);


        }
        else
        {
            newGame();
        }



    }

    void activateCameraPrompt()
    {
        
        cameraTypePrompt.SetActive(true);
        Destroy(spawnedAskPrompt);
        CharGameController.getActiveCharacter().SetActive(false);
        
    }
    

    public void newGame()
    {

        GlobalController.Instance.clearSceneList();
        SceneManager.LoadScene((int)GlobalController.Instance.fullGameSceneList[0]);
    }

    public void settings()
    {
        hideUnhideMainButtons(true);
        subsettingsMenu.GetComponent<EnableDisableUI>().activate();

        //HideUnhideButton[] hubs = subsettingsMenu.GetComponentsInChildren< HideUnhideButton >();
        ////Debug.Log(hubs.Length);
        //foreach(HideUnhideButton hub in hubs)
        //{
        //    hub.activate();
        //}

    }

    //This functions loads data from data file and creates buttons for each episode that user explored so far.
    public void openMoments()
    {               
        //Get the scene list
        List<int> scenes = GlobalController.Instance.maxSceneList;
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

            //if (newButton.GetComponent<RectTransform>().childCount == 0)
            //{
            //    Debug.Log("Button doesn't have text");
            //    return;
            //}

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
        //Debug.Log("scene int "+episodeID+" numberOfSceneThatShouldBeRemoved "+ numberOfSceneThatShouldBeRemoved);
        //Debug.Log("Cleaning list");
        for (int i = 0; i < numberOfSceneThatShouldBeRemoved; i++)
        {
            //Debug.Log("Removing " + GlobalController.Instance.sceneList[GlobalController.Instance.sceneList.Count - 1]);
            GlobalController.Instance.sceneList.RemoveAt(GlobalController.Instance.sceneList.Count-1);
        }


        Timing.RunCoroutine(_loadScene(scene, episodeID));
    }

    
    public IEnumerator<float> _loadScene(GlobalController.Scenes scene, int episodeID)
    {
        IEnumerator<float> handler= Timing.RunCoroutine(blackScreen.script.fadeOut());
        yield return Timing.WaitUntilDone(handler);
        Debug.Log("Loading " + scene.ToString() + " with id of " + (int)scene);

        //Before loading active camera. pliiiiz

        menuCamera.SetActive(false);
        CharGameController.getCamera().SetActive(true);

        //string[] characters = sceneCharacters.text.Split('\n');
        //string characterToActivate = characters[episodeID];
        //if (characterToActivate != "Null")
        //{
        //    Debug.Log("Character top activate " + characterToActivate);
        //    CharGameController.setCharacter(characterToActivate.Trim());


        //    CharGameController.getCamera().GetComponent<CameraFollower>().updateTarget();
        //    CharGameController.getCamera().GetComponent<CameraFollower>().fixRelativeToDefault();
        //}

        //Trim game controllers that are after current episodeID
        //It should be called while laoding a episode from moments menu. So commenting this right now. Call this in the function that loads scenes from moments
        //trimGameControllers(episodeID);


        SceneManager.LoadScene((int)scene);
    }


    //Trims game controllers that are not part of episodes that are played before this episode.
    //It must called while loading game from moments.
    void trimGameControllers(int episodeID)
    {

        Debug.Log("Game controllers before trimm");
        //For debug
        foreach (string gc in GlobalController.Instance.usedGameControllers)
        {
            Debug.Log(gc);
        }


        //string episodeIDString = episodeID.ToString();

        List<string> gcToRemove = new List<string>();

        foreach (string gc in GlobalController.Instance.usedGameControllers)
        {
            int index = gc.IndexOf("_");
            Debug.Log("index is " + index);

            Debug.Log(gc.Substring(index+1, gc.Length - (index+1)));
            int gcEpisodeID = int.Parse(gc.Substring(index + 1, gc.Length - (index + 1)));

            if (gcEpisodeID > episodeID)
            {
                Debug.Log("GC id is " + gcEpisodeID + " episod ıd is " + episodeID + " so removing it");
                gcToRemove.Add(gc);
            }
        }

        foreach (string gc in gcToRemove)
        {
            GlobalController.Instance.usedGameControllers.Remove(gc);
        }

        Debug.Log("Game controllers after trimm");
        //For debug
        foreach (string gc in GlobalController.Instance.usedGameControllers)
        {
            Debug.Log(gc);
        }

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

    public void returnFromSettings()
    {

        subsettingsMenu.GetComponent<EnableDisableUI>().deactivate();
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

        //Trim last index of sceneList because we are loading that scene and it shouldn't be registered right now
        int sceneToLoad = sceneList[sceneList.Count - 1];
        int episodeID=sceneList.Count;

        GlobalController.Instance.sceneList.RemoveAt(GlobalController.Instance.sceneList.Count - 1);


        Debug.Log("Scene list: ");
        foreach (int i in sceneList)
        {
            Debug.Log(i);
        }


        //Time.timeScale = 0;
        Timing.RunCoroutine(_loadScene((GlobalController.Scenes)sceneToLoad,episodeID));
        
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

    void hideMainButtons() { hideUnhideMainButtons(true); }
    void unhideMainButtons() { hideUnhideMainButtons(false); }


    //Fills dynamic button with image and changes its text to scene name
    //I canceled scene image
    //Also assigns listener for button
    void fillDynamicButton(GameObject button, int episodeID)
    {
        string[] descriptionLines = sceneDescriptions.text.Split('\n');

        //Image buttonImage = button.GetComponent<Image>();
        Text buttonText = button.GetComponentInChildren<Text>();
        //Sprite momentImage=(Sprite)Resources.Load(momentsImageDirectory + episodeID, typeof(Sprite));
        //if (!momentImage) Debug.Log("Couldn't find momentImage");
                
        //buttonImage.sprite = momentImage;
        buttonText.text = descriptionLines[episodeID];

  
        button.GetComponent<Button>().onClick.AddListener(delegate () { loadScene((GlobalController.Scenes)GlobalController.Instance.sceneList[episodeID],episodeID); });
        button.GetComponent<HideUnhideButton>().activate();
    }

    
    

    //void OnDisable()
    //{
    //    Debug.Log("On disable function");
    //    cc.enabled = true;
    //}
}
