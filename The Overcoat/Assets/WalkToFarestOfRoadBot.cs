using UnityEngine;
using System.Collections;

public class WalkToFarestOfRoadBot : MonoBehaviour {
	public GameObject obj;
	NavMeshAgent nma;
	Vector3 lastCheckedPos;
	float timer=0;
	public float waitBetweenWalks;
	public float tolerance;

	// Use this for initialization
    
	bool isMax=false;
	public int axis = 1;

	void Start () {
		nma = GetComponent<NavMeshAgent> ();

	}

	// Update is called once per frame
	void Update () {

		if (timer <= 0) {
			timer = 0;

			WalkToOtherEdge ();
		} else {
			timer -= Time.deltaTime;
		}
		//timer == 0 ? WalkIfNotWalking() : timer -= Time.deltaTime;


	}

	public void interruptAndWalkAgain(){
		nma.Stop();
		WalkToOtherEdge ();
	}

	void WalkToOtherEdge(){
		if (checkIsMoving () == false) {
			isMax = !isMax;
			if (nma.isOnNavMesh) {
				nma.destination = FarestPoint ();
			}
			timer = waitBetweenWalks;
		}
	}

//for waiting between walks
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

	Vector3 FarestPoint(){
		
		Mesh planeMesh = obj.GetComponent<MeshFilter>().mesh;
		Bounds bounds = planeMesh.bounds;

		Vector3 newVec;
		float min, max;

		switch (axis) {
		case 1:

			min = obj.transform.position.x - obj.transform.localScale.x * bounds.size.x * 0.5f;
			max = obj.transform.position.x+ obj.transform.localScale.x  * bounds.size.x * 0.5f;
			if (isMax) {
				newVec = new Vector3 (max,
					transform.position.y,
					transform.position.z);

			} else {
				newVec = new Vector3 (min,
					transform.position.y,
					transform.position.z);
			}


			return newVec;
		case 2:

			min = obj.transform.position.z - obj.transform.localScale.z * bounds.size.z * 0.5f;
			max = obj.transform.position.z+ obj.transform.localScale.z  * bounds.size.z * 0.5f;
			if (isMax) {
				newVec = new Vector3 (transform.position.x,
					transform.position.y,
					max);

			} else {
				newVec = new Vector3 (transform.position.x,
					transform.position.y,
					min);
			}


			return newVec;
		default:

			min = obj.transform.position.x - obj.transform.localScale.x * bounds.size.x * 0.5f;
			max = obj.transform.position.x+ obj.transform.localScale.x  * bounds.size.x * 0.5f;
			if (isMax) {
				newVec = new Vector3 (max,
					transform.position.y,
					transform.position.z);

			} else {
				newVec = new Vector3 (min,
					transform.position.y,
					transform.position.z);
			}


			return newVec;
		}


	

	}

}
