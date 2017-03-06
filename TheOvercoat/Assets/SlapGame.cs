using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class SlapGame : MonoBehaviour {

    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        transform.localPosition = Vector3.zero;
        transform.LookAt(CharGameController.getCamera().transform);

        Timing.RunCoroutine(applyForce());


	}
	
	// Update is called once per frame
	void Update () {
	
	}

    

    public IEnumerator<float> applyForce()
    {
        yield return Timing.WaitForSeconds(3);
        rb.AddForceAtPosition(transform.right*10, transform.position,ForceMode.Impulse);
    }
}
