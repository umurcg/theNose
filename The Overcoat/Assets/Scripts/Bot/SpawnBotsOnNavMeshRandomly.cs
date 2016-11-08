using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnBotsOnNavMeshRandomly : MonoBehaviour {

    
    public int spawnNumber;
    public Transform[] objects;
    List<GameObject> spawnedObjects;
    public float minSpeed = 1;
    public float maxSpeed = 4;

    // Use this for initialization
    void Awake()
    {
        spawnedObjects = new List<GameObject>();
        if (objects.Length > 0)
        {
            for (int i = 0; i < spawnNumber; i++)
            {
                spawneBot();
            }
        }

    }

    public List<GameObject>  getSpawnedObjects()
    {
        return spawnedObjects;
    }

    void spawneBot()
    {
        Transform botType = objects[Random.Range(0, objects.Length - 1)];
        GameObject spawnedObject = (GameObject)(Instantiate(botType.gameObject, getRandomPosOnMesh(botType.gameObject), Quaternion.LookRotation(transform.forward), gameObject.transform));
        spawnedObjects.Add(spawnedObject);
        WalkToFarestOfRoadBot wtfrb = spawnedObject.GetComponent<WalkToFarestOfRoadBot>();
        if (wtfrb)
            wtfrb.obj = transform.gameObject;
        NavMeshAgent nma = spawnedObject.GetComponent<NavMeshAgent>();
        if (nma != null)
        {

            nma.speed = Random.Range(minSpeed, maxSpeed);
        }
    }


    Vector3 getRandomPosOnMesh(GameObject obj)
    {
        SphereCollider sc = GetComponent<SphereCollider>();
        Vector3 pos = transform.position+ sc.radius * Random.insideUnitSphere;
        NavMeshHit nmh;
        NavMeshAgent nma = obj.GetComponent<NavMeshAgent>();
        if (NavMesh.SamplePosition(pos, out nmh, sc.radius,nma.areaMask)) {
            return nmh.position;
        }
        else
        {
            return Vector3.zero;
        }

    }


}
