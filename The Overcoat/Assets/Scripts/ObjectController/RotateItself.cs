using UnityEngine;
using System.Collections;

public class RotateItself : MonoBehaviour {
    public float speed;
    Vector3 axis;

	// Use this for initialization
	void Awake () {
       
	}
	
	// Update is called once per frame
	void Update () {

 
        transform.Rotate(Vector3.forward* speed * Time.deltaTime);
    }  
}
