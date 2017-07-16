using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MovementEffects;
using System.Linq;
using System;

//Static list that is attached to MainCamera
//It is never destroyed

public enum inputTye {Keyboard, Gamepad};

public class GlobalController : MonoBehaviour {


    public enum Scenes {MainMenu=0 ,City=1,IvanHouse=2,KovalevHouse=3,PoliceStation=4,Newspaper=5,Church=6,Doctor=7,PrisonerGame=8,Atolye=9,OsmanBey=10, None=11 };
    
    public static GlobalController Instance;

    public bool startAtFullScreen = false;


    public inputTye Input=inputTye.Keyboard;
    
    //Language settings
    public enum Language { ENG=0, TR=1, RUS=2};
    public Language languageSetting = Language.ENG;
    public float audioLevel = 1f;
    //public float musicLevel=1f;

    //This list  will hold game controller  names that are used and won't be used again. For example some city games that only shoul
    //be played once.
    //TODO find more stable way to hold gameControllers
    public List<string> usedGameControllers;
    

    //  This list holds scenes that are explored. One scene can register more than one.
    //  For example if player goes scene 2 from scene 1 and come back to scene 1 list will become
    //  [1,2,1]
    //  Also use this class for holding all savable data
    public List<int> sceneList;

    //While player can also load previous episode this script will hold maximum scenes that user played. 
    public List<int> maxSceneList;
    //Same logic with maxScne List. While loading a moment, mainplayer trims usedGameControllers. So this list holds nontrimmed format.
    //public List<string> maxUsedGameController;

    //Forground object forward distance. This value will be used when insantiating object on forground. It indicates distance from camera.
    public static float cameraForwardDistance = 100;

    //You can use this variable for starting game with specific sceneList in editor.
    public Scenes[] debugSceneList;

    //  This list holding scene squence (shortest) for all game. So from this array, all levels can be load with only necessary scene sequence
    //  Dont include main menu
    //public TextAsset fullGameSceneListTA;

    //[HideInInspector]
    public Scenes[] fullGameSceneList= {
    Scenes.City,
        Scenes.IvanHouse,
        Scenes.City,
        Scenes.KovalevHouse,
        Scenes.City,
        Scenes.PoliceStation,
        Scenes.City,
        Scenes.Newspaper,
        Scenes.City,
        Scenes.Newspaper,
        Scenes.City,
        Scenes.Church,
        Scenes.City,
        Scenes.KovalevHouse,
        Scenes.City,
        Scenes.Doctor,
        Scenes.City
};


public int setDebugListToLevelIndex;

    public void setDebugListToIndex()
    {
        debugSceneList = new Scenes[setDebugListToLevelIndex];
        for (int i = 0; i < setDebugListToLevelIndex; i++)
        {
            debugSceneList[i] = fullGameSceneList[i];

        }
    }

    void Awake()
    {

        //if (startAtFullScreen) StartAtFullScreen();

        //Kill all mec before starting new scene
        Timing.KillAllCoroutines();

        ////Extract full game scene list from ta
        //string[] fullGamesString = fullGameSceneListTA.text.Split('\n');
        //fullGameSceneList = new Scenes[fullGamesString.Length];
        //for(int i=0;  i<fullGamesString.Length; i++)
        //{
        //    fullGameSceneList[i]=(Scenes)System.Enum.Parse(typeof(Scenes), fullGamesString[i]);
        //    Debug.Log(fullGameSceneList[i].ToString());
        //}

        


        if (Instance == null)
        {
            //Debug.Log("Creating global controller instance");
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        if (debugSceneList.Length!=0 && sceneList.Count==0)
        {
            //if (Application.isEditor)
            //{
                foreach (Scenes sce in debugSceneList)
                {

                    sceneList.Add((int)sce);
                    maxSceneList.Add((int)sce);
                }
                debugSceneList = new Scenes[0];
            //}
        }

        usedGameControllers = new List<string>();
        //maxUsedGameController = usedGameControllers;
        
    }

    //private void StartAtFullScreen()
    //{
    //    //Debug.Log(Screen.currentResolution);
    //    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
    //}

    void Start()
    {
        //if it is not main menu register to scene list
        
        //if (SceneManager.GetActiveScene().buildIndex != (int)Scenes.MainMenu)
        //{
           
        //    GlobalController.Instance.registerToSceneList();

        //}



    }

    void OnEnable()
    {
        //Tell our 'registerToSceneList' function to start listening for a scene change as soon as this script is enabled.
       SceneManager.sceneLoaded += registerToSceneList;
    }

    void OnDisable()
     {
        //Tell our 'registerToSceneList' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
       SceneManager.sceneLoaded -= registerToSceneList;
     }

    //This function register creates scene list if it is not exist.
    //After that it registers scene to sceneList
    public void registerToSceneList(Scene scene, LoadSceneMode mode)
    {

        //Debug.Log("Registering");
        if (SceneManager.GetActiveScene().buildIndex == (int)Scenes.MainMenu) return;
 

        if (GlobalController.Instance.sceneList == null)
        {
            GlobalController.Instance.sceneList = new List<int>();
        }

        if (GlobalController.Instance.maxSceneList == null)
        {
            GlobalController.Instance.maxSceneList = new List<int>();
        }
        
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (sceneList.Count > 0)
        {

            if (GlobalController.Instance.sceneList[GlobalController.Instance.sceneList.Count - 1] == currentScene ) return;

            //If current scene index equals next scene in story line which means player play in right direction that add that scene into sceneList 
            //Debug.Log("scene list count: " + sceneList.Count + " full game scene list count" + fullGameSceneList.Length);
            //Debug.Log("current scene is " + currentScene + " it should be " + (int)fullGameSceneList[sceneList.Count ]);
            if (currentScene == (int)fullGameSceneList[sceneList.Count])
            {
                //Debug.Log(currentScene + "is added to scene list");
                sceneList.Add(currentScene);

                if (sceneList.Count > maxSceneList.Count) maxSceneList = sceneList;

            }else
            {
                Debug.Log("Player is not going to right direction in story");
            }
         }
          else
        {
            //Debug.Log("Registered scene " + currentScene); //TODOthis part is for debugging
            //sceneList.Add((int)fullGameSceneList[0]);
            sceneList.Add(currentScene);
        }


        SaveData();
        //print(scene.buildIndex + " added to list");
    }


	public void removeSaveData()
    {

        Debug.Log("Deleting file");
        File.Delete(Application.persistentDataPath + "/Saves/save.vkcrs");
    }


    public bool SaveData()
    {      

        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
            
        }


        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath+ "/Saves/save.vkcrs");
                


        formatter.Serialize(saveFile, sceneList);
        formatter.Serialize(saveFile, usedGameControllers);

        //Save player name 
        string playerName = "";
        
        playerName = CharGameController.getActiveCharacter().name;
        formatter.Serialize(saveFile, playerName);
        
        saveFile.Close();
        
        Debug.Log("Saved to "+ Application.persistentDataPath + "/Saves/save.vkcrs");
        return true;
    }

    public bool LoadData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        var files = new DirectoryInfo(Application.persistentDataPath + "/Saves/").GetFiles();
        //Debug.Log("Files are ");
        //foreach (var f in files) Debug.Log(f);

        if (files.Length==0)
        {
            Debug.Log("No save file");
            return false;
        }

        FileStream saveFile = File.Open(Application.persistentDataPath + "/Saves/save.vkcrs", FileMode.Open);
        
        maxSceneList = (List<int>)formatter.Deserialize(saveFile);
        usedGameControllers = (List<string>)formatter.Deserialize(saveFile);

        //Get player name and set active character as that player
        string playerName = (string)formatter.Deserialize(saveFile);
        if (playerName != "" && CharGameController.cgc != null)
        {
            Debug.Log("Setting character as " + playerName);
            CharGameController.setCharacter(playerName);
            CharGameController.getCamera().GetComponent<CameraFollower>().updateTarget();
            CharGameController.getCamera().GetComponent<CameraFollower>().fixRelativeToDefault();
        }


        sceneList = maxSceneList;
        //usedGameControllers = maxUsedGameController;

        saveFile.Close();
        
        Debug.Log("Loaded");

        return true;
    }


    public bool isSaveDataAvaible()
    {
        return File.Exists(Application.persistentDataPath + "/Saves/save.vkcrs");
    }

    //Unnecesseary
    public FileInfo[] readAllSaveFileNames()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
            return null;

        var info = new DirectoryInfo(Application.persistentDataPath+"/Saves/");
        var fileInfo = info.GetFiles();

        //Prints name of files
        foreach (var file in fileInfo)
             Debug.Log(file.Name);

        return (FileInfo[])fileInfo;

    }

    
    public List<int> getCurrentSceneList()
    {
        return sceneList;
    }

    public int getLastSceneInList()
    {
        return sceneList[sceneList.Count - 1];
    }

    public void removeLastScene()
    {
        sceneList.RemoveAt(getLastSceneInList());
    }

    public void clearSceneList()
    {
        sceneList.Clear();
    }
    

    public void debugWriteCustomSaveFile()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");


        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath + "/Saves/save.vkcrs");

        List<int> exampleList = new List<int>() { 2, 3 , 4, 5, 6, 7,8,9,2,1};
        
        formatter.Serialize(saveFile,exampleList);


        saveFile.Close();


        Debug.Log("Saved");
    }

    //Counts how may times a scene is visited by player
    public static int  countSceneInList(GlobalController.Scenes scn)
    {
        int count=0;
        if (!Instance.sceneList.Contains((int)scn)) return count;

        for(int i = 0; i < Instance.sceneList.Count; i++)
        {
            if (Instance.sceneList[i] == (int)scn) count++;
        }

        return count;
    }

    public static bool isScnListContains(GlobalController.Scenes scn)
    {
        return Instance.sceneList.Contains((int)scn);
    }

    public Scenes getNextScene()
    {
        return fullGameSceneList[sceneList.Count];
    }

    public void loadNextScene()
    {
        SceneManager.LoadScene((int)getNextScene());
    }

    public static Scenes getPreviousScene()
    {
        if (Instance.sceneList.Count < 2) return Scenes.None;
        Scenes prevScene = (GlobalController.Scenes)Instance.sceneList[Instance.sceneList.Count - 2];
        //Debug.Log(prevScene);
        return prevScene;
    }

    public inputTye getInputType()
    {
        return Instance.Input;
    }

    public void setInputType(int type)
    {
        Input =(inputTye) type;
    }

    public  Language getLangueSetting()
    {
        return Instance.languageSetting;
    }

    public void setLanguageSetting(int id)
    {
        languageSetting = (Language)id;

        //Change current visible texts
        DynamicLanguageTexts.updateAllCurrentVisibleTexts();

        //Update who is talking dictionary
        if (WhoIsTalking.self != null)
        {
            foreach(KeyValuePair<string,List<GameObject>> pair in WhoIsTalking.self.characters)
            {
                foreach(GameObject chr in pair.Value)
                {
                    RegisterToSubtitleList register = chr.GetComponent<RegisterToSubtitleList>();
                    if (register) register.refreshName();
                }

               
            }
        }

        if (GetTextFromTA.allTexts != null)
        {
            foreach(GetTextFromTA script in GetTextFromTA.allTexts)
            {
                script.getString();
            }
        }


        Debug.Log("Language is " + languageSetting);
        //Instance.languageSetting = lan;
    }

    public void setAudioLevel(float level)
    {
        Debug.Log("Setting audio leve");
        audioLevel = level;
    }

    public void setMusicLevel(float level)
    {
        Debug.Log("Setting music");
        GetComponent<AudioSource>().volume = Mathf.Clamp(level, 0, 1);
    }

    public float getAudioLevel()
    {
        return audioLevel;
    }

    public float getMusicLevel()
    {
        return GetComponent<AudioSource>().volume;
    }

    public void registerGameController(string gc)
    {
        if (!usedGameControllers.Contains(gc) && !isGameControllerIsUsed(trimEpisodeID(gc)))
        {
            usedGameControllers.Add(gc);
            //Debug.Log(gc);
        }
        //if (!maxUsedGameController.Contains(gc)) maxUsedGameController.Add(gc);

    }

    //With this function you can add game controllers more than one time.
    public void registerGameControllerCanBeDuplicated(string gc)
    {
         usedGameControllers.Add(gc);

        //Add to max used game controller if current game controller count in usedgamecontroller exceeds in current cunt in max used game controller.
        //I now it is hard to think
        //if(countGameController(gc) > countGameControllerInMax(gc))
        //{
        //    maxUsedGameController.Add(gc);
        //}
    }

    //Counts game controller with trimming ids in maxUsedGameController
    //public int countGameControllerInMax(string gc)
    //{
    //    int count = 0;
    //    foreach (string ugc in maxUsedGameController)
    //    {
    //        if (gc == trimEpisodeID(ugc)) count++;
    //    }

    //    return count;
    //}


    //Counts game controller with trimming ids
    public int  countGameController(string gc)
    {
        int count = 0;
        foreach (string ugc in usedGameControllers)
        {
            if (gc == trimEpisodeID( ugc)) count++;
        }

        Debug.Log("This game controller registered " + count + " times");

        return count;
   }

    string trimEpisodeID(string gc)
    {
        int index = gc.IndexOf('#');
        return gc.Substring(0, index);
    }

    //While checking wether or not game controller is used it trimmes id because ids are just for trimming game controllers while loading game
    public bool isGameControllerIsUsed(string gc)
    {
        foreach(string s in usedGameControllers)
        {
            //Trim episodeID of it while episode id is only used in trimmin while loading from moments
            string trimedEpisodeID = trimEpisodeID(s);
            //Debug.Log("Game Controller ID is "+trimedEpisodeID);

            if (gc == trimedEpisodeID) return true;
            

        
        }
        return false;
    }

    //This functions help you to check used game controllers without generated and episode id. It is not very stable while scene names and game objects name can be duplicated. But if you are sure game controller is not duplicated then you can use it
    public bool isGameControllerIsUsedSceneNameAndGameObjectName(string gc)
    {
        int lengthOfGC = gc.Length;
        foreach (string ugc in usedGameControllers)
        {
            if (gc.Length<ugc.Length  && gc == ugc.Substring(0, lengthOfGC)) return true;
        }

        return false;
    }



}
