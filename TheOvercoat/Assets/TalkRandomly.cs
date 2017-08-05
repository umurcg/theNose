using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkRandomly : MonoBehaviour {

    public int subtIndex;
    public float maxTimeBetweenTalks,minTimeBetweenTalks;
    SubtitleCaller sc;
    Timer timer;

	// Use this for initialization
	void Start () {
        timer = new Timer(randomTime());
        sc = GetComponent<SubtitleCaller>();
	}
	
	// Update is called once per frame
	void Update () {

        if (timer.ticTac(Time.deltaTime))
        {
            Debug.Log("CALLİGN SUBT");
            sc.callRandomSubtTime(subtIndex);
            timer = new Timer(randomTime());
        }

	}

    float randomTime (){ return Random.Range(maxTimeBetweenTalks, maxTimeBetweenTalks); }
}
