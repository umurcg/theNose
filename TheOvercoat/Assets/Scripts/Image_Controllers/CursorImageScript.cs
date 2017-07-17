using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//Dependent to: Player(Owner)
//This script changes mouse image according to hitted object. 
//If object is note an active object, default image will be shown.

public class CursorImageScript : MonoBehaviour
{

    //public float xOffset, yOffset = 10;
    public Texture2D defaultTexture;
    public Texture2D floor;
    public Texture2D activeObject;
    public Texture2D frontierObject;
    public Texture2D nextSubtitle;
    public Texture2D draw;
    public Texture2D button;

    Texture2D currentCursor;


    [HideInInspector]
    public Texture2D externalTexture;

    public LayerMask ignoreLayers;

    public bool printShootedTag=false;

    public bool printShootedName = false;

    [HideInInspector]
    public bool forceToDefault = false;

    RaycastHit lastHit;

    GameObject player;
    MoveTo mt;
    UnityEngine.AI.NavMeshAgent playerAgent;

    Text subt;

    Camera currentCamera;

    public bool showCursor = true;

    ////TODO right whole script again
    ////Use this dictionary for cursors havng most priority. 
    //Dictionary<string, Texture2D> tagCursorPair;


    //private void Awake()
    //{
    //    tagCursorPair = new Dictionary<string, Texture2D>();
    //}

    // Use this for initialization
    void Start()
    {
        //Vckrs.doItAfterOneFrame(testDelegate);
        //testDelegate();

    }

    //public void testDelegate()
    //{
    //    Debug.Log("Yes your ddelegate method works");
    //}

    void OnEnable()
    {
        //Tell our 'registerToSceneList' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += newSceneIsLoad;
    }

    void OnDisable()
    {
        //Tell our 'registerToSceneList' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= newSceneIsLoad;
    }

    void newSceneIsLoad(Scene scene, LoadSceneMode mode)
    {
        updatePlayerVariables();
        updateSubtitle();
        Vckrs.doItAfterFrame(assignCamera,3);
        //assignCamera();

    }

    public void updatePlayerVariables()
    {
        //Debug.Log("Getting new move to ");
        player = CharGameController.getActiveCharacter();
        if (player != null)
        {
            mt = player.GetComponent<MoveTo>();
            playerAgent = player.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
    }


    void updateSubtitle()
    {
        if (SubtitleFade.subtitles != null)
        {
            if (SubtitleFade.subtitles.ContainsKey("CharacterSubtitle"))
            {
                subt = SubtitleFade.subtitles["CharacterSubtitle"];
            }
        }
    }

    void assignCamera()
    {
        Camera[] allCameras = Camera.allCameras;

        foreach (Camera c in allCameras)
        {
            if (c.gameObject.activeSelf == true)
            {
                currentCamera = c;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        checkForInputType();


        if (!showCursor)
        {
            return;
        }


        //If external texture is not null then just put it no matter what
        if (externalTexture != null)
        {
            //Debug.Log("External texture is not null");
            setCursor(externalTexture);
            return;
        }

        if (subt == null) updateSubtitle();

        //First look at char subtitile. If it is empty then raycast.
        if (subt != null)
        {
            //Debug.Log("Subtitile is not null");
            if (subt.text != "" && subt.fontStyle==FontStyle.Normal)
            {
                setCursor(nextSubtitle);
                return;
            }

        }
        else
        {
            Debug.Log("Subtitle is null");
        }

        if (forceToDefault)
        {
            setCursor(defaultTexture);
            return;
        }

        if (currentCamera == null) assignCamera();
        if (currentCamera==null)
        {
            enabled = false;
            Debug.Log("Disabling cursor image script because there is no camera");
            return;
        }


        if (raycastFor2d())
        {
            //Debug.Log("Button is hit");
            return;
        }

        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayers))
        {
            //Debug.Log(hit.transform.gameObject.name + " " + hit.transform.tag + " " + hit.transform.gameObject.layer);

            //if(tagCursorPair!=null)
            ////Look for dictinary in here. Dictionary hs most priority.
            //foreach(KeyValuePair<string, Texture2D> pair in tagCursorPair)
            //{
            //    if (hit.transform.tag == pair.Key)
            //    {
            //        Cursor.SetCursor(pair.Value, Vector2.zero, CursorMode.Auto);
            //        return;
            //    }
            //}

            string shootedTag = hit.transform.tag;

            if (printShootedTag) Debug.Log(shootedTag);
            if (printShootedName) Debug.Log(hit.transform.name);

            switch (shootedTag)
            {
                case "Floor":

                    //Debug.Log("chekc avabilirty" + checkAvaiblity() + " reachblity " + isPositionReachabel(hit.point));

                    if (checkAvaiblity() && isPositionReachabel(hit.point))
                    {
                        setCursor(floor);
                    }else if(checkAvaiblity())
                    {
                        setCursor(defaultTexture);
                    }
                    break;

                case "ActiveObject":
                    //Debug.Log("Active object");
                    setCursor(activeObject);
                    break;

                case "ActiveObjectOnlyCursor":

                    setCursor(activeObject);
                    break;

                case "ActiveEvenCantWalk":

                    setCursor(activeObject);
                    break;
                case "Grab":

                    setCursor(frontierObject);
                    break;

                case "Draw":

                    setCursor(draw);
                    break;

       
                default:
                    setCursor(defaultTexture);

                    break;

                    
            }

            return;

          
        }




        setCursor(defaultTexture);
        

    }




    bool checkAvaiblity()
    {
        
        if (mt)
        {
            return mt.enabled;
        }else
        {
            //FOR BIRD

            GameObject player = CharGameController.getActiveCharacter();
            if (player!=null)
            {
                MoveToWithoutAgent mtwa = player.GetComponent<MoveToWithoutAgent>();
                if (mtwa && mtwa.enabled) return true;
            }
        }

        //If mt is null this means player couldn't be found. So it assumes player (real player) can still make interactive actions.
        return true;
    }

    //public void updateComponents()
    //{
    //    player = CharGameController.getActiveCharacter();
    //    mt = player.GetComponent<MoveTo>();

    //}

    public void setExternalTexture(Texture2D texture)
    {
        externalTexture = texture;
    }

    public void resetExternalCursor()
    {
        if(externalTexture!=null)
           externalTexture = null;
        //Debug.Log("Reseting external cursor");
        //DestroyImmediate(externalTexture);

    }

    //public void addTagCursorPair(string tag, Texture2D pair)
    //{
    //    tagCursorPair.Add(tag, pair);
    //}

    //public void removeTagCursorPairWithTag(string tag)
    //{
    //    tagCursorPair.Remove(tag);
    //}
    
    bool isPositionReachabel(Vector3 position)
    {
        if (player == null)
        {
            updatePlayerVariables();
            Debug.Log("player is bull");
            return false;
        }


        if (player.GetComponent<UnityEngine.AI.NavMeshAgent>() == null) return false;

       //Vckrs.testPosition(hit.point);
       //Debug.Log("Player is not null");

       UnityEngine.AI.NavMeshHit nmHit;
        return (UnityEngine.AI.NavMesh.SamplePosition(position, out nmHit, 2f, playerAgent.areaMask));
                
    }

    void setCursor(Texture2D cursor)
    {
        if (currentCursor == cursor) return;

        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        currentCursor = cursor;
    }


    bool raycastFor2d()
    {
        //Now raycast for 2d objects

        RaycastHit2D hit2d = Physics2D.Raycast(currentCamera.transform.position, Input.mousePosition);

        if (hit2d.collider != null)
        {
   
            if (hit2d.transform.gameObject.tag == "Button")
            {
                //Debug.Log("2D tag is " + hit2d.transform.gameObject.tag);
                setCursor(button);
                return true;
            }
        }

        return false;
    }


    private void checkForInputType()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            showCursor = true;
            Cursor.visible = true;
        }else if (isControlerInput())
        {
            showCursor = false;
            Cursor.visible = false;
        }

    }




    private bool isControlerInput()
    {
        bool gamePad = false;
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown("joystick button " + i))
            {
                gamePad = true;
            }
        }


        gamePad = gamePad || Input.GetAxis("GamePadL_X") != 0 || Input.GetAxis("GamePadL_Y") != 0 || Input.GetAxis("GamePadR_X") != 0 || Input.GetAxis("GamePadR_Y") != 0;


        // joystick buttons
        if (gamePad)
        {
            //Debug.Log("GamePAAAAAAAAD");
            return true;

        }

        //// joystick axis
        //if (Input.GetAxis("XC Left Stick X") != 0.0f ||
        //   Input.GetAxis("XC Left Stick Y") != 0.0f ||
        //   Input.GetAxis("XC Triggers") != 0.0f ||
        //   Input.GetAxis("XC Right Stick X") != 0.0f ||
        //   Input.GetAxis("XC Right Stick Y") != 0.0f)
        //{
        //    return true;
        //}

        return false;
    }

}


//    //Even player can't move if object have this tag, it will change cursor to activeObject
//    if (hit.transform.tag== "ActiveEvenCantWalk")
//    {

//        Cursor.SetCursor(activeObject, Vector2.zero, CursorMode.Auto);
//        return;
//    } 

//    else if (hit.transform.tag == "ActiveObject" || hit.transform.tag=="ActiveObjectOnlyCursor")
//    {
//        if (!checkAvaiblity())
//        {
//            Cursor.SetCursor(disabled, Vector2.zero, CursorMode.Auto);
//            return;
//        }

//        Cursor.SetCursor(activeObject, Vector2.zero, CursorMode.Auto);

//    } 
//    else if (hit.transform.tag == "Floor")
//    {

//        if (!checkAvaiblity())
//        {
//            Cursor.SetCursor(disabled, Vector2.zero, CursorMode.Auto);
//            return;
//        }

//        //Check is reachable if player is active
//        if (player != null)
//        {
//            //Vckrs.testPosition(hit.point);
//            //Debug.Log("Player is not null");

//            NavMeshHit nmHit;
//            if (NavMesh.SamplePosition(hit.point, out nmHit, 0.1f, NavMesh.AllAreas))
//            {
//                //Debug.Log("There is navmesh");
//                Cursor.SetCursor(floor, Vector2.zero, CursorMode.Auto);
//            }
//            else
//            {
//                //Debug.Log("There is no navmesh");
//                Cursor.SetCursor(defaultTexture, Vector2.zero, CursorMode.Auto);
//            }
//        }
//        else
//        {
//            Cursor.SetCursor(defaultTexture, Vector2.zero, CursorMode.Auto);
//        }


//    }
//    else if (hit.transform.tag == "Grab")
//    {

//        Cursor.SetCursor(grab, Vector2.zero, CursorMode.Auto);
//    }

//    else
//    {
//        if (!checkAvaiblity())
//        {
//            //Debug.Log("is not avaible");
//            Cursor.SetCursor(disabled, Vector2.zero, CursorMode.Auto);
//            return;
//        }

//        Cursor.SetCursor(defaultTexture, Vector2.zero, CursorMode.Auto);

//    }

//}