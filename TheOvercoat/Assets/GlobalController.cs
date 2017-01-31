﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//Static list that is attached to MainCamera
//It is never destroyed
public class GlobalController : MonoBehaviour {


    public enum Scenes {City=1,IvanHouse=2,KovalevHouse=3,PoliceStation=4,Newspaper=5,Church=6,Doctor=7 };

    public static GlobalController Instance;
    


//  This list holds scenes that are explored. One scene can register more than one.
//  For example if player goes scene 2 from scene 1 and come back to scene 1 list will become
//  [1,2,1]
//  Also use this class for holding all savable data
    public List<int> sceneList;


    //  This list holding scene squence (shortest) for all game. So from this array, all levels can be load with only necessary scene sequence
    //  Dont include main menu
    public List<Scenes> fullGameSceneList;

    void Awake()
    {
        if (Instance == null)
        {
        
            Instance = this;
            //SceneManager.sceneLoaded += registerToSceneList;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        debugWriteCustomSaveFile();


    }

    //This function register creates scene list if it is not exist.
    //After that it registers scene to sceneList
    public void registerToSceneList(/*Scene scene, LoadSceneMode mode*/)
    {
        if (GlobalController.Instance.sceneList == null)
        {
            GlobalController.Instance.sceneList = new List<int>();
        }

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (sceneList.Count > 0)
        {
            //If current scene index equals next scene in story line which means player play in right direction that add that scene into sceneList 
            if(currentScene == (int)fullGameSceneList[sceneList.Count - 1])
            {
                sceneList.Add(currentScene);
            }
        }else
        {
            sceneList.Add((int)fullGameSceneList[0]);
        }


        SaveData();
        //print(scene.buildIndex + " added to list");
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SaveData()
    {

      

        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");

  
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath+ "/Saves/save.vkcrs");

        
        formatter.Serialize(saveFile, sceneList);


        saveFile.Close();


        Debug.Log("Saved");
    }

    public void LoadData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open(Application.persistentDataPath + "/Saves/save.vkcrs", FileMode.Open);
     

        sceneList = (List<int>)formatter.Deserialize(saveFile);


        saveFile.Close();


        Debug.Log("Loaded");
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

}
