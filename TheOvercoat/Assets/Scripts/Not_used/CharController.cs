using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {
    // Use this for initialization
    CharacterController cc;

    public float speed = 3f;
    Vector3 move;

   
    void Awake () {
        cc = GetComponent<CharacterController>();
       

    }

    
	// Update is called once per frame
	void Update () {

        // move=(new Vector3(Input.GetAxis("Horizontal"),0,-Input.GetAxis("Vertical")));
        move = transform.right* Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        cc.Move(move*speed*Time.deltaTime);

	}
}
