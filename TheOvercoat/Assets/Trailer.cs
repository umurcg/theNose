using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine.UI;


public class Trailer : MonoBehaviour {

    public AudioClip trailerMusic;
    public GameObject bigBook;
    public GameObject trailerIcon;
    public float zoomSpeed = 60;
    Camera cam;


    CameraController cc;

	// Use this for initialization
	void Start () {

        CharGameController.getCamera().SetActive(false);

        cam = GetComponentInChildren<Camera>();
        cc = cam.GetComponent<CameraController>();

        trailerIcon.SetActive(false);

        bigBook.GetComponent<BookAC>().openBook();

        float zoomAmount = 1000;

        //Vckrs.setCameraSize(cam, 1000);
        cc.zoomOut(zoomAmount);

        //Timing.RunCoroutine(Vckrs._cameraSizeRootFunc(cam, 10, zoomSpeed, 1f/*0.7f*/));
        Timing.RunCoroutine(cc._smoothZoomInFromVeryFar(1000,zoomSpeed,1f));

        CharGameController.getOwner().GetComponent<LevelMusicController>().setMusicManually(trailerMusic);


        Timing.RunCoroutine(startSubtitle());
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    IEnumerator<float> startSubtitle()
    {
        yield return Timing.WaitForSeconds(5f);
        GetComponent<SubtitleCaller>().callSubtitleWithIndexTime(0);

        yield return 0;

        Text narratorSubt = SubtitleFade.subtitles["NarratorSubtitle"];
        while (narratorSubt.text != "") yield return 0;

        blackScreen.script.fadeOut();

        yield break;
    }
}
