using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class TestPace : MonoBehaviour {

    public bool interrupt = false;
    IEnumerator<float> handler;

	// Use this for initialization
	void Start () {
       handler= Timing.RunCoroutine(Vckrs._pace(gameObject, transform.right * 20, -transform.right * 20));
	}
	
	// Update is called once per frame
	void Update () {
        if (interrupt)
        {
            interrupt = false;
            Timing.KillCoroutines(handler);

        }
	}
}
