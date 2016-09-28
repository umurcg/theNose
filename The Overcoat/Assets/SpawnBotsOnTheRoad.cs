using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnBotsOnTheRoad : MonoBehaviour {
	public float offset;
	public int spawnNumber;
	public Transform[] objects;
	public List<GameObject> spawnedObjects;
	public float minSpeed=1;
	public float maxSpeed=4;
	// Use this for initialization
	void Awake () {
		if (objects.Length > 0) {
			for (int i = 0; i < spawnNumber; i++) {
				spawneBot ();
			}
		}

	}

	void spawneBot(){
		GameObject spawnedObject=(GameObject)(Instantiate (objects [Random.Range (0, objects.Length - 1)].gameObject, GetARandomTreePos (),Quaternion.LookRotation(transform.forward)));
		spawnedObjects.Add (spawnedObject);
		WalkToFarestOfRoadBot wtfrb = spawnedObject.GetComponent<WalkToFarestOfRoadBot> ();
		wtfrb.obj = transform.gameObject;
		spawnedObject.GetComponent<NavMeshAgent>().speed= Random.Range (minSpeed,maxSpeed);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	Vector3 GetARandomTreePos(){

		Mesh planeMesh = GetComponent<MeshFilter>().mesh;
		Bounds bounds = planeMesh.bounds;

		float minX = transform.position.x - transform.localScale.x * bounds.size.x * 0.5f;
		float maxX = transform.position.x+ transform.localScale.x  * bounds.size.x * 0.5f;
		float minZ = transform.position.z- transform.localScale.z * bounds.size.z * 0.5f;
		float maxZ = transform.position.z+ transform.localScale.z * bounds.size.z * 0.5f;
		Vector3 newVec = new Vector3(Random.Range (maxX, minX),
			transform.position.y+offset,
			Random.Range (maxZ, minZ));
		return newVec;
	}
}
