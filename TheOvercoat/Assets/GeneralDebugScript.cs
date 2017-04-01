using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using MovementEffects;
using System.Collections.Generic;

public class GeneralDebugScript : GameController {

	// Use this for initialization
	public override void Start () {
        base.Start();
        Timing.RunCoroutine(subtitleCaller());
        //SceneManager.LoadScene(3);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator<float> subtitleCaller()
    {
        sc.callSubtitleWithIndex(0);

        while (subtitle.text == "") yield return 0;

        sc.callSubtitleWithIndexTime(0);

        yield break;
    }
}
