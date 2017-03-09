using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class SetTimeScale : MonoBehaviour {
    public float scale;
    float prevScale;
	// Use this for initialization
	void Start () {
        scale = Mathf.Clamp(scale, 0, 10);
        Time.timeScale = scale;
        prevScale = scale;
    }
	
	// Update is called once per frame
	void Update () {
        if (prevScale != scale)
        {
            setTimeScale(scale);

        }

        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.T)) Timing.RunCoroutine(setScaleDuringPlay());
        }


    }

    public void setTimeScale(float scale)
    {
        scale = Mathf.Clamp(scale, 0, 10);
        Time.timeScale = scale;
        prevScale = scale;
        Debug.Log("Time scale is set to " + scale);

    }

    //This enables user to set time scale. It is for debugging. But also it can be a cheat in futuer;)
    IEnumerator<float> setScaleDuringPlay()
    {
        //Numbers
        KeyCode[] keyCodes = {
           KeyCode.Alpha0,
            KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
        };

        //Wait for input
        while (Input.anyKeyDown == false)
        {
            Debug.Log("Waiting for a number");
            yield return 0;
        }

        float localSpeed = 0;

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKey(keyCodes[i]))
            {
                Debug.Log("You pressed a number which is" + keyCodes[i].ToString());
                localSpeed += 10 * (i + 1);

            }
        }

        //Wait for one frame 
        yield return 0;

        //Wait for input
        while (Input.anyKeyDown == false)
        {
            yield return 0;
            //Debug.Log("Waiting for a number");
        }


        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKey(keyCodes[i]))
            {
                localSpeed += (i + 1);
                Debug.Log("You pressed a number which is" + keyCodes[i].ToString());
            }

        }

        Debug.Log("Your speed is " + localSpeed);
        if (localSpeed > 0) setTimeScale( localSpeed);

        yield break;
    }



}
