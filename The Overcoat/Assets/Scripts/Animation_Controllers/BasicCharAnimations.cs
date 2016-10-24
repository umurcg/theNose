using UnityEngine;
using System.Collections;

//_BasicCharAnimation.cs
//_Dependent to: 

//This script controls animations of player character.
//It adjust walking animation according to speed of object.
//It triggers idle animation when object doesnt move.


public class BasicCharAnimations : MonoBehaviour {
    public float threshold=0f;
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
        //RuntimeAnimatorController ac = anim.runtimeAnimatorController;
        //for(int i; i < ac.animationClips.Length; i++)
        //{
        //    if (ac.animationClips[i].name == "Walk")
        //        walk = ac.animationClips[i];
        //}
        
        

	}
	
	// Update is called once per frame
	void Update () {
        


        

        float dist = Vector3.Distance(transform.position, lastPosition);
        float speed =speedFactor* dist / Time.deltaTime;

        float rotation = Quaternion.Angle(transform.rotation, lastRotate);
		//float rotSpeed=speedFactor* rotation / Time.deltaTime;
       // print(rotation);

		if (dist > threshold) {
			anim.speed = speed;
			anim.SetBool ("Walk", dist > threshold);
            
			//anim.SetFloat("Blend", 0.5f + rotation);
		} 
        else
        {
            anim.SetBool("Walk", false);
            anim.speed = 1;
        }

     
//		else if (rotation > threshold) {
//			anim.speed = rotSpeed;
//			anim.SetBool ("Walk", rotation > threshold);
//		}

        lastPosition = transform.position;
        lastRotate = transform.rotation;



	}
}
