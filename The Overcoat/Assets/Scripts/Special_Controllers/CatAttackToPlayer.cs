using UnityEngine;
using System.Collections;

public class CatAttackToPlayer : MonoBehaviour {
	public GameObject player;
	public bool jump=false;
	public bool lookToPlayer=false;

	public float speed=2;
	public float jumpTolerance = 0.5f;
	Vector3 initialPos;


	Quaternion initialRot;
	public float rotTolerance=0.1f;
	float ratio=0;
	Quaternion aimRot;

	// Use this for initialization
	void Start () {
		initialPos = transform.position;
		aimRot = Quaternion.LookRotation (player.transform.position,transform.up);
	}
	
	// Update is called once per frame
	void Update () {

		if (lookToPlayer) {
			StartCoroutine ("LookToPlayer");
		}

	}

	public IEnumerator Jump(){


		initialPos = transform.position;
		ratio = 0;

		while (Vector3.Distance (transform.position, player.transform.position)>jumpTolerance) {
			ratio += Time.deltaTime * speed;
			transform.position = Vector3.Lerp (initialPos, player.transform.position,ratio);
			yield return null;

		}


	}


}
