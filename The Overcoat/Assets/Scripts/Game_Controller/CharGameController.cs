using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharGameController : MonoBehaviour {
    static CharGameController cgc;

    static int lastDoorId;

    //public GameObject go;

    //static string activeCharacter;

	// Use this for initialization
	void Awake () {
        if (cgc == null)
        {
            cgc = this;
            Object.DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += setPositionToDoor;
        }
        else if(cgc!=this){
            Destroy(gameObject);
        }
   

    }

    void Update()
    {
        
    }

    void setPositionToDoor(Scene scene, LoadSceneMode mode)
    {

     
        if (lastDoorId != 0)
        {

            GameObject activeChar = getActiveCharacter();

            NavMeshAgent nma = activeChar.GetComponent < NavMeshAgent > ();
            nma.enabled = false;

            print("position");
            if (OpenDoor.doors.ContainsKey(lastDoorId))
            {

                GameObject door = OpenDoor.doors[lastDoorId].gameObject;

                if (door.transform.childCount>0) { 
                    GameObject spawnPos = door.transform.GetChild(0).gameObject;
                    activeChar.transform.position = spawnPos.transform.position;

                    //Instantiate(go, spawnPos.transform.position,transform.rotation);
                    //print("Object started at spawn position " + spawnPos.transform.position);
                }
                else
                {
                    activeChar.transform.position = door.transform.position;

                    //print("Object started at door position " + door.transform.position);
                }
                //print("position changes");
            }

            nma.enabled = true;
        }
               

    }



    static public void setLastDoorId(int index)
    {
        lastDoorId = index;
    }

    public static GameObject getActiveCharacter()
    {

        if (cgc == null)
        {
            print("There is no characte game controller instance");
        }
        int childCount = cgc.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = cgc.transform.GetChild(i);
            if (child.tag != "MainCamera" && child.gameObject.activeSelf)
            {
                return child.gameObject;
            }
        }

        return null;

     }

    public static void setCharacter(string characterName)
    {
        

        int childCount = cgc.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = cgc.transform.GetChild(i);
            if (child.name == "Main Camera")
                ;
            else if (child.name == characterName)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }       
           

            }
                
         
        
    }
}
