using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class CharacterControllerKeyboard : MonoBehaviour {

    CharacterController cc;

    public float speed = 3f;
    Vector3 move;
    public float rotateSpeed = 3f;


    void Awake()
    {
        cc = GetComponent<CharacterController>();


    }


    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor) {
            if (Input.GetKey(KeyCode.C)) Timing.RunCoroutine(setSpeedDuringPlay());
        }

        // move=(new Vector3(Input.GetAxis("Horizontal"),0,-Input.GetAxis("Vertical")));
        if (Input.GetAxis("Vertical") > 0){
            move = transform.forward * Input.GetAxis("Vertical");
        }
        else { move = Vector3.zero; }
        transform.RotateAround(transform.position, transform.up, rotateSpeed * Input.GetAxis("Horizontal"));
        if(cc.enabled==true)
         cc.Move(move * speed * Time.deltaTime);

    }

    //This enables user to set speed. It is for debugging. But also it can be a cheat in futuer;)
    IEnumerator<float> setSpeedDuringPlay()
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

        for (int i=0;i<9;i++)
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
        if (localSpeed > 0) speed = localSpeed;

        yield break;
    }
}
