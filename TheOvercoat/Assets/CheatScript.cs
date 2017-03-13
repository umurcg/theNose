using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//This script deals with cheats
//For now cheats only avaible on editor mode but in future they maybe in build too ;)

public class CheatScript : MonoBehaviour {

    CharacterControllerKeyboard cck;

    //Numbers
    KeyCode[] numbers = {
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

    // Use this for initialization
    void Start () {
        cck = CharGameController.getActiveCharacter().GetComponent<CharacterControllerKeyboard>();

	}
	
	// Update is called once per frame
	void Update () {

        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.T)) Timing.RunCoroutine(setScaleDuringPlay());
            if (Input.GetKey(KeyCode.C)) Timing.RunCoroutine(setSpeedDuringPlay());

        }
    }


    //This enables user to set time scale. It is for debugging. But also it can be a cheat in futuer;)
    IEnumerator<float> setScaleDuringPlay()
    {
        yield return 0;

        //Wait for input
        while (Input.anyKeyDown == false)
        {
            //Debug.Log("Waiting for a number");
            yield return 0;
        }

        float timeScale = 0;

        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKey(numbers[i]))
            {
                //Debug.Log("You pressed a number which is" + numbers[i].ToString());
                timeScale += 10 * (i );

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


        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKey(numbers[i]))
            {
                timeScale += (i);
                //Debug.Log("You pressed a number which is" + numbers[i].ToString());
            }

        }

        //Debug.Log("Your time scale is " + timeScale);
        if (timeScale > 0) Time.timeScale = timeScale;

        yield break;
    }
    

    //This enables user to set speed. It is for debugging. But also it can be a cheat in futuer;)
    IEnumerator<float> setSpeedDuringPlay()
    {
        yield return 0;

        //Wait for input
        while (Input.anyKeyDown == false)
        {
            //Debug.Log("Waiting for a number");
            yield return 0;
        }

        float localSpeed = 0;



        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKey(numbers[i]))
            {
                //Debug.Log("You pressed a number which is" + numbers[i].ToString());
                localSpeed += 10 * (i );


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


        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKey(numbers[i]))
            {
                localSpeed += (i );
                //Debug.Log("You pressed a number which is" + numbers[i].ToString());
            }

        }

        //Debug.Log("Your speed is " + localSpeed);
        if (localSpeed > 0) cck.speed = localSpeed;

        yield break;
    }

}
