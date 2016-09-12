using UnityEngine;
using System.Collections;

public class MoveToHere : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void Move(Vector3 offset,GameObject obj){
		obj.transform.position = this.transform.position + offset; 
		obj.transform.rotation = this.transform.rotation;
	}
}
