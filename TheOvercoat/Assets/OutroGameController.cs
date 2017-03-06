using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class OutroGameController : GameController {

    public GameObject cameraObj,cameraInitialPosition , kovalev, ivan, berberShop ;

    Vector3 berberShopPos;

	// Use this for initialization
	public override void Start () {
        base.Start();

        berberShopPos = cameraObj.transform.position;

        outro();

	}
	
	// Update is called once per frame
	void Update () {

        
	
	}

    public void outro()
    {
        Timing.RunCoroutine(_outro());
    }

    IEnumerator<float> _outro()
    {
        cameraObj.transform.position = cameraInitialPosition.transform.position;

        Timing.RunCoroutine(Vckrs._Tween(cameraObj, berberShopPos, 1f));

        

        yield break;
    }
}
