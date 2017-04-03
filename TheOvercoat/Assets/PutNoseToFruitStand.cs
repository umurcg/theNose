using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class PutNoseToFruitStand : GameController, IClickAction, IClickActionDifferentPos
{

    public GameObject salesclerk;
    public GameObject noseCopy;
    public GameObject specificLocation;

    //max distance that player can move before interrupted by salesclerk
    public float moveAwayDistance = 5f;

    GameObject nose;
   

    // Use this for initialization
    public override void Start () {
        base.Start();

        if (player == null){

            enabled = false;
            return;

        }
        //Check is character is ivan. 
        if (player.name != "Ivan") enabled = false;

        nose = CharGameController.getObjectOfHand("nosePackage",CharGameController.hand.LeftHand);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator<float> _putNose()
    {
        print("putNose");
        nose.SetActive(false);
        noseCopy.SetActive(true);

        //Wait for player to try move away
        Vector3 initialPos = player.transform.position;
        while (Vector3.Distance(player.transform.position, initialPos) < moveAwayDistance)
        {
            //print(Vector3.Distance(player.transform.position, initialPos));
            yield return 0;

        }

        //Stop player and start subtitles
        pcc.StopToWalk();
        playerNma.Stop();
        sc.callSubtitle();

        Timing.RunCoroutine(Vckrs._lookTo(player, salesclerk.transform.position - player.transform.position, 1f));
        yield return Timing.WaitForSeconds(1f);
        while (subtitle.text != "") yield return 0;

        playerNma.Resume();
        playerNma.SetDestination(noseCopy.transform.position);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(player, 0));
        yield return Timing.WaitUntilDone(handlerHolder);

        nose.SetActive(true);
        noseCopy.SetActive(false);

        pcc.ContinueToWalk();

        Destroy(this);
        yield break;

    }

    public override void Action()
    {
        base.Action();
        Debug.Log("Child controller");
        Timing.RunCoroutine(_putNose());
    }

    public Vector3 giveMePosition()
    {
        //if (transform.childCount == 0)
        //{
        //    print("Add child object for specifiyng psoition");
        //    return Vector3.zero;
        //}

        return specificLocation.transform.position;
    }


    public override void activateController()
    {
        base.activateController();
        transform.tag = "ActiveObject";
    }
    public override void deactivateController()
    {
        base.deactivateController();
        transform.tag = "Untagged"; 
          

    }

}
