using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class OpenDoorLoad : LoadScene {

    static public Dictionary<int, OpenDoorLoad> doors = new Dictionary<int, OpenDoorLoad>();

    public float speed = 1;
    public bool otherCanOpen=false;
    public bool playerCanOpen = false;
    public string lockMessageToPlayer = "The door is locked.";
    public float messageDuration = 3f;
    public int doorId;

    //ForMapUI
    public string doorName;

    SkinnedMeshRenderer smr;
    float key = 0;
    bool open;
    bool close;
    bool playerInside;

    public void Unlock()
    {
        playerCanOpen = true;
    }

    public void Lock()
    {
        playerCanOpen = false;
    }


    // Use this for initialization
    void Awake()
    {
        smr = GetComponent<SkinnedMeshRenderer>();
        key = smr.GetBlendShapeWeight(0);

        if (doors.ContainsKey(doorId))
        {
            Debug.Log("There cant be more than one door having same index in scene. Check it. Door id is " + doorId);
            return;
        }
        doors[doorId] = this;

        if (doorName == null) doorName = Scene.ToString(); 
    
    }

    // Update is called once per frame
    void Update () {

        if (debugLoad)
        {
            Load();
            debugLoad = false;
        }

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
            Load();
            enabled = false;
        }

    }


    public override IEnumerator<float> _Load()
    {  

        CharGameController.setLastDoorId(doorId);
        Timing.RunCoroutine(base._Load());

        yield break;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && !playerCanOpen)
        {
            Timing.RunCoroutine(Vckrs.showMessageForSeconds(lockMessageToPlayer, SubtitleFade.subtitles["CharacterSubtitle"], messageDuration));
        }

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

    void OnDisable()
    {
        if (doors.Count > 0)
            doors.Clear();
    }
     
    //public static OpenDoorLoad getDoorOfScene(GlobalController.Scenes scene)
    //{
    //    foreach(KeyValuePair<int,OpenDoorLoad> door in doors)
    //    {
    //        if (door.Key == (int)scene)
    //            return door.Value ;
    //    }

    //    return null;

    //}

    //public static void lockUnlockDoor(GlobalController.Scenes scene, bool locked)
    //{
        
    //    OpenDoorLoad door= getDoorOfScene(scene);
    //    if (door)
    //    {
    //        Debug.Log("Openning " + scene.ToString() + " door");
    //        door.playerCanOpen = !locked;

    //    }

    //}

}
