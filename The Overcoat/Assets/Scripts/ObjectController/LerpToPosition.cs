using UnityEngine;
using System.Collections;

public class LerpToPosition : MonoBehaviour {


	public Transform aim;
	public float speed = 1;
	public float middleHeight=0;
	public float tolerance=0.5f;
	Vector3 initialPosition;

    public float forward;
    public float right;

	float ratio=0;
	//TODO
	public bool rotate;
	public float rotateSpeed=30f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 
//		if (lerp) {
//
//			ratio += Time.deltaTime*speed;
//			transform.position = Vector3.Lerp (initialPosition, aim.position, ratio);
//           
//			if (middleHeight > 0) {
//				if(ratio<0.5f){
//					transform.position = new Vector3(transform.position.x, Mathf.Lerp (initialPosition.y, middleHeight, ratio * 2),transform.position.z);
//				}else{
//					transform.position = new Vector3(transform.position.x, Mathf.Lerp (initialPosition.y, middleHeight, 2-2*ratio),transform.position.z);
//				}
//			}
//
//			if (ratio>=1) {
//				ratio = 0;
//				lerp = false;
//				transform.position = aim.position;
//
//			}
//		}
	}

	public IEnumerator Lerp(){
		
		initialPosition = transform.position;
	//	print (initialPosition.y);
		ratio = 0;

        Vector3 aimPos = aim.transform.position + aim.transform.forward*forward + aim.transform.right*right;

		while (Vector3.Distance (transform.position, aimPos)>tolerance) {
			ratio += Time.deltaTime * speed;

			transform.position = Vector3.Lerp (initialPosition, aimPos, ratio);
			if (middleHeight != 0) {
				if (ratio < 0.5f) {
					transform.position = new Vector3 (transform.position.x, initialPosition.y+Mathf.Sin (Mathf.Lerp (0, Mathf.PI/2, ratio * 2)) * middleHeight, transform.position.z);

									} else {
					transform.position = new Vector3 (transform.position.x,initialPosition.y+ Mathf.Sin (Mathf.Lerp (0, Mathf.PI/2, 2-2*ratio)) * middleHeight, transform.position.z);
				}
			}

			if (rotate)
				transform.Rotate (new Vector3 ( Time.deltaTime*rotateSpeed, Time.deltaTime*rotateSpeed,  Time.deltaTime*rotateSpeed));
	//		print (Vector3.Distance (transform.position, aim.transform.position));
			yield return null;

		}

	}
}
