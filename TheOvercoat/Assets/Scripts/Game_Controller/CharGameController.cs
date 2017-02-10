using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//This script is attached to player object
//It is active for all game while every scene player object is alive
//You can get and set characters through this script
//Owner object should have camera and players objects, nothing else.


public class CharGameController : MonoBehaviour {
    static CharGameController cgc;
    public string rigthHand = "/ Armature / Torso / Chest / Arm_L / Hand_R";
    public string leftHand = "/ Armature / Torso / Chest / Arm_L / Hand_L";
    public enum hand { LeftHand,RightHand};

    //For matching doors
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


    void setPositionToDoor(Scene scene, LoadSceneMode mode)
    {

     
        if (lastDoorId != 0)
        {

            GameObject activeChar = getActiveCharacter();

            NavMeshAgent nma = activeChar.GetComponent < NavMeshAgent > ();
            nma.enabled = false;

            print("position");
            if (OpenDoorLoad.doors.ContainsKey(lastDoorId))
            {

                GameObject door = OpenDoorLoad.doors[lastDoorId].gameObject;

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

    public static GameObject getOwner()
    {
        return cgc.gameObject;
    }

    public static GameObject getActiveCharacter()
    {

        if (cgc == null)
        {
            Debug.Log("There is no characte game controller instance");
            return null;
        }
        int childCount = cgc.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = cgc.transform.GetChild(i);

            Camera camComponent = child.GetComponent<Camera>();
            if (camComponent==null && child.gameObject.activeSelf)
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
            if (child.tag != "MainCamera") { 
            
           if (child.name == characterName)
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

    public static void setCharacter(int characterIndex)
    {


        int childCount = cgc.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = cgc.transform.GetChild(i);


            if (i == characterIndex)
            {
                if (child.tag == "MainCamera")
                {
                    Debug.Log("You can't set camera as character");
                    return;
                }

                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }


        }

    }

    public static GameObject getCamera()
    {
        return cgc.transform.GetChild(0).gameObject;
    }

    public static void deactivateAllCharacters()
    {
        int childCount = cgc.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = cgc.transform.GetChild(i);
            Camera camComponent = child.GetComponent<Camera>();

            if (camComponent==null)
            {
                //Debug.Log("Deactivating " + child.name);
                child.gameObject.SetActive(false);
            }

        }
    }

 

    //This function returns object that is owned by the hand of the armature of a player character.

    public static  GameObject getObjectOfHand(string objectName, hand r_l)
    {
        string playerName = getActiveCharacter().transform.GetChild(0).name;
        string handName="";
        if (r_l == hand.LeftHand) { handName = "Hand_L"; } else if (r_l == hand.RightHand) { handName = "Hand_R"; }

        string fullString = playerName + "/Armature/Torso/Chest/Arm_L/" + handName + "/" + objectName;

        Transform handTransform = getActiveCharacter().transform.Find(fullString);
        if (handTransform!=null)
            return handTransform.gameObject;

        return null;
    }

}
