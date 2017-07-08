using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class FadeAndDie : MonoBehaviour {

    public float speed;
    Renderer rend;

	// Use this for initialization
	void Awake () {
        rend = GetComponentInChildren<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void fadeAndDie()
    {
        Timing.RunCoroutine(_fadeAndDie());
    }
    IEnumerator<float> _fadeAndDie()
    {
        yield return 0;
       yield return Timing.WaitUntilDone(Timing.RunCoroutine(Vckrs._fadeObjectOut(rend.gameObject, speed)));
        Destroy(gameObject);
        yield break;
    }
}
