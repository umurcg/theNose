using UnityEngine;
using System.Collections;
using MovementEffects;

public class VickersTestScript : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
        //Time.timeScale = 0.3f;
        testTweenHeight();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //TweenHeigthTest
    void testTweenHeight()
    {
        Timing.RunCoroutine(Vckrs._TweenSinHeight(gameObject, player.transform.position, 1f));
    }
}
