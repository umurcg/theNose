using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script is similar to spawnBotsOnNavMesh script. Bu it is less complicated and doesn't consider edges
public class SpawnBotsInsideSphere : MonoBehaviour {

    public GameObject objectToSpawn;
    public float navMeshSpeed;
    public int numberOfSpawn;
    List<GameObject> spawnedObjects;

    // Use this for initialization
    void Start()
    {

        spawnedObjects = new List<GameObject>();
        for (int i = 0; i < numberOfSpawn; i++)
        {
            GameObject spawnedObject = spawneBot();

        }
    }



    GameObject spawneBot()
    {

        GameObject spawnedObject = (GameObject)(Instantiate(objectToSpawn.gameObject, getRandomPosOnMesh(objectToSpawn.gameObject), Quaternion.LookRotation(transform.forward)));
        spawnedObjects.Add(spawnedObject);

        spawnedObject.transform.localScale = Vector3.one;
        spawnedObject.transform.parent = transform;
        
        NavMeshAgent nma = spawnedObject.GetComponent<NavMeshAgent>();
        if (nma != null)
        {
            nma.speed = navMeshSpeed;
        }
        return spawnedObject;
    }

    Vector3 getRandomPosOnMesh(GameObject obj)
    {
        SphereCollider sc = GetComponent<SphereCollider>();
        Vector3 pos = sc.center + transform.position + sc.radius * Random.insideUnitSphere;
        NavMeshHit nmh;
        NavMeshAgent nma = obj.GetComponent<NavMeshAgent>();
        if (NavMesh.SamplePosition(pos, out nmh, sc.radius, nma.areaMask))
        {
            return nmh.position;
        }
        else
        {
            return Vector3.zero;
        }

    }


    // Update is called once per frame
    void Update () {
	
	}
}
