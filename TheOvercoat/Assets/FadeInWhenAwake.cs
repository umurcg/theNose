using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

//Fades in when script starts
//Owner should have fade or transparent material
public class FadeInWhenAwake : MonoBehaviour {

    public float delay = 0;
    public float speed = 1f;

	// Use this for initialization
	void Start () {

        Renderer rend = GetComponent<Renderer>();
        Color c = rend.material.color;
        c.a = 0;
        rend.material.color = c;

        Timing.RunCoroutine(fadeIn());
    }

    IEnumerator<float> fadeIn()
    {
        yield return Timing.WaitForSeconds(delay);
       
        Timing.RunCoroutine(Vckrs._fadeObject(gameObject, speed));
        yield break;
    }


}
