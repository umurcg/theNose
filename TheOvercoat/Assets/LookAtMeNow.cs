using UnityEngine;
using System.Collections;

//This script adds alwayslookat script to objects that having indicated tag when collider is triggered.
//Also it disables object navmesh if it is exist
public class LookAtMeNow : MonoBehaviour {

    public string aimedTag="";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Somehing triggered "+col.transform.name);
        if (col.transform.tag == aimedTag && !col.transform.GetComponent<AlwaysLookTo>())
        {
            GameObject triggeredObj = col.transform.gameObject;
            AlwaysLookTo alt=triggeredObj.AddComponent<AlwaysLookTo>();
            alt.aim = CharGameController.getActiveCharacter();

            UnityEngine.AI.NavMeshAgent nma = triggeredObj.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (nma) nma.enabled = false;

            //Destroy(col.transform.gameObject);
            //Debug.Log("Collider " + col.transform.name + " is triggered me");
        }
    }
}
