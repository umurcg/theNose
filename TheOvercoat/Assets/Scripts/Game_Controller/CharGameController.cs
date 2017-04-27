using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

//This script is attached to player object
//It is active for all game while every scene player object is alive
//You can get and set characters through this script
//Owner object should have camera and players objects, nothing else.


public class CharGameController : MonoBehaviour {
    public static CharGameController cgc;
    public static string rigthHand = "Armature/Torso/Chest/Arm_R/Hand_R/HandPosition";
    public static string leftHand = "Armature/Torso/Chest/Arm_L/Hand_L/HandPosition";
    public static string nose = "Armature/Torso/Chest/Neck/Head/NosePosition";
    public enum hand { LeftHand,RightHand};

    //Camera type

 

    public enum cameraType
    {
        Ortographic = 0,
        Perspective = 1
    }
    public cameraType camType= cameraType.Perspective;

    ////For matching doors
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

    //Called when new scene is load
    //It gets last visited scene and looks a door to that scene. And set position of player to it while player came from that door.
    
    void setPositionToDoor(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Setting position to door "+lastDoorId);

        //Get active char
        GameObject activeChar = getActiveCharacter();

        if (activeChar == null) return;

        //Disable nav mesh
        NavMeshAgent nma = activeChar.GetComponent < NavMeshAgent > ();
        if(nma) nma.enabled = false;

        //If lastdoorid is not 0 then set player position to it
        if (lastDoorId != 0)
        {
            setPlayerPositionToDoor(OpenDoorLoad.doors[lastDoorId]);

            if (nma) nma.enabled = true;
            return;
        }

        //While it is called in awake new scene is not assigned to sceneList so last scene is still previous scene. EDİT NO IT WASNT FUCKER!!!
        //GlobalController.Scenes lastVisitedScene = (GlobalController.Scenes) GlobalController.Instance.sceneList[GlobalController.Instance.sceneList.Count-1];
        GlobalController.Scenes lastVisitedScene = GlobalController.getPreviousScene();
        //Debug.Log("Last visited scene is " + lastVisitedScene + " and scene count is " + GlobalController.Instance.sceneList.Count);

        int doorForLastVisitedScene = OpenDoorLoad.getIndexWithScene(lastVisitedScene);

        //Debug.Log("Last visited door id is " + doorForLastVisitedScene);

        if (OpenDoorLoad.doors.ContainsKey(doorForLastVisitedScene))
        {
            OpenDoorLoad lastVisitedDoorScript = OpenDoorLoad.doors[doorForLastVisitedScene];
            setPlayerPositionToDoor(lastVisitedDoorScript);


            if (nma) nma.enabled = true;
            return;

            //print("position changes");
        }
        else
        {
            Debug.Log("Couldn't find door for previous scene");
        }


        //If both case didn't happened then check if scene has only one door. If it is then move player to that door
        if (OpenDoorLoad.doors.Count == 1)
        {
            foreach(KeyValuePair<int,OpenDoorLoad> door in OpenDoorLoad.doors)
            {
                setPlayerPositionToDoor(door.Value);


                if (nma) nma.enabled = true;
                return;
            }
           

        }



        if (nma) nma.enabled = true; 

    }

    void setPlayerPositionToDoor(OpenDoorLoad doorScript)
    {

        
        GameObject lastVisitedDoor = doorScript.gameObject;

        Vector3 spawnPos;
        Quaternion spawnRot;

        //Debug.Log("Assinging position to " + doorScript.Scene);

        if (doorScript.getSpawnPositionAndRotation(out spawnPos, out spawnRot))
        {
           
            movePlayer(spawnPos);
            getActiveCharacter().transform.rotation = spawnRot;
        }
        else
        {
            movePlayer(lastVisitedDoor.transform.position + lastVisitedDoor.transform.forward * 2);
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

    public static GameObject setCharacter(string characterName)
    {

        GameObject character=null;

        int childCount = cgc.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = cgc.transform.GetChild(i);
            if (child.tag != "MainCamera") { 
            
           if (child.name == characterName)
                {
                    child.gameObject.SetActive(true);
                    character = child.gameObject;
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }

          }

        //If owner has see through walls scr'pt update 't
        SeeBehindWall3 sbw = cgc.GetComponentInChildren<SeeBehindWall3>();
        if (sbw) sbw.updateTarget();


        if (character == null) Debug.Log("Could't find character while trying to activate it " + characterName);
        return character;
                
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

        //If owner has see through walls scr'pt update 't
        SeeBehindWall3 sbw = cgc.GetComponentInChildren<SeeBehindWall3>();
        if (sbw) sbw.updateTarget();

    }

    public static GameObject getCamera()
    {
        if (cgc == null) return null;
        
        GameObject mainCam = cgc.transform.GetChild(0).gameObject;
        return mainCam;
        //return getMainCameraComponent().gameObject;
    }

    public static Camera getMainCameraComponent()
    {
        GameObject owner = getOwner();
        Camera cam = owner.GetComponentInChildren<Camera>();
        if (cam == null)
        {
            Debug.Log("Couldnt find camera component under owner");
            return null;
        } 
        return cam;

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

    public static void deactivateCamera()
    {
        getCamera().SetActive(false);
    }

    //This function returns object that is owned by the hand of the armature of a player character.

    public static  GameObject getObjectOfHand(string objectName, hand r_l, bool passNotification=false)
    {

        GameObject hand=getHand(r_l);
        if (!hand) return null;
        Transform obj= hand.transform.Find(objectName);
        if (obj == null)
        {
            //if(!passNotification) Debug.Log("Couldn't find object in " + r_l.ToString());
            return null;
        }

        return obj.gameObject;

        //string playerName = getActiveCharacter().transform.GetChild(0).name;
        //string handName = "";
        //if (r_l == hand.LeftHand)
        //{
        //    handName = leftHand;
        //}
        //else if (r_l == hand.RightHand)
        //{
        //    handName = rigthHand;

        //}

        //string fullString = playerName + "/Armature/Torso/Chest/Arm_L/" + handName + "/" + objectName;

        //Transform handTransform = getActiveCharacter().transform.Find(fullString);
        //if (handTransform!=null)
        //    return handTransform.gameObject;

        //return null;
    }

    //This function returns hand of armature as gameobject
    public static GameObject getHand(hand r_l)
    {
        string handName = "";
        if (r_l == hand.LeftHand) {
            handName = leftHand;
        } else if (r_l == hand.RightHand) {
            handName = rigthHand;

        }
            

        GameObject owner = getActiveCharacter();
        if (owner == null)
        {
            Debug.Log("No player");
            return null;
       }


        Transform handObject = owner.transform.Find(handName);
        if (handObject == null)
        {
            Debug.Log("Couldn't find hand. The path name is " + handName);
            return null;
        }

        return handObject.gameObject;
    }

    //Searches an object through children of hands. It returns first found object. If it doesn't find it then returns null
    public static GameObject searchThroughHands(string objectName)
    {
        GameObject foundObject;
        foundObject=getObjectOfHand(objectName, hand.LeftHand);
        if (foundObject == null)
        {
            foundObject = getObjectOfHand(objectName, hand.RightHand);
        }

        return foundObject;
    }

    //Moves active main character to position with considering camera and unactive characters
    public static void movePlayer(Vector3 pos)
    {
        getOwner().transform.position = pos;
        getActiveCharacter().transform.position = pos;
    }

    public static GameObject getNosePos()
    {

        GameObject owner = getActiveCharacter();
        if (owner == null)
        {
            Debug.Log("No player");
            return null;
        }


        Transform noseObject = owner.transform.Find(nose);
        if (noseObject == null)
        {
            Debug.Log("Couldn't find hand. The path name is " + nose);
            return null;
        }

        return noseObject.gameObject;
    }

    //Sun should have night and day cycle so we found it with that script
    public static GameObject getSun()
    {
        DayAndNightCycle danc= cgc.GetComponentInChildren<DayAndNightCycle>();
        if(!danc)
        {
            //Debug.Log("Couldn't found sun");
            return null;
        }

        return danc.gameObject;
    }

    public static Vector3 getActiveCharacterPosition()
    {
        Vector3 pos=getActiveCharacter().transform.position;
        Debug.Log("Character position is " + pos.ToString());
        //Debug.Log("Name of active character is " + getActiveCharacter().transform.name);
        //Vckrs.testPosition(pos);
        return pos;
    }

    //This scirpts make kovalev covers his face with a napkin
    //If main char is not kovalev does nothing
    //You should call this method when kovalev missing his nose and searches it in the story
    public static void coverKovalevsFace()
    {
        GameObject player = getActiveCharacter();
        if (player.name != "Kovalev") return;

        GameObject paper = searchThroughHands("paper");
        if (paper == null)
        {
            Debug.Log("CouldnT find paper");
        }
        else
        {
            paper.SetActive(true);
        }


        Animator anim = player.GetComponent<Animator>();
        anim.SetBool("RightHandAtFace", true);


    }

    public static void setCameraType(cameraType type)
    {
        cgc.camType = type;

        Debug.Log("Setting camera type to "+ type.ToString());

        //Update main camera

        //If player camera is active dont bother to find camera
        if (getCamera().activeSelf)
        {
            getCamera().GetComponent<CameraController>().updateCameraType();
            return;
        }

        if (CameraController.activeCamera != null)
        {
            CameraController.activeCamera.updateCameraType();
        }else
        {
            Debug.Log("No active camera");
        }

    }

    public static cameraType getCameraType()
    {
        return cgc.camType;
    }

    

}
