using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

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

    RaycastHit lastHit;

    GameObject player;
    MoveTo mt;

    Text subt;
    // Use this for initialization
    void Start()
    {
        player = CharGameController.getActiveCharacter();
        if (player != null)
            mt = player.GetComponent<MoveTo>();
        subt = SubtitleFade.subtitles["CharacterSubtitle"];
    }

    // Update is called once per frame
    void Update()
    {

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



        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << 8)))
        {
            //Debug.Log(hit.transform.tag);



            if (hit.transform.tag == "ActiveObject")
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

}
