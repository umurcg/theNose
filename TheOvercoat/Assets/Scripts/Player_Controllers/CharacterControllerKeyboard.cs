using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerKeyboard : MonoBehaviour {

    CharacterController cc;

    public float speed = 3f;
    Vector3 move;
    public float rotateSpeed = 3f;
    //CameraRotator rotator;


    void Awake()
    {
        cc = GetComponent<CharacterController>();


    }

     void Start()
    {
        //rotator = CharGameController.getCamera().GetComponent<CameraRotator>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //TODO fix bug that appears while camera rotating and player is moving. For now I prevent player to move while camera rotates
        //if (rotator && rotator.rotating) return;

        // move=(new Vector3(Input.GetAxis("Horizontal"),0,-Input.GetAxis("Vertical")));
        if (Input.GetAxis("Vertical") > 0){
            move = transform.forward * Input.GetAxis("Vertical");
        }
        else { move = Vector3.zero; }
        transform.RotateAround(transform.position, transform.up, rotateSpeed * Input.GetAxis("Horizontal"));
        if (cc.enabled == true)
        {
            cc.Move(move * speed * Time.deltaTime);
            if (move != Vector3.zero)
            {
                Debug.Log("moving, amount: " + move * speed * Time.deltaTime);
            }

        }
    }


}
