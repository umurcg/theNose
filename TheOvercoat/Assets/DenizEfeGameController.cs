using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class DenizEfeGameController : GameController {


    public FishGameController fgc;
    public FisherStandController fsc;
    public GameObject bucket;
    public GameObject kovalevFishingPos;
    public GameObject rod;

    public override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    void Update () {
	
	}

    public override void gameIsUsed()
    {
        base.gameIsUsed();
        gameObject.SetActive(false);
    }

    void callKovalev()
    {
        if(!isUsed())
            sc.callSubtitleWithIndex(2);
    }

    public void startConverstaion()
    {
        if (isUsed())
        {
            loopSubtitle();
        }
        else
        {
            Debug.Log("Startgin conversation");
            Timing.RunCoroutine(_startConverstaion());
        }

    }

    public void fishingGameIsFinished()
    {
        Timing.RunCoroutine(_fishingGameIsFinished());
    }

    IEnumerator<float> _startConverstaion()
    {

        sc.callSubtitleWithIndex(1);
        while (subtitle.text != "") yield return 0;

        
        playerNma.SetDestination(kovalevFishingPos.transform.position);
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(Vckrs.waitUntilStop(player)));
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(Vckrs._lookTo(player,kovalevFishingPos.transform.forward,1f)));

        instantiateRod();

        fgc.gameObject.SetActive(true);

    }
    
    void instantiateRod()
    {

        //Vector3 rodLocalPos = rod.transform.localPosition;
        //Quaternion rot = rod.transform.localRotation;

        //Duplcate rod

        rod = Instantiate(rod);
        rod.transform.parent = CharGameController.getHand(CharGameController.hand.LeftHand).transform;
        rod.transform.localPosition = new Vector3(-7.8f, 8.7f, 2.8f);
        rod.transform.localRotation = Quaternion.Euler(40.7f, 85.45f, -119.42f);
        rod.transform.localScale = Vector3.one * 2.25f;
        //rod.transform.localRotation = rot;
        //rod.transform.localPosition = rodLocalPos;



        playerAnim.SetBool("Fishing", true);
        rod.GetComponent<Animator>().SetTrigger("Throw");

    }

    public void loopSubtitle()
    {
        sc.callSubtitleWithIndex(3);
    }

    IEnumerator<float> _fishingGameIsFinished()
    {
        fgc.gameObject.SetActive(false);
        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "") yield return 0;

        registerAsUsed();
        ActivateAnotherObject.Activate(bucket);

        fsc.transform.tag = "ActiveObject";
        fsc.enabled = true;
        fsc.bucket = bucket;

        playerAnim.SetBool("Fishing", false);
        Destroy(rod);

        

        yield break;
    }

    public override void activateController()
    {
        gameObject.SetActive(true);
    }

    public override void deactivateController()
    {
        gameObject.SetActive(false);
    }


}
