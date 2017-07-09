using UnityEngine;
using System.Collections;

//This script is force to object stop and look to an game object.
//It is called by InearObjectAction interface
//It is uses AlwayLookTo script
//If owner has walk the fares road (generally bot object has that) it disables it.
//Stopping perfomred with stopping navmesh agent.

public class StopAndLookWhenNear : MonoBehaviour, INearObjectAciton {

    //While this script is used generally by prefab AlwaysLookTo and IsNearOfObject must be filled with object.

    //This string holds the tag of obj.
    //It is added for prefabs.
    //In awake scripts adds object that hold the tag to obj variable.
    public string findObjectInAwakeWithTag;

    void Awake()
    {
        AlwaysLookTo alt = GetComponent<AlwaysLookTo>();
        isNearOfObject ino = GetComponent<isNearOfObject>();

        if (findObjectInAwakeWithTag != "")
        {
            GameObject obj = GameObject.FindGameObjectWithTag(findObjectInAwakeWithTag);
            alt.aim = obj;
            ino.obj = obj;

        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void noAction()
    {
    UnityEngine.AI.NavMeshAgent nma=    GetComponent<UnityEngine.AI.NavMeshAgent>();
        WalkToFarestOfRoadBot wfr = GetComponent<WalkToFarestOfRoadBot>();
        AlwaysLookTo alt = GetComponent<AlwaysLookTo>();
        if (wfr != null)
            wfr.enabled = false;
        if (nma != null)
            nma.Stop();
        if (alt != null)
            alt.enabled = true;
        

    }

}
