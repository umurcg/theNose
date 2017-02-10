using UnityEngine;
using System.Collections;


//Loops animation trigger with delay betweens
public class LoopAnimTrigger : MonoBehaviour {

    public float delay;
    public string triggerName;
    float timer;
    Animator ac;
	// Use this for initialization
	void Start () {

        ac = GetComponent<Animator>();
        
	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(timer);

        if (timer <= 0)
        {
            timer = delay;
            ac.SetTrigger(triggerName);
        }else{
            timer -= Time.deltaTime;
        }

	}
}
