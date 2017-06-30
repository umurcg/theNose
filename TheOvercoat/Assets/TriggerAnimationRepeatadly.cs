using UnityEngine;
using System.Collections;

public class TriggerAnimationRepeatadly : MonoBehaviour {

    public float timeBetweenRepeats;
    public string triggerName;

    Animator anim;
    float timer = 0;

	// Use this for initialization
	void Start () {
	    anim=GetComponent<Animator>();
        anim.SetTrigger(triggerName);
        timer = timeBetweenRepeats;
	}
	
	// Update is called once per frame
	void Update () {

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = timeBetweenRepeats;
                anim.SetTrigger(triggerName);
            }
        }

	}
}
