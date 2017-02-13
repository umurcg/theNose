using UnityEngine;
using System.Collections;


//This script triggers objec's animation when scene reaches the time.
public class TriggerAnimationAtTime : MonoBehaviour {
    public float time;

    public enum AnimParameter { Boolean, Trigger };
    public AnimParameter animationParameter;
    public string parameterName;



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(time>0)
        time -= Time.deltaTime;

        if (time <= 0)
        {
            Animator an = GetComponent<Animator>();
            if (an != null)
            {
                switch (animationParameter)
                {

                    case AnimParameter.Boolean:
                        an.SetBool(parameterName, true);
                        break;
                    case AnimParameter.Trigger:
                        an.SetTrigger(parameterName);
                        break;
                    default:
                        return;

                }
            }

            time = 0;
        }

	}
}
