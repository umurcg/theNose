using UnityEngine;
using System.Collections;

//This script plays animation


public class PlayAnimation : MonoBehaviour {

    public AnimType animParameter = AnimType.Boolean;
    public string animationName;
    public bool animate;
    public bool listen;


    Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();

        if (animationName == "")
        {
            Debug.Log("Specify name of animation parameter");
            this.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (listen)
        {
            if (animate)
            {
                activateAnimation(anim, animParameter, animationName);

            }
            else
            {
                deactivateAnimaton(anim, animParameter, animationName);
            }
        }

    }

    //Activates animation. Work with boolean or trigger.
    public static void activateAnimation(Animator anim, AnimType type, string name)
    {
        if (type != AnimType.Boolean && type != AnimType.Trigger) return;

        if (anim != null)
            switch (type)
            {
                case AnimType.Boolean:
                    anim.SetBool(name, true);

                    break;
                case AnimType.Trigger:
                    anim.SetTrigger(name);
                    break;
                default:
                    break;
            }

    }

    //Deactivates animaton. Only works with bool
    public static void deactivateAnimaton(Animator anim, AnimType type, string name)
    {
        if (type != AnimType.Boolean) return;

        anim.SetBool(name, false);

    }


}
