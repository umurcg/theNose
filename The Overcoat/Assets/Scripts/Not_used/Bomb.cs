using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour, IClickAction {

	// Use this for initialization

	// Update is called once per frame

    public void Action()
    {
        print("bomb");
        
        Rigidbody rb=GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().enabled = false;
        rb.AddExplosionForce(100000, transform.position-transform.up*5, 20);

    }

}
