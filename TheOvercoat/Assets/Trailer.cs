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

	// Use this for initialization
	void Start () {

        CharGameController.getCamera().SetActive(false);

        cam = GetComponentInChildren<Camera>();

        trailerIcon.SetActive(false);

        bigBook.GetComponent<BookAC>().openBook();
        cam.orthographicSize = 1000;
        Timing.RunCoroutine(Vckrs._cameraSizeRootFunc(cam, 10, zoomSpeed, 1f/*0.7f*/));

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
