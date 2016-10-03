using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class SpawnObjectInSphere : MonoBehaviour {
	public GameObject prefab;
	public float speed=3;
	public int numberOfSpawn=10;
	List <GameObject> prefabs;
	SphereCollider sc;
	public Vector3 scale;

	// Use this for initialization
	void Start () {
		prefabs = new List<GameObject> ();
		sc= GetComponent<SphereCollider> ();
		Spawn ();

	}

	void Spawn(){
		for (int i = 0; i < numberOfSpawn; i++) {
			GameObject ob=(GameObject)Instantiate (prefab, Random.insideUnitSphere*sc.radius*2+transform.position, Random.rotation, transform);
			ob.transform.localScale=scale;
			prefabs.Add (ob);
			randomMovement rm=ob.GetComponent<randomMovement> ();
			if(rm!=null){
			rm.speed = speed;
            }
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
