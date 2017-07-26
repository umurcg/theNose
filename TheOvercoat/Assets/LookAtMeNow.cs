using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//This script adds alwayslookat script to objects that having indicated tag when collider is triggered.
//Also it disables object navmesh if it is exist

//Also character should complaint about this so
//it will trigger randomly subtitle
//Contidions for subtitle:
//There is a timer that should complete
//At least one subject should be enter
//Also probability must be match
//Also there shouldnt be any subtitle


public class LookAtMeNow : MonoBehaviour {

    public string aimedTag="";
    // Use this for initialization


    
    public float complainProbability = 50;
    public float timeBetwenComplains = 100;
    Timer timer;

    SubtitleCaller sc;
    Text subt;

	void Start () {

        complainProbability = Mathf.Clamp(complainProbability, 0, 100);
        timer = new Timer(timeBetwenComplains);
        timer.autoReset = false;

        sc = GetComponent<SubtitleCaller>();
        subt= SubtitleFade.subtitles["CharacterSubtitle"];
        if (subt == null) Debug.Log("Couldnt find character subtitle");
	}
	
	// Update is called once per frame
	void Update () {

        timer.ticTac(Time.deltaTime);
        //Debug.Log("is time over " + timer.isTimeOver());
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

            if (timer.isTimeOver() && Random.Range(0, 100) <= complainProbability) complain();

            //Destroy(col.transform.gameObject);
            //Debug.Log("Collider " + col.transform.name + " is triggered me");
        }
    }

    void complain()
    {
        if(subt==null) subt = SubtitleFade.subtitles["CharacterSubtitle"];

        //Debug.Log("Complaining");
        if (subt.text != "") return;
        

        sc.callRandomSubtTime(0);
        timer.resetTimet();

    }


}
