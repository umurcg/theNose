using UnityEngine;
using System.Collections;

public class AnimationLegacyLoop : MonoBehaviour {

    Animation anim;
    public string animationName;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!anim.IsPlaying(animationName))
        {
            anim.Play();
        }    
	}
}
