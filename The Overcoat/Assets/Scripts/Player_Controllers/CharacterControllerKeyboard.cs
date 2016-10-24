using UnityEngine;
using System.Collections;

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

        // move=(new Vector3(Input.GetAxis("Horizontal"),0,-Input.GetAxis("Vertical")));
        if (Input.GetAxis("Vertical") > 0){
            move = transform.forward * Input.GetAxis("Vertical");
        }
        else { move = Vector3.zero; }
        transform.RotateAround(transform.position, transform.up, rotateSpeed * Input.GetAxis("Horizontal")); 
        cc.Move(move * speed * Time.deltaTime);

    }
}
