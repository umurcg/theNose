using UnityEngine;
using System.Collections;

public class RandomObjectGenerator : MonoBehaviour {

    public GameObject[] objectsToGenerate;
    public int numberOfSpawn;

        
    // Use this for initialization
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void generate()
    {

        for(int i = 0; i < numberOfSpawn; i++)
        {
            GameObject spawnedObj=Instantiate(objectsToGenerate[i % objectsToGenerate.Length]) as GameObject;
            spawnedObj.transform.position=Vckrs.generateRandomPositionInBox(gameObject);
            spawnedObj.transform.RotateAround(transform.position, Vector3.up, Random.Range(0, 360));
            spawnedObj.transform.localScale = Vector3.one;
            spawnedObj.transform.parent = transform;
            

        }

    }

}
