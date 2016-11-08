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
    public bool test = false;
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
        if (reverse)
        {
            reverseChildren();
            reverse = false;
        }
        if (test)
        {
            test = false;
            StartCoroutine(tween(testObject, s));

        }

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


        if (children.Count == 0)
            yield break;
        int index = 0;
        float ratio = 0;
        Vector3 initialPosition = subject.transform.position;
        Vector3 aimPosition = children[index].transform.position;

        while (true)
        {
            ratio += Time.deltaTime * speed;
            subject.transform.position = Vector3.Lerp(initialPosition, aimPosition, ratio);
            if (ratio >= 1)
            {
                ratio = 0;
                index++;
                if (!(index < children.Count))
                    yield break;
                subject.transform.position = aimPosition;
                initialPosition = subject.transform.position;
                aimPosition = children[index].transform.position;

            }
            yield return 0;
        }


    }




    public IEnumerator tween(GameObject subject, float speed)
    {

    
        if (children.Count == 0)
            yield break;
        int index = 0;
        float ratio = 0;
        Vector3 initialPosition = subject.transform.position;
        Vector3 aimPosition = children[index].transform.position;

        while (true)
        {
            ratio += Time.deltaTime * speed;
            subject.transform.position = Vector3.Lerp(initialPosition, aimPosition, ratio);
            if (ratio >= 1)
            {
                ratio = 0;
                index++;
                if (!(index < children.Count))
                    yield break;
                subject.transform.position = aimPosition;
                initialPosition = subject.transform.position;
                aimPosition = children[index].transform.position;

            }
            yield return null;
        }


    }




}
