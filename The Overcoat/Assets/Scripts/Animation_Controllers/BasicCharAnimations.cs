using UnityEngine;
using System.Collections;

public class BasicCharAnimations : MonoBehaviour {
    public float threshold=0.1f;
    Vector3 lastPosition;
    Animator anim;
    AnimationClip walk;
    public float speedFactor = 1;

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
       // print(rotation);

        if (dist > threshold)
        {
            anim.speed = speed;
            anim.SetBool("Walk", dist > threshold);
            
            //anim.SetFloat("Blend", 0.5f + rotation);
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.speed = 1;
        }

     

        lastPosition = transform.position;
        lastRotate = transform.rotation;



	}
}
