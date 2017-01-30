using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;



public class MountCarier : MonoBehaviour {

    public enum animType { Bool, Trigger };
    public animType AnimType = animType.Bool;
    public string animationName = "Sit";

    //It is due to design fault. You fucking idiot.
    public string triggerTwo = "ForceIdle";

    public GameObject wayPoints;
    public bool Mount;
    public bool unMount;

    CharacterController cc;
    NavMeshAgent nma;
    Collider col;

    GameObject originalParent;
    // Use this for initialization
    void Start () {
       nma= GetComponent<NavMeshAgent>();
        cc= GetComponent<CharacterController>();
        col = GetComponent<Collider>();
        originalParent = transform.parent.gameObject ;
    }
	
	// Update is called once per frame
	void Update () {
        if (Mount)
        {
            Mount = false;
            mount();
        }

        if (unMount)
        {
            unMount = false;
            StartCoroutine(_unmount());
        }

    }

    public void mount()
    {
        Timing.RunCoroutine(_mount());
    }

    public void unmount()
    {
        Timing.RunCoroutine(_unmount());
    }

    public IEnumerator<float> _mount()
    {



        myTween mt = wayPoints.GetComponent<myTween>();

        if (nma)
            nma.enabled = false;
        if (cc)
            cc.enabled = false;
        if (col)
            col.enabled = false;



        mt.reverse = false;
        transform.parent = wayPoints.transform.parent;
        IEnumerator<float> handle= Timing.RunCoroutine(mt._tweenMEC(gameObject, 2f));
        yield return Timing.WaitUntilDone(handle);

        Animator anim = GetComponent<Animator>();
        if (anim)
            if (AnimType == animType.Bool)
            {
                anim.SetBool(animationName, false);
            }
            else
            {
                anim.SetTrigger(animationName);
            }
       
    }


    public IEnumerator<float> _unmount()
    {

        myTween mt = wayPoints.GetComponent<myTween>();

        Animator anim = GetComponent<Animator>();
        if (anim)
            if (AnimType == animType.Bool)
            {
                anim.SetBool(animationName, false);
            }
            else
            {
                if (triggerTwo != "")
                {
                    anim.SetTrigger(triggerTwo);
                }
                else
                {

                    anim.SetTrigger(animationName);
                }
            }

        //if (nma)
        //    nma.enabled = true;
        //if (cc)
        //    cc.enabled = true;
        //if (col)
        //    col.enabled = true;

        mt.reverse = true;
        IEnumerator<float> handler=Timing.RunCoroutine(mt._tweenMEC(gameObject, 2f));
        yield return Timing.WaitUntilDone(handler);
        if (nma)
            nma.enabled = true;
        if (cc)
            cc.enabled = true;
        if (col)
            col.enabled = true;
        transform.parent = originalParent.transform;

    }

}
