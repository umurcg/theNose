using UnityEngine;
using System.Collections;

public class ObjectChildSpawn : MonoBehaviour {
	public Object obj;
	GameObject spawnedObj;
	public Vector3 localPos;
	//public bool test=false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		if (test) {
//			Spawn ();
//			test = false;
//		}
	}

	public void Spawn(){
		if (spawnedObj == null) {
			spawnedObj = (GameObject)(Instantiate (obj, transform));
			spawnedObj.transform.localScale = new Vector3 (1, 1, 1);
		//	spawnedObj.transform.localPosition = new Vector3 (0, 0, 1);
	        spawnedObj.transform.localPosition = localPos;
			spawnedObj.transform.rotation = transform.rotation;
		}
	}

	public void Destroy(){
		if (spawnedObj != null)
			DestroyObject (spawnedObj);
	}
}
