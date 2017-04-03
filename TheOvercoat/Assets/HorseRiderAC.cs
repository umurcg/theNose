using UnityEngine;
using System.Collections;

//Animator controllller scripot of horse rider character
public class HorseRiderAC : MonoBehaviour {


    public float whipeTime = 7f;
    float tolerance = 0.00001f;
    Vector3 prevPos;
    Animator ac;
    float timer;

    private void Awake()
    {
        ac = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {

        prevPos = transform.position;


	}
	
	// Update is called once per frame
	void Update () {


        float delta = Vector3.Distance(transform.position, prevPos);
        if (delta > tolerance)
        {
            timer -= Time.deltaTime;

            if (timer <= 0) whip();
        }
        else
        {
            timer = whipeTime;
        }

        prevPos = transform.position;
	}

    void whip()
    {
        timer = whipeTime;
        ac.SetTrigger("whip");
        
    }

}
