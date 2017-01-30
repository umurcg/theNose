using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//This scripts tweens subject object through children objects.
//It got revese function which enables to rever children way points for force object to move reverse direction.
//It takes subject as aramater so it is more moduler than LerpThroughObjects scripts.

public class myTween : MonoBehaviour {

    List<GameObject> children=new List<GameObject>();


    //Test
    public GameObject testObject;
    public float s;
    //public bool test = false;
    public bool reverse = false;
    public bool mec = false;
    void Awake()
    {
       for(int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);

        }

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //if (reverse)
        //{
        //    reverseChildren();
        //    reverse = false;
        //}
        //if (test)
        //{
        //    test = false;
        //    StartCoroutine(tween(testObject, s));

        //}

        if (mec)
        {
            mec = false;
            Timing.RunCoroutine(_tweenMEC(testObject, s));
        }

	}

    public void reverseChildren()
    {
        children.Reverse();
    }


    public IEnumerator<float> _tweenMEC(GameObject subject, float speed)
    {


        GameObject[] childrenArray;

        if (reverse)
        {
            children.Reverse();
            childrenArray = children.ToArray();
            children.Reverse();
        }
        else
        {
            childrenArray = children.ToArray();
        }
        float t;

        if (childrenArray.Length == 0)
            yield break;
        int index = 0;
        float ratio = 0;
        Vector3 initialPosition = subject.transform.position;
        Vector3 aimPosition = childrenArray[index].transform.position;
        t = speed / Vector3.Distance(initialPosition, aimPosition);
        Timing.RunCoroutine(_rotateToPosition(subject, aimPosition, speed, true));
        while (true)
        {
            ratio += Time.deltaTime * t;
            subject.transform.position = Vector3.Lerp(initialPosition, aimPosition, ratio);
            if (ratio >= 1)
            {
                ratio = 0;
                index++;
                if (!(index < children.Count))
                {
                    Timing.RunCoroutine(_rotateToPosition(subject, childrenArray[index - 1].transform.position + childrenArray[index - 1].transform.forward, t, true));
                    yield break;
                }
                subject.transform.position = aimPosition;
                initialPosition = subject.transform.position;
                aimPosition = childrenArray[index].transform.position;
                t = speed / Vector3.Distance(initialPosition, aimPosition);
                Timing.RunCoroutine(_rotateToPosition(subject, aimPosition, t, true));
            }
            yield return 0;
        }



    }




    public IEnumerator<float> _rotateToPosition(GameObject subject, Vector3 aim, float time, bool horizontal)
    {

        if (horizontal)
            aim.y = subject.transform.position.y;


        Quaternion initialRot = subject.transform.rotation;
        Quaternion aimRot = Quaternion.LookRotation(aim - subject.transform.position);
        float ratio = 0;


        while (ratio < 1)
        {

            ratio += Time.deltaTime * time;
            subject.transform.rotation = Quaternion.Lerp(initialRot, aimRot, ratio);

            yield return 0;
        }
        subject.transform.rotation = aimRot;

    }


    //public IEnumerator tween(GameObject subject, float speed)
    //{
    //    float s;
    
    //    if (children.Count == 0)
    //        yield break;
    //    int index = 0;
    //    float ratio = 0;
    //    Vector3 initialPosition = subject.transform.position;
    //    Vector3 aimPosition = children[index].transform.position;
    //    s = Vector3.Distance(initialPosition, aimPosition) * speed;
    //    while (true)
    //    {
    //        ratio += Time.deltaTime * s;
    //        subject.transform.position = Vector3.Lerp(initialPosition, aimPosition, ratio);
    //        if (ratio >= 1)
    //        {
    //            ratio = 0;
    //            index++;
    //            if (!(index < children.Count))
    //                yield break;
    //            subject.transform.position = aimPosition;
    //            initialPosition = subject.transform.position;
    //            aimPosition = children[index].transform.position;
    //            s = Vector3.Distance(initialPosition, aimPosition) * speed;

    //        }
    //        yield return null;
    //    }


    //}




}
