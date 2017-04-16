using UnityEngine;
using System.Collections;

//Spawn bugs inside of box collider.

public class Bugs : MonoBehaviour {

    public float numberOfSpawn;
    public GameObject colliderObject;
    public GameObject objectToSpawn;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < numberOfSpawn; i++)
        {
            GameObject spawnedObject = (GameObject)Instantiate(objectToSpawn) as GameObject;
            spawnedObject.transform.parent = transform;
            spawnedObject.transform.position=  Vckrs.generateRandomPositionInBox(colliderObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
