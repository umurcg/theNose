using UnityEngine;
using System.Collections;

public class CharactrerAnimation : MonoBehaviour {
    Animator anim;
	// Use this for initialization
	void Awake () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

       // anim.SetFloat("walking", (int)(Input.GetAxis("Vertical")));
       
	}
}
