using UnityEngine;
using System.Collections;

//_BasicCharAnimation.cs
//_Dependent to: 

//This script controls animations of player character.
//It adjust walking animation according to speed of object.
//It triggers idle animation when object doesnt move.


public class BasicCharAnimations : MonoBehaviour {
    public float threshold=0f;
    public string animName="Walk";
    Vector3 lastPosition;
    Animator anim;
    AnimationClip walk;
    public float speedFactor = 0.5f;

    float angle;
    Quaternion lastRotate;
    float rotTreshold = 0.05f;
	// Use this for initialization
	void Awake () {
        anim = GetComponent<Animator>();
        lastPosition = transform.position;
        lastRotate = transform.rotation;

        
        

	}
	
	// Update is called once per frame
	void Update () {
        
          

        float dist = Vector3.Distance(transform.position, lastPosition);
        float speed =speedFactor* dist / Time.deltaTime;

        float rotation = Quaternion.Angle(transform.rotation, lastRotate);


		if (dist > threshold) {
			anim.speed = speed;
			anim.SetBool (animName, dist > threshold);
 
		} 
        else
        {
            anim.SetBool(animName, false);
            anim.speed = 1;
        }


        lastPosition = transform.position;
        lastRotate = transform.rotation;



	}
}
