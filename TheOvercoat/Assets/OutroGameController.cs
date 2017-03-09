using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class OutroGameController : GameController {

    public GameObject cameraObj,cameraInitialPosition , kovalev, ivan, berberShop, building, ivanAim;
    characterComponents kovCC, ivanCC;


    Vector3 berberShopPos;

	// Use this for initialization
	public override void Start () {
        base.Start();

        berberShopPos = cameraObj.transform.position;

        ivanCC = new characterComponents(ivan);
        kovCC = new characterComponents(kovalev);

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
        //Time.timeScale = 10f;

        kovCC.animator.SetBool("Sit", true);
        ivanCC.animator.SetBool("Hands", true);
        ivan.transform.position = ivanAim.transform.position;
        ivan.transform.LookAt(kovalev.transform.position);

        //Wait for one frame
        yield return 0;

        //Make camera zoom in
        Camera camScr = cameraObj.GetComponent<Camera>();
        camScr.orthographicSize = 20;
        Timing.RunCoroutine(Vckrs._cameraSize(camScr, 10, 0.3f));

        //Set camera intiial pso
        cameraObj.transform.position = cameraInitialPosition.transform.position;

        //Start camera to move through city
        handlerHolder= Timing.RunCoroutine(Vckrs._Tween(cameraObj, berberShopPos, 0.01f));

        //Wait for 3 seconds
        yield return Timing.WaitForSeconds(3f);

        //Start narrator subtitile
        sc.callSubtitleWithIndexTime(0);

        yield return Timing.WaitUntilDone(handlerHolder);
        
        Timing.RunCoroutine(Vckrs._fadeObject(building, 1f,false));

        sc.callSubtitleWithIndex(1);
        while (subtitle.text != "") yield return 0;

        sc.callSubtitleWithIndexTime(1);
        handlerHolder=Timing.RunCoroutine(Vckrs._cameraSize(camScr, 50, 1f));

        yield return Timing.WaitForSeconds(3f);
        Timing.RunCoroutine(Vckrs._fadeObject(building, 1f, false));

        yield return Timing.WaitUntilDone(handlerHolder);

        Timing.RunCoroutine(blackScreen.script.fadeOut());

        while (narSubtitle.text != "") yield return 0;
        
        Application.Quit();

        yield break;
    }

    public override void deactivateController()
    {
        base.deactivateController();
        gameObject.SetActive(false);
    }

    public override void activateController()
    {
        base.activateController();
        gameObject.SetActive(true);
    }
}
