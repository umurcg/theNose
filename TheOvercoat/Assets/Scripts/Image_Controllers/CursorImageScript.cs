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
    public Texture2D disabled;
    public Texture2D floor;
    public Texture2D activeObject;
    public Texture2D grab;
    public Texture2D nextSubtitle;

    public Texture2D externalTexture;

    public LayerMask ignoreLayers;

    public bool forceToDefault = false;

    RaycastHit lastHit;

    GameObject player;
    MoveTo mt;

    Text subt;

    Camera currentCamera;

    // Use this for initialization
    void Start()
    {


    }

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
        //Debug.Log("Getting new move to ");
        player = CharGameController.getActiveCharacter();
        if (player != null)
            mt = player.GetComponent<MoveTo>();
        if (SubtitleFade.subtitles != null)
        {
            if (SubtitleFade.subtitles.ContainsKey("CharacterSubtitle"))
            {
                subt = SubtitleFade.subtitles["CharacterSubtitle"];
            }
        }

        Camera[] allCameras = Camera.allCameras;

        foreach(Camera c in allCameras)
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
        //If external texture is not null then just put it no matter what
        if (externalTexture != null)
        {
            //Debug.Log("External texture is not null");
            Cursor.SetCursor(externalTexture, Vector2.zero, CursorMode.Auto);
            return;
        }


        //First look at char subtitile. If it is empty then raycast.


        if (subt != null)
        {

            //Debug.Log("Subtitile is not null");
            if (subt.text != "")
            {
                Cursor.SetCursor(nextSubtitle, Vector2.zero, CursorMode.Auto);
                return;
            }

        }
        //else Debug.Log("Subtitle is null");

        if (forceToDefault)
        {
            Cursor.SetCursor(defaultTexture, Vector2.zero, CursorMode.Auto);
            return;
        }

        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //LayerMask mask = (1 << 8);
        ////mask = (1 << 2);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayers))
        {
            //Debug.Log(hit.transform.gameObject.name + " " + hit.transform.tag + " " + hit.transform.gameObject.layer);

            //Even player can't move if object have this tag, it will change cursor to activeObject
            if(hit.transform.tag== "ActiveEvenCantWalk")
            {

                Cursor.SetCursor(activeObject, Vector2.zero, CursorMode.Auto);
                return;
            } 

            else if (hit.transform.tag == "ActiveObject")
            {
                if (!checkAvaiblity())
                {
                    Cursor.SetCursor(disabled, Vector2.zero, CursorMode.Auto);
                    return;
                }

                Cursor.SetCursor(activeObject, Vector2.zero, CursorMode.Auto);

            }
            else if (hit.transform.tag == "Floor")
            {

                if (!checkAvaiblity())
                {
                    Cursor.SetCursor(disabled, Vector2.zero, CursorMode.Auto);
                    return;
                }

                //Check is reachable if player is active
                if (player != null)
                {
                    //Debug.Log("Player is not null");
                    NavMeshHit nmHit;
                    if (NavMesh.SamplePosition(hit.point, out nmHit, 0.1f, NavMesh.AllAreas))
                    {
                        //Debug.Log("There is navmesh");
                        Cursor.SetCursor(floor, Vector2.zero, CursorMode.Auto);
                    }
                    else
                    {
                        //Debug.Log("There is no navmesh");
                        Cursor.SetCursor(defaultTexture, Vector2.zero, CursorMode.Auto);
                    }
                }
                else
                {
                    Cursor.SetCursor(defaultTexture, Vector2.zero, CursorMode.Auto);
                }


            }
            else if (hit.transform.tag == "Grab")
            {

                Cursor.SetCursor(grab, Vector2.zero, CursorMode.Auto);
            }

            else
            {
                if (!checkAvaiblity())
                {
                    //Debug.Log("is not avaible");
                    Cursor.SetCursor(disabled, Vector2.zero, CursorMode.Auto);
                    return;
                }

                Cursor.SetCursor(defaultTexture, Vector2.zero, CursorMode.Auto);

            }

        }



    }




    bool checkAvaiblity()
    {
        if (mt)
        {
            return mt.enabled;
        }

        //If mt is null this means player couldn't be found. So it assumes player (real player) can still make interactive actions.
        return true;
    }

    public void updateComponents()
    {
        player = CharGameController.getActiveCharacter();
        mt = player.GetComponent<MoveTo>();

    }

}
