using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class SliderScript : MonoBehaviour {

    public bool incr = true;
    public float minvalue, maxvalue;
    public float value;
    public float speed;
    public bool automaticSlide;


	// Use this for initialization
	void Start () {
        //slideTo(2.12312345f);
	}

    // Update is called once per frame
    void Update()
    {
        //if (minvalue > maxvalue)
        //{
        //    print("you set min bigger than max idiot.");
        //    return;

        //}

        if (automaticSlide)
        {

            if (incr)
            {
                value = Mathf.Clamp(value + Time.deltaTime * speed, minvalue, maxvalue);

                if (value == maxvalue)
                    incr = false;

            }
            else
            {
                value = Mathf.Clamp(value - Time.deltaTime * speed, minvalue, maxvalue);

                if (value == minvalue)
                    incr = true;
            }

        }
    }


    public void slideTo(float v)
    {
        Timing.RunCoroutine(_slideTo(v));
    }

     IEnumerator<float> _slideTo(float v)
    {
       
        if (value > v)
        {

            while (value > v)
            {
                print("working");
                value -= Time.deltaTime * speed;
                if (value <= v)
                {
                    //print("finish");
                    value = v;
                    yield break;

                }
                yield return 0;

            }
        }
        else if(value < v )
        {
            while (value < v)
            {
                print("working");
                value += Time.deltaTime * speed;
                if (value >= v)
                {

                    //print("finish2");
                    value = v;
                    yield break;

                }
                yield return 0;

            }
        }
            
     }

}
