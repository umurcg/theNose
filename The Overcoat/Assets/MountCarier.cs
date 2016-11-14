using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class MountCarier : MonoBehaviour {

    public GameObject wayPoints;
    public bool Mount;
    public bool unMount;

    CharacterController cc;
    NavMeshAgent nma;
    Collider col;

    // Use this for initialization
    void Start () {
       nma= GetComponent<NavMeshAgent>();
        cc= GetComponent<CharacterController>();
        col = GetComponent<Collider>();
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
            unmount();
        }
    }

    public void mount()
    {

        myTween mt = wayPoints.GetComponent<myTween>();

        if (nma)
            nma.enabled = false;
        if (cc)
            cc.enabled = false;
        if (col)
            col.enabled = false;

        mt.reverse = false;
        Timing.RunCoroutine(mt._tweenMEC(gameObject, 2f));

    }


    public void unmount()
    {

        myTween mt = wayPoints.GetComponent<myTween>();

        //if (nma)
        //    nma.enabled = true;
        //if (cc)
        //    cc.enabled = true;
        //if (col)
        //    col.enabled = true;

        mt.reverse = true;
        Timing.RunCoroutine(mt._tweenMEC(gameObject, 2f));
    }

}
