using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class CafeSingerGameController : GameController {

    public GameObject Kovalev, CameraObj, kovalevStartPoint, /*kovalevFirstPoint*/ mirror, chair1, chair2, building, sun, city;

    characterComponents kovalevCC;
    Camera cam;
	// Use this for initialization
	public override void Start () {
        base.Start();

        Debug.Log("Cafe singer start");

        //Disable playerCamera object
        CharGameController.getOwner().SetActive(false);
        kovalevCC = new characterComponents(Kovalev);
        cam = CameraObj.GetComponent<Camera>();

        //Set camera object rotation
        CameraObj.transform.eulerAngles = new Vector3(30, 135, 0);

        Timing.RunCoroutine(_start());

    }
	
	// Update is called once per frame
	void Update () {

    
    }

    IEnumerator<float> _start()
    {
        WhoIsTalking.self.setCameraComponent(CameraObj.GetComponent<Camera>());

        ////Register kovalev to see throug objects script
        //SeeCharacterThroughObjects scto = city.GetComponent<SeeCharacterThroughObjects>();
        //scto.registerToTargets(Kovalev);

        ////Change camera to cafe singer camera
        //scto.changeCamera(CameraObj);


        cam.orthographicSize = 30;

        //Debug.Log("CafeSingerController");
        Kovalev.transform.position = kovalevStartPoint.transform.position;
        kovalevCC.navmashagent.SetDestination(mirror.transform.position-mirror.transform.up*1);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Kovalev));

        
        Timing.RunCoroutine(Vckrs._cameraSize(cam, 10, 0.5f));


        //Narrtor subtitle
        sc.callSubtitleWithIndexTime(0);

        //Wait for finis walk
        yield return Timing.WaitUntilDone(handlerHolder);

        //Kovalev mirror talk
        handlerHolder= Timing.RunCoroutine(Vckrs._lookTo(Kovalev, mirror,1f));
        yield return Timing.WaitUntilDone(handlerHolder);
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

        Timing.RunCoroutine(Vckrs._cameraSize(cam, 30, 0.5f));

        while (SubtitleFade.subtitles["NarratorSubtitle"].text != "") yield return 0;

        yield return Timing.WaitForSeconds(1f);

        GetComponent<LoadScene>().Load();

        

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

        enabled = true;

     
    }

    public override void deactivateController()
    {
        //gameObject.SetActive(false);
        Kovalev.SetActive(false);
        CameraObj.SetActive(false);
        sun.SetActive(false);
                
        enabled = false;
    }
}
