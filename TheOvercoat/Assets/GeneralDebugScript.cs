using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using MovementEffects;
using System.Collections.Generic;

public class GeneralDebugScript : GameController {

    //string prisonerGC = "PoliceStationPrisoner";
    public GameObject other;


	// Use this for initialization
	public override void Start () {
        base.Start();
        //Timing.RunCoroutine(subtitleCaller());
        //SceneManager.LoadScene(3);

        //if (GlobalController.Instance.isGameControllerIsUsedSceneNameAndGameObjectName(prisonerGC))
        //{
        //    sc.callSubtitleWithIndex(0);
        //}else
        //{
        //    sc.callSubtitleWithIndex(1);
        //}
        

    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Vector3.Distance(other.transform.position, transform.position));

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == this.other)
        {
            Debug.Log("Insiiiiiiiiiiiiiiide");
       
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == this.other)
        {
            Debug.Log("Exiiiiiiiiiiiit");
        }
    }

    //IEnumerator<float> subtitleCaller()
    //{
    //    sc.callSubtitleWithIndex(0);

    //    while (subtitle.text == "") yield return 0;

    //    sc.callSubtitleWithIndexTime(0);

    //    yield break;
    //}
}
