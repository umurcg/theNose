using UnityEngine;
using System.Collections;

public class LerpToPosition : MonoBehaviour {

	bool lerp=false;
	public Transform aim;
	public float speed = 1;
	Vector3 initialPosition;
	float ratio=0;
	//TODO
	public bool rotate;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 
		if (lerp) {

			ratio += Time.deltaTime*speed;
			transform.position = Vector3.Lerp (initialPosition, aim.position, ratio);
			print (ratio);
			if (ratio>=1) {
				ratio = 0;
				lerp = false;

			}
		}
	}

	public void Lerp(){
		initialPosition = transform.position;
		lerp = true;
		ratio = 0;

	}
}
