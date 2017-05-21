using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterSceneGameController : GameController{

    public GameObject  building, ivan, kovalev, head, aims,cameraObj, sun, bigBook, tutorials;



    Camera cam;
    NavMeshAgent ivanNma, kovalevNma;
    Animator ivanAnim, kovalevAnim;

    //public GameObject videoPlayerObj;
    //public MovieTexture enterVideo;
    //VideoPlayer videoPlayer;

    //public bool recordVideo;

    public override void Awake()
    {
        base.Awake();
        //videoPlayer = videoPlayerObj.GetComponent<VideoPlayer>();

    }

    // Use this for initialization
    public override void Start () {

        base.Start();

        //At enter scene blackscreen shouldn't fade in automatically
        if(blackScreen.script!=null)
            blackScreen.script.fadeInAtStart = false;  //race condition

        if (player != null)
        {
            this.enabled = false;
            Debug.Log("No player");
            return;
        }

        ivanNma = ivan.GetComponent<NavMeshAgent>();
        kovalevNma = kovalev.GetComponent<NavMeshAgent>();
        cam = cameraObj.GetComponent<Camera>();
        

        ivanAnim = ivan.GetComponent<Animator>();
        kovalevAnim = kovalev.GetComponent<Animator>();


        WhoIsTalking wit = subtitle.GetComponent<WhoIsTalking>();
        if (wit != null)
        {
            wit.characters.Clear();
            wit.characters.Add("Kovalev", kovalev);
            wit.characters.Add(ivan.name, ivan);
            wit.setCameraComponent(cam);
        }else
        {
            Debug.Log("Couldnt find who is talking script");
        }

        
        if (enabled)
            Timing.RunCoroutine(_intro());



    }
	
    void OnDisable()
    {
        //Debug.Log("Disabliiiiiiiiiiiing");
        CharGameController.getOwner().GetComponent<CursorImageScript>().forceToDefault = false;
    }

	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator<float> _intro()
    {
        yield return 0;


        //Force to default cursor while this scene is not interactive
        CharGameController.getOwner().GetComponent<CursorImageScript>().forceToDefault = true;




        //yield return Timing.WaitForSeconds(25);


        //Timing.RunCoroutine(Vckrs._cameraSize(cam, 10, 20/*0.7f*/));


        //yield return Timing.WaitUntilDone(fadeHandler);

        //bigBook.SetActive(true);
        //Animator bookAnim = bigBook.GetComponent<Animator>();
        //bookAnim.SetBool("")


        //if (recordVideo)
        //{

        //VRCapture.VRCapture.Instance.StartCapture();

        sc.callSubtitleWithIndexTime(0);

        yield return Timing.WaitForSeconds(5f);
        
        IEnumerator<float> fadeHandler = blackScreen.script.fadeIn();

        bigBook.GetComponent<BookAC>().openBook();

        cam.orthographicSize = 1000;

        if (cam.orthographic)
        {

            Timing.RunCoroutine(Vckrs._cameraSizeRootFunc(cam, 10, 55, 1f/*0.7f*/));

            while (cam.orthographicSize != 10 || narSubtitle.text != "")
            {
                yield return 0;
            }
        }
        else
        {

            //TODO solve far terrain issue
            float far = cam.farClipPlane;
            cam.farClipPlane = 10000;

            float camMoveSpeed = 0.1f;
            Vector3 aim = cam.transform.position;
            cam.transform.position = cam.transform.position - cam.transform.forward * 8000;
            handlerHolder = Timing.RunCoroutine(Vckrs._TweenRootFunc(cam.gameObject, aim, camMoveSpeed, 5f));
            yield return Timing.WaitUntilDone(handlerHolder);

            cam.farClipPlane = far;
        }

        //VRCapture.VRCapture.Instance.StopCapture();

        //}
        //else
        //{
        //    blackScreen.script.setAsTransparent();

        //    videoPlayer.mt = enterVideo;
        //    videoPlayer.play();

        //    yield return 0;

        //    yield return Timing.WaitForSeconds(5f);
        //    sc.callSubtitleWithIndexTime(0);

        //    while (videoPlayer.isPlaying) yield return 0;

        //}


        tutorials.GetComponent<TutorailCanvas>().startTutorial(5);

        ivanNma.SetDestination(aims.transform.GetChild(0).position);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(ivan));
        Timing.RunCoroutine(Vckrs._fadeObject(building, 1f,false));
        sc.callSubtitleWithIndex(3);
        yield return Timing.WaitUntilDone(handlerHolder);
        ivanAnim.SetBool("Hands", true);


        while (subtitle.text != "")
        {
            yield return 0;
        }

        ivanAnim.SetBool("Hands", false);
        ivanNma.SetDestination(aims.transform.GetChild(1).position);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(ivan));
        yield return Timing.WaitUntilDone(handlerHolder);


        ivanAnim.SetBool("Hands", true);
        sc.callSubtitleWithIndex(4);
        while (subtitle.text != "")
        {
            yield return 0;
        }
        ivanAnim.SetBool("Hands", false);

        ivanNma.SetDestination(aims.transform.GetChild(2).position);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(ivan, 0));
        yield return Timing.WaitUntilDone(handlerHolder);

        ivanAnim.SetBool("Head", true);
        sc.callSubtitleWithIndex(5);
        while (subtitle.text != "")
        {
            yield return 0;
        }
        ivanAnim.SetBool("Head",false);

        ivanNma.SetDestination(aims.transform.GetChild(3).position);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(ivan, 0));
        yield return Timing.WaitUntilDone(handlerHolder);

        ivanAnim.SetBool("Hands", true);
        sc.callSubtitleWithIndex(6);
        while (subtitle.text != "")
        {
            yield return 0;
        }
        ivanAnim.SetBool("Hands", false);


        ivanNma.SetDestination(aims.transform.GetChild(4).position);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(ivan, 0));
        yield return Timing.WaitUntilDone(handlerHolder);

        ivanAnim.SetBool("Hands", true);

        sc.callSubtitleWithIndexTime(1);
        while (narSubtitle.text != "")
        {
            yield return 0;
        }

        yield return Timing.WaitForSeconds(3);
        head.SetActive(true);

        sc.callSubtitleWithIndexTime(2);
        while (narSubtitle.text != "")
        {
            yield return 0;
        }

        tutorials.GetComponent<TutorailCanvas>().startTutorial(3);

        RemoveSquares rs = head.transform.GetChild(1).GetComponent<RemoveSquares>();
        rs.enabled = true;


    }
    public void outro()
    {
        Timing.RunCoroutine(_outro());
    }

    IEnumerator<float> _outro()
    {

        head.SetActive(false);
        ivanAnim.SetBool("Hands", false);
        kovalevAnim.SetBool("Sit", false);

        handlerHolder= Timing.RunCoroutine(Vckrs._Tween(kovalev, kovalev.transform.position + kovalev.transform.forward, 1f));
        yield return Timing.WaitUntilDone(handlerHolder);

        kovalevNma.enabled = true;

        kovalevNma.SetDestination(aims.transform.GetChild(6).position);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(kovalev, 0));

        ivanNma.SetDestination(aims.transform.GetChild(5).position);
        IEnumerator<float> handlerHolder2 = Timing.RunCoroutine(Vckrs.waitUntilStop(ivan, 0));

        yield return Timing.WaitUntilDone(handlerHolder);

        Timing.RunCoroutine(Vckrs._lookTo(kovalev, kovalev.transform.right, 1));

        yield return Timing.WaitUntilDone(handlerHolder2);
        AlwaysLookTo alt= ivan.GetComponent<AlwaysLookTo>();
        alt.enabled = true;

        sc.callSubtitleWithIndex(7);
        while (subtitle.text != "")
        {
            yield return 0;
        }

        kovalevNma.SetDestination(aims.transform.GetChild(7).position);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(kovalev, 0));
        yield return Timing.WaitUntilDone(handlerHolder);

        sc.callSubtitleWithIndex(8);
        handlerHolder = Timing.RunCoroutine(Vckrs._lookTo(kovalev, ivan.transform.position - kovalev.transform.position, 1f));
        yield return Timing.WaitUntilDone(handlerHolder);

        while (subtitle.text != "")
        {
            yield return 0;
        }

        kovalevNma.SetDestination(aims.transform.GetChild(8).position);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(kovalev, 0));


        Timing.RunCoroutine(Vckrs._cameraSize(cam, 30, 1f));

        sc.callSubtitleWithIndexTime(3);

        yield return Timing.WaitForSeconds(2f);
        Timing.RunCoroutine(Vckrs._fadeObject(building, 1f, false));

        while (narSubtitle.text != "")
        {
            yield return 0;
        }


        if (cam.orthographicSize != 30)
        {
            yield return 0;
        } 

        handlerHolder = blackScreen.script.fadeOut();
        yield return Timing.WaitUntilDone(handlerHolder);
        SceneManager.LoadScene((int)GlobalController.Scenes.IvanHouse);
    }

    public override void activateController() {

        base.activateController();
        //gameObject.SetActive(true);
        
        enabled = true;
        cameraObj.SetActive(true);
        ivan.SetActive(true);
        kovalev.SetActive(true);
        sun.SetActive(true);
    }
    public override void deactivateController() {

        base.deactivateController();
        //gameObject.SetActive(false);
        enabled = false;
        cameraObj.SetActive(false);
        ivan.SetActive(false);
        kovalev.SetActive(false);
        sun.SetActive(false);

    }

}
