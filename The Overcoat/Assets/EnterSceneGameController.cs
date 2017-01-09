using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnterSceneGameController : GameController{

    public GameObject blackScreen, building, ivan, kovalev, head, aims,cameraObj;

    Camera cam;
    NavMeshAgent ivanNma, kovalevNma;
    Animator ivanAnim, kovalevAnim;

    // Use this for initialization
    public override void Start () {

        base.Start();

        if (player != null)
        {
            this.enabled = false;
            return;
        }

        ivanNma = ivan.GetComponent<NavMeshAgent>();
        kovalevNma = kovalev.GetComponent<NavMeshAgent>();
        cam = cameraObj.GetComponent<Camera>();
        cam.orthographicSize = 40;

        ivanAnim = ivan.GetComponent<Animator>();
        kovalevAnim = kovalev.GetComponent<Animator>();

        RawImage r = blackScreen.GetComponent<RawImage>();
        Color textureColor = r.color;
        textureColor.a = 1;
        r.color = textureColor;

        Timing.RunCoroutine(_intro());

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator<float> _intro()
    {
        yield return Timing.WaitForSeconds(5f);

        sc.callSubtitleWithIndexTime(0);

        yield return Timing.WaitForSeconds(25);

        Timing.RunCoroutine(Vckrs._fadeInfadeOut(blackScreen, 1f));
        Timing.RunCoroutine(Vckrs._cameraSize(cam, 10, 0.7f));

        while (cam.orthographicSize!=10 || narSubtitle.text!="")
        {
            yield return 0;
        }


        ivanNma.SetDestination(aims.transform.GetChild(0).position);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(ivan, 0));
        Timing.RunCoroutine(Vckrs._fadeObject(building, 1f));
        sc.callSubtitleWithIndex(3);
        yield return Timing.WaitUntilDone(handlerHolder);
        ivanAnim.SetBool("Hands", true);


        while (subtitle.text != "")
        {
            yield return 0;
        }

        ivanAnim.SetBool("Hands", false);
        ivanNma.SetDestination(aims.transform.GetChild(1).position);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(ivan, 0));
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
        while (narSubtitle.text != "")
        {
            yield return 0;
        }


        if (cam.orthographicSize != 30)
        {
            yield return 0;
        } 

        handlerHolder= Timing.RunCoroutine(Vckrs._fadeInfadeOut(blackScreen, 1f));
        yield return Timing.WaitUntilDone(handlerHolder);

    }
}
