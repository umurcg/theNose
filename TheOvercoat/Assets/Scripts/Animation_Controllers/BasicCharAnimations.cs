using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//_BasicCharAnimation.cs
//_Dependent to: 

//This script controls animations of player character.
//It adjust walking animation according to speed of object.
//It triggers idle animation when object doesnt move.


public class BasicCharAnimations : MonoBehaviour {
    public float threshold=0.01f;
    public string animName="Walk";
    Vector3 lastPosition;
    Animator anim;
    AnimationClip walk;
    public float speedFactor = 0.5f;

    //float angle;
    //Quaternion lastRotate;
    //float rotTreshold = 0.05f;

    //After movement stop a timer will be triggered for preventing odd walks.
    public float timeBeforeStopAnimation=0.1f;

    bool stoped;
    IEnumerator<float> handler=null;

	// Use this for initialization
	void Awake () {
        anim = GetComponent<Animator>();
      
        //lastRotate = transform.rotation;
            
        

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if(lastPosition==Vector3.zero) lastPosition = transform.position;

        float dist = Vector3.Distance(transform.position, lastPosition);
        float speed =speedFactor* dist / Time.deltaTime;

        //float rotation = Quaternion.Angle(transform.rotation, lastRotate);


		if (dist > threshold) {

            if (handler != null)
            {
                Timing.KillCoroutines(handler);
                handler = null;
            }

            anim.speed = speed;
			anim.SetBool (animName,true);
            stoped = false;

        
 
		} 
        else
        {
            if(handler==null) handler=Timing.RunCoroutine(stopAnim());
        }



        lastPosition = transform.position;
        //lastRotate = transform.rotation;



	}

    IEnumerator<float> stopAnim()
    {

        float timer = timeBeforeStopAnimation;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return 0;
        }

        anim.SetBool(animName, false);
        anim.speed = 1;
        stoped = true;

        handler = null;

    }
    
    

}
