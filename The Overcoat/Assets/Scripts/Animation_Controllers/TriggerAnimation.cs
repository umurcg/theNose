using UnityEngine;
using System.Collections;

//This script sets animation parameter.

public class TriggerAnimation : MonoBehaviour , IEnterTrigger{

    public enum AnimParameter { Boolean, Trigger};
    public AnimParameter animationParameter;
    public float secondTriggerTime = 0;
    public string parameterName;
    float timer;

    bool oneTimeUse = false;

    Animator animGlobal;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                switch (animationParameter)
                {

                    case AnimParameter.Boolean:
                        animGlobal.SetBool(parameterName, false);
                        break;
                    case AnimParameter.Trigger:
                        animGlobal.SetTrigger(parameterName);
                        break;
                    default:
                        return;

                }
                timer = 0;
            }
        }

    }

   public void TriggerAction(Collider col) {
        Animator an = col.gameObject.GetComponent<Animator>();
        if (an != null) {
            trigger(an);
       }

    }



    void trigger(Animator anim)
    {
        print("trigger");
        animGlobal = anim;
        switch (animationParameter)
        {

            case AnimParameter.Boolean:
                anim.SetBool(parameterName, true);
                break;
            case AnimParameter.Trigger:
                anim.SetTrigger(parameterName);
                break;
            default:
                return;

        }
        timer = secondTriggerTime;

    }
    

}
