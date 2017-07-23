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
    public GameObject spawnObject;

    public Material nonActiveMat;
    public Material activeMat;
    bool active = false;
    Renderer[] renderers;

    //ForMapUI
    public string doorNameENG;
    public string doorNameTR;

    SkinnedMeshRenderer smr;
    float key = 0;
    bool open;
    bool close;
    bool playerInside;

    public void Unlock()
    {
        playerCanOpen = true;
        transform.tag = "ActiveObject";
    }

    public void Lock()
    {
        playerCanOpen = false;
        transform.tag = "Untagged";
    }

    public bool isLocked()
    {
        return !playerCanOpen;
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

        //Debug.Log("Registering " + Scene);
        doors.Add(doorId,this);

        if (doorNameTR == null) doorNameTR = Scene.ToString();
        if (doorNameENG == null) doorNameENG = Scene.ToString();


        if (spawnObject == null && transform.childCount>0) spawnObject = transform.GetChild(0).gameObject;

        renderers = GetComponentsInChildren<Renderer>();

    }

    // Update is called once per frame
    void Update () {

        checkMaterial();

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


    void checkMaterial()
    {
        if (nonActiveMat == null || activeMat == null) return;

        if (!active && playerCanOpen)
        {
            Debug.Log("Making object active");
            foreach (Renderer rend in renderers)
            {
                rend.material = activeMat;
            }
            active = true;
        }
        else if (active && !playerCanOpen)
        {
            foreach (Renderer rend in renderers)
            {
                rend.material = nonActiveMat;
            }
            active = false;
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


        doors.Remove(doorId);


    }
     
    //Opens all the scene doors that player went previously
    public static void openAllTheVisitedDoors()
    {
        List<int> sceneList = GlobalController.Instance.getCurrentSceneList();
        
        foreach(KeyValuePair<int,OpenDoorLoad> door in doors)
        {
            if (sceneList.Contains((int)door.Value.Scene))
            {
                door.Value.playerCanOpen=true;
            }
        }

    }

    public static void openAllDoors()
    {
        List<int> sceneList = GlobalController.Instance.getCurrentSceneList();

        foreach (KeyValuePair<int, OpenDoorLoad> door in doors)
        {
           
                door.Value.playerCanOpen = true;
            
        }

    }

    public static int getIndexWithScene(GlobalController.Scenes scene)
    {
        //Debug.Log("You are looking for door to " + scene.ToString());
        foreach (KeyValuePair<int, OpenDoorLoad> door in doors)
        {
            //Debug.Log(door.Value.Scene);
            if ((int)door.Value.Scene == (int)scene) return door.Key;
        }

        //If fails to find scene
        return -1;
    }

    public static OpenDoorLoad getDoorSciptWithScene(GlobalController.Scenes scene)
    {
        int index=getIndexWithScene(scene);
        if (index == -1) return null;
        return doors[index].GetComponent<OpenDoorLoad>();

    }

    public static int numberOfActiveDoors()
    {
        int number = 0;
        foreach (KeyValuePair<int, OpenDoorLoad> door in doors)
        {
            if (door.Value.playerCanOpen) number++;
        }

        return number;
    }

    public static List<OpenDoorLoad> getAllActiveDoors()
    {
        List<OpenDoorLoad> activeDoors = new List<OpenDoorLoad>();
        foreach (KeyValuePair<int, OpenDoorLoad> door in doors)
        {
            if (door.Value.playerCanOpen) activeDoors.Add(door.Value);
        }

        return activeDoors;
    }

    public bool getSpawnPositionAndRotation(out Vector3 pos , out Quaternion rot)
    {
        
        pos = spawnObject.transform.position;
        rot = spawnObject.transform.rotation;

        return !(spawnObject==null);
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

    public void openDoor()
    {
        open = true;
    }
    public void closeDoor()
    {
        close = true;
    }

    public string getDoorName()
    {
        GlobalController.Language l = GlobalController.Instance.getLangueSetting();
        if (l == GlobalController.Language.ENG)
        {
            return doorNameENG;
        }else if (l == GlobalController.Language.TR)
        {
            return doorNameTR;    
        }
                

        return "";
        

    }

}
