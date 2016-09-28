using UnityEngine;
using System.Collections;

public class RandomWalkBot : MonoBehaviour {
	public GameObject obj;
	NavMeshAgent nma;
	Vector3 lastCheckedPos;
	float timer=0;
	public float waitBetweenWalks;
	public float tolerance;
	// Use this for initialization
	void Start () {
		nma = GetComponent<NavMeshAgent> ();
			
	}
	
	// Update is called once per frame
	void Update () {

		if (timer <= 0) {
			timer = 0;
			WalkIfNotWalking ();
		} else {
			timer -= Time.deltaTime;
		}
		//timer == 0 ? WalkIfNotWalking() : timer -= Time.deltaTime;
		

	}

	public void interruptAndWalkAgain(){
		nma.Stop();
		WalkIfNotWalking ();
	}

	void WalkIfNotWalking(){
		if (checkIsMoving () == false) {
			nma.destination = GetARandomTreePos ();
			timer = waitBetweenWalks;
		}
	}


	bool checkIsMoving(){
		if (lastCheckedPos == null) {
			lastCheckedPos = transform.position;
			return false;
		} else {
			if (Vector3.Distance( transform.position, lastCheckedPos)>tolerance) {
				lastCheckedPos = transform.position;
				return true;
			} else {
				return false;
			}
		}

	}

    Vector3 GetARandomTreePos(){

		Mesh planeMesh = obj.GetComponent<MeshFilter>().mesh;
		Bounds bounds = planeMesh.bounds;

		float minX = obj.transform.position.x - obj.transform.localScale.x * bounds.size.x * 0.5f;
		float maxX = obj.transform.position.x+ obj.transform.localScale.x  * bounds.size.x * 0.5f;
		float minZ = obj.transform.position.z- obj.transform.localScale.z * bounds.size.z * 0.5f;
		float maxZ = obj.transform.position.z+ obj.transform.localScale.z * bounds.size.z * 0.5f;
		Vector3 newVec = new Vector3(Random.Range (maxX, minX),
			transform.position.y,
			Random.Range (maxZ, minZ));
		return newVec;
	}



}
