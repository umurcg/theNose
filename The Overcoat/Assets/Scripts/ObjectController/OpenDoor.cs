using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenDoor : MonoBehaviour {

    static public Dictionary<int, OpenDoor> doors = new Dictionary<int, OpenDoor>();

    public float speed = 1;
    public bool otherCanOpen=false;
    public bool playerCanOpen = false;
    public int doorId;

    SkinnedMeshRenderer smr;
    float key = 0;
    bool open;
    bool close;
    bool playerInside;


    // Use this for initialization
    void Awake()
    {
        smr = GetComponent<SkinnedMeshRenderer>();
        key = smr.GetBlendShapeWeight(0);
        doors[doorId] = this;

    }
   
	
	// Update is called once per frame
	void Update () {
        if (open)
        {
            //print(key);
            key += Time.deltaTime * speed;
            if (key >= 100)
            {
                open = false;
                key = 100;

            }
            smr.SetBlendShapeWeight(0, key);
        }
        else if (close)
        {
            key -= Time.deltaTime * speed;
            if (key <= 0)
            {
                close = false;
                key = 0;

            }
            smr.SetBlendShapeWeight(0, key);
        }

        if(key>=100 && playerInside)
        {
            loadScene();
        }

    }


   void loadScene()
    {
        CharGameController.setLastDoorId(doorId);
        LoadScene ls = GetComponent<LoadScene>();
        ls.Load();
    }

   void OnTriggerEnter(Collider col)
    {
        if ((col.tag == "Player"&&playerCanOpen)|| (col.tag != "Player"&&otherCanOpen))
        {
            if (col.tag == "Player" && playerCanOpen)
                playerInside = true;

            //print("open");
            open = true;
            close = false;
        }

    }

    void OnTriggerExit(Collider col)
    {
        if ((col.tag == "Player" && playerCanOpen) || (col.tag != "Player" && otherCanOpen))
        {

            if (col.tag == "Player")
                playerInside = false;

            open = false;
            close = true;
        }
   }


     


}
