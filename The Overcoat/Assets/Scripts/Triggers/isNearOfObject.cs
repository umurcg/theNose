using UnityEngine;
using System.Collections;

//This script detects wheter or not obj near of owner object.
//maxDistance is the point when script triggers interface method of INearObjectAction


//This scripts call action method when object is near of another object.
public class isNearOfObject : MonoBehaviour {
    public GameObject obj;
    public float maxDist;


    ////This string holds the tag of obj.
    ////It is added for prefabs.
    ////In awake scripts adds object that hold the tag to obj variable.
    //public string findObjectInAwakeWithTag;

    //void Awake()
    //{
    //    if(findObjectInAwakeWithTag!="")
    //    obj = GameObject.FindGameObjectWithTag(findObjectInAwakeWithTag);
    //}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    //    print(Vector3.Distance(transform.position, obj.transform.position));
        if (Vector3.Distance(transform.position, obj.transform.position)<maxDist)
        {
            //print("NEAR");
            INearObjectAciton noa = GetComponent<INearObjectAciton>();
            noa.noAction();
            enabled = false;

        }
	
	}


}
