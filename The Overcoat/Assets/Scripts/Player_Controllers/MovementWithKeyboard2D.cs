using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public class MovementWithKeyboard2D : MonoBehaviour {


    public float speed = 3f;
    public float rotateSpeed = 1f;
    public float jumpHeight = 2f;
    public float jumpSpeed = 1f;

    //For controlling from script
    public float scriptInput;

    Vector3 forward;
    Vector3 right;
    float ratio;

    float prevHorInputState;

    bool jumping=false;

    float y;

    // Use this for initialization
    void Start () {
       forward = Camera.main.gameObject.transform.forward;
        right = Camera.main.gameObject.transform.right;
        prevHorInputState = Input.GetAxis("Horizontal");
       y = Camera.main.ScreenToWorldPoint(new Vector3(0, -5, 0)).y;

    }
	
	// Update is called once per frame
	void Update () {
        float horInput;
        if (scriptInput == 0)
        {
            horInput = Input.GetAxis("Horizontal");
        }else
        {
             horInput = scriptInput;
        }

        if (horInput != prevHorInputState)
        {
            prevHorInputState = horInput;
            ratio = 0;
        }

        transform.position = transform.position + right * speed * horInput;

        ratio += Time.deltaTime*rotateSpeed;

        if (horInput == 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forward,transform.up), ratio);
        } else if (horInput < 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-right, transform.up), ratio);
        }
        else if (horInput > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(right, transform.up), ratio);
        }

        if (Input.GetAxis("Jump") > 0&&!jumping)
        {
            Timing.RunCoroutine(_jump());
        }

       
    }

    IEnumerator<float> _jump()
    {
   

        jumping = true;
        float ratio = 0;
        

        while (ratio<0.5f)
        {
            ratio += Time.deltaTime;
            transform.position = transform.position + transform.up * jumpSpeed;
            yield return 0;
        }

        while (ratio < 1f)
        {
            ratio += Time.deltaTime;

            transform.position = transform.position - transform.up * jumpSpeed;
            yield return 0;
        }

        transform.position = new Vector3(transform.position.x, y, transform.position.z);

        jumping = false;
    }




}
