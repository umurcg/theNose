using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//_BasicCharAnimation.cs
//_Dependent to: 

//This script controls animations of player character.
//It adjust walking animation according to speed of object.
//It triggers idle animation when object doesnt move.


public class BasicCharAnimations : MonoBehaviour{
    public float threshold=0.01f;

    //In some cases object canbe moved by scritps with too much distance. In that case walk animation shouldn't be triggered while it also effects
    //speed of animation.
    public float maxDelta = 5f;

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

        addOptimization();

        //lastRotate = transform.rotation;

 

	}


    private void Update()
    {

        
        if (lastPosition == Vector3.zero) lastPosition = transform.position;

        float dist = Vector3.Distance(transform.position, lastPosition);
        float speed = speedFactor * dist / Time.deltaTime;

        //float rotation = Quaternion.Angle(transform.rotation, lastRotate);


        if (dist > threshold)
        {
            anim.speed = speed;
            //print(dist);
            anim.SetBool(animName, dist > threshold);

        }
        else
        {
            anim.SetBool(animName, false);
            anim.speed = 1;
        }


        lastPosition = transform.position;
        
    }


    //// Update is called once per frame
    //void FixedUpdate () {

    //       if(lastPosition==Vector3.zero) lastPosition = transform.position;

    //       float dist = Vector3.Distance(transform.position, lastPosition);
    //       float speed =speedFactor* dist / Time.deltaTime;

    //       //float rotation = Quaternion.Angle(transform.rotation, lastRotate);


    //	if (dist > threshold && dist<maxDelta) {

    //           if (handler != null)
    //           {
    //               Timing.KillCoroutines(handler);
    //               handler = null;
    //           }

    //           anim.SetBool(animName, true);
    //           anim.speed = speed;
    //           stoped = false;



    //	} 
    //       else
    //       {
    //           if(handler==null) handler=Timing.RunCoroutine(stopAnim());
    //       }



    //       lastPosition = transform.position;
    //       //lastRotate = transform.rotation;



    //}

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

    //This funciton adds optimization script to renderer object.
    //It makes disable and enable animation and update functiion of this sciprt when renderer invisible and visible respectevly.
    void addOptimization()
    {
        if (GetComponentInChildren<DisableEnableBACInParent>()) return; 

        Renderer rend=GetComponent<Renderer>();
        if (!rend) rend= GetComponentInChildren<Renderer>();

        rend.gameObject.AddComponent<DisableEnableBACInParent>();
    }

}
