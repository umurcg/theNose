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


    public string animName="Walk";
    Vector3 lastPosition;
    Animator anim;
    AnimationClip walk;
    public float speedFactor = 0.5f;


    public int delayForStop = 50;
    int timer=0;
    

	// Use this for initialization
	void Awake () {
        anim = GetComponent<Animator>();

        

#if UNITY_EDITOR
        //Debug.Log("It is in editor");
      
        return;
#else
    
          addOptimization();
#endif

        //lastRotate = transform.rotation;



    }


    private void FixedUpdate()
    {

        
        if (lastPosition == Vector3.zero) lastPosition = transform.position;

        float dist = Vector3.Distance(transform.position, lastPosition);
        float speed = speedFactor * dist / Time.deltaTime;

        //float rotation = Quaternion.Angle(transform.rotation, lastRotate);

        //if (gameObject == CharGameController.getActiveCharacter())
        //{
        //    Debug.Log("Gameobject: " + gameObject.name + " Dist is " + dist + " threshold is " + threshold);
        //    if (dist <= threshold && timer>delayForStop) { Debug.Log("STOPPINGGG"); }
        //}

        if (dist > threshold)
        {
            anim.speed = speed;
            //print(dist);
            anim.SetBool(animName, true);


        }        
        else if(anim.GetBool(animName)==true)
        {
            if (timer < delayForStop)
            {
                timer++;
            }
            else
            {
                timer = 0;
                anim.speed = 1;
                anim.SetBool(animName, false);

            }
        }


        lastPosition = transform.position;
        
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
