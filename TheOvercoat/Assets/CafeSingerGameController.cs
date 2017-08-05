using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class CafeSingerGameController : GameController {

    public GameObject Kovalev, CameraObj, kovalevStartPoint, /*kovalevFirstPoint*/ mirror, chair1, chair2, building, sun, city, lady1, lady2;
    

    characterComponents kovalevCC;
    Camera cam;

    float zoomAmount = 20;
    CameraController cc;

    public GameObject acitvecharacter;

    // Use this for initialization
    public override void Start () {
        base.Start();

        Debug.Log("Cafe singer start");

        //Disable playerCamera object
        //CharGameController.getOwner().SetActive(false);
        acitvecharacter = CharGameController.getActiveCharacter();
        acitvecharacter.SetActive(false);
        CharGameController.getCamera().SetActive(false);

        kovalevCC = new characterComponents(Kovalev);
        cam = CameraObj.GetComponent<Camera>();

        //Set camera object rotation
        //CameraObj.transform.eulerAngles = new Vector3(30, 135, 0);

        cc =  CameraObj.GetComponent<CameraController>();

        Timing.RunCoroutine(_start());

    }
	
	// Update is called once per frame
	void Update () {

    
    }

    void recoverPlayerAndCam()
    {
        acitvecharacter.SetActive(true);
        CharGameController.getCamera().SetActive(true);
    }
    

    IEnumerator<float> _start()
    {


        //Time.timeScale = 0;
        
        WhoIsTalking.self.setCameraComponent(CameraObj.GetComponent<Camera>());

        ////Register kovalev to see throug objects script
        //SeeCharacterThroughObjects scto = city.GetComponent<SeeCharacterThroughObjects>();
        //scto.registerToTargets(Kovalev);

        ////Change camera to cafe singer camera
        //scto.changeCamera(CameraObj);

        //cam.orthographic = true;
        //yield return 0;
        //cam.orthographic = false;

        //cam.orthographicSize = 30;

        cc.zoomOut(zoomAmount);

    

        //Debug.Log("CafeSingerController");
        //Kovalev.transform.position = kovalevStartPoint.transform.position;
        kovalevCC.navmashagent.SetDestination(mirror.transform.position);

  
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Kovalev));


        //Timing.RunCoroutine(Vckrs._cameraSize(cam, 10, 0.5f));

        yield return 0;


        Timing.RunCoroutine(cc._smoothZoomIn(zoomAmount, 1f));


        //Narrtor subtitle
        sc.callSubtitleWithIndexTime(0);

        //Wait for finis walk
        yield return Timing.WaitUntilDone(handlerHolder);


        while (Vector3.Distance(Kovalev.transform.position,mirror.transform.position)>3)
        {
            yield return 0;
        }



        //Kovalev mirror talk
        Vector3 mirrorLookPos =  new Vector3(1, 0, -1);
        //handlerHolder = Timing.RunCoroutine(Vckrs._lookTo(Kovalev, mirror.transform.parent.gameObject,1f));
        handlerHolder = Timing.RunCoroutine(Vckrs._lookTo(Kovalev, mirrorLookPos, 1f));
        yield return Timing.WaitUntilDone(handlerHolder);

        while (narSubtitle.text != "") yield return 0;

        sc.callSubtitleWithIndex(1);

        while (subtitle.text != "") yield return 0;

        kovalevCC.navmashagent.SetDestination(chair1.transform.position + Vector3.Normalize(Kovalev.transform.position - chair1.transform.position));

        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Kovalev));
        yield return Timing.WaitUntilDone(handlerHolder);

        handlerHolder= Timing.RunCoroutine(chair1.GetComponent<WalkLookAnim>()._sit());
        yield return Timing.WaitUntilDone(handlerHolder);

        yield return Timing.WaitForSeconds(1f);

        sc.callSubtitleWithIndex(2);
        while (subtitle.text != "")
        {
            yield return 0;
         
        }
        
        handlerHolder = Timing.RunCoroutine(chair1.GetComponent<WalkLookAnim>()._getUp());
        yield return Timing.WaitUntilDone(handlerHolder);

        

        kovalevCC.navmashagent.SetDestination(chair2.transform.position + Vector3.Normalize(Kovalev.transform.position - chair2.transform.position));


        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Kovalev));
        yield return Timing.WaitUntilDone(handlerHolder);

    

        handlerHolder = Timing.RunCoroutine(chair2.GetComponent<WalkLookAnim>()._sit());
        yield return Timing.WaitUntilDone(handlerHolder);

        yield return Timing.WaitForSeconds(1f);

        sc.callSubtitleWithIndex(3);
        while (subtitle.text != "") yield return 0;


        sc.callSubtitleWithIndexTime(1);

        //Timing.RunCoroutine(Vckrs._cameraSize(cam, 30, 0.5f));
        Timing.RunCoroutine(cc._smoothZoomOut(zoomAmount, 1f));

        while (SubtitleFade.subtitles["NarratorSubtitle"].text != "") yield return 0;

        yield return Timing.WaitForSeconds(1f);

        GetComponent<LoadScene>().Load();

        while (blackScreen.script.getAlpha() < 0.9f) yield return 0;

        recoverPlayerAndCam();
        

        yield break;
    }

    void OnDestroy()
    {
        CharGameController.getOwner().SetActive(true);
       
    }

    public override void activateController()
    {
        base.activateController();

        Kovalev.SetActive(true);
        CameraObj.SetActive(true);
        sun.SetActive(true);

        lady1.SetActive(true);
        lady2.SetActive(true);

        enabled = true;

     
    }

    public override void deactivateController()
    {
        //gameObject.SetActive(false);
        Kovalev.SetActive(false);
        CameraObj.SetActive(false);
        sun.SetActive(false);


        lady1.SetActive(false);
        lady2.SetActive(false);


        enabled = false;
    }
}
