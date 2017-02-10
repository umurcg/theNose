using UnityEngine;
using System.Collections;
using MovementEffects;

//Fades in when script starts
//Owner should have fade or transparent material
public class FadeInWhenAwake : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Renderer rend = GetComponent<Renderer>();
        Color c = rend.material.color;
        c.a = 0;
        rend.material.color = c;
        Timing.RunCoroutine(Vckrs._fadeObject(gameObject, 1f));
    }


}
