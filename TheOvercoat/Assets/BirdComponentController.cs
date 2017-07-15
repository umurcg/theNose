using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdComponentController : PlayerComponentController {

    BirdController birdController;
    MoveToWithoutAgent birdNma;
    

	// Use this for initialization
	protected override void Awake () {
        base.Awake();
        birdController = GetComponent<BirdController>();
        birdNma = GetComponent<MoveToWithoutAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void StopToWalk()
    {

        birdNma.stop();
        birdNma.enabled = false;
        cl.enabled = false;
        cc.enabled = false;


        bcanPlayerWalk = false;

    }

    public override void ContinueToWalk()
    {


        birdNma.enabled = true;
        cl.enabled = true;
        cc.enabled = true;


        bcanPlayerWalk = true;

   

    }


    public override void pauseNma()
    {
        birdNma.stop();
    }

    public override void disableNma()
    {
        pauseNma();
        birdNma.enabled = false;
    }

    public override void enableNma()
    {
        birdNma.enabled = true;
        //nma.Resume();
    }

}
