using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GlobalController : MonoBehaviour {

    public static GlobalController Instance;

//  This list holds scenes that are explored. One scene can register more than one.
//  For example if player goes scene 2 from scene 1 and come back to scene 1 list will become
//  [1,2,1]
    public List<int> sceneList;

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



    }

    //This function register creates scene list if it is not exist.
    //After that it registers scene to sceneList
    public void registerToSceneList(/*Scene scene, LoadSceneMode mode*/)
    {
        if (GlobalController.Instance.sceneList == null)
        {
            GlobalController.Instance.sceneList = new List<int>();
        }
        


        GlobalController.Instance.sceneList.Add(SceneManager.GetActiveScene().buildIndex);
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


        print("Saved!");
    }

    public void LoadData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open(Application.persistentDataPath + "/Saves/save.vkcrs", FileMode.Open);
     

        sceneList = (List<int>)formatter.Deserialize(saveFile);


        saveFile.Close();


        print("Loaded");
    }

}
