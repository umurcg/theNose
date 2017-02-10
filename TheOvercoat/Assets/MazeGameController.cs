using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class MazeGameController : MonoBehaviour {
    public GameObject[] spawnPoints;
    public int maxNumberOfTrial;
    public float waitSecondsBetweenCatches = 2f;

    //For finish function message
    public GameObject finishMessageObject;
    public string finishMessage;

    int numberOfCatch = 0;
    IEnumerator<float> handler;

    GameObject caller;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void catched(Object callerObject)
    {
  
        caller = (GameObject)callerObject;
        Timing.RunCoroutine(_catched());
    }

    IEnumerator<float> _catched()
    {
        //Debug.Log("_catched");
        if (numberOfCatch<maxNumberOfTrial) {
            handler = blackScreen.script.fadeOut();
            yield return Timing.WaitUntilDone(handler);

            yield return Timing.WaitForSeconds(waitSecondsBetweenCatches);
            GameObject player = CharGameController.getActiveCharacter();
            player.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].transform.position;
            PlayerComponentController ppp=player.GetComponent<PlayerComponentController>();
            ppp.ContinueToWalk();

            yield return Timing.WaitForSeconds(0.5f);

            blackScreen.script.fadeIn();
            numberOfCatch++;


            PolicePatrol pp= caller.GetComponent<PolicePatrol>();  //TODO more generic approach ie dont use policepatrol for calling finish function
            caller = null;
            if (pp != null)
            {
                pp.Resume();
            }else
            {
                Debug.Log("No police patrol script");
            }

        } else
        {
            finishGame();
        }
    }

    void finishGame()
    {
        if (finishMessageObject == null)
        {
            Debug.Log("No message object");
            return;
        }

        finishMessageObject.SendMessage(finishMessage);
    }
}
