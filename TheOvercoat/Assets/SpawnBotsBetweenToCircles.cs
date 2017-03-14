using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnBotsBetweenToCircles : MonoBehaviour {

    public GameObject objectToSpawn;
    public int numberOfSpawn;
    List<GameObject> spawnedObjects;
    public float radius1;
    public float radius2;
    public bool tryRadius;

    // Use this for initialization
    void Start()
    {

        spawnedObjects = new List<GameObject>();
        for (int i = 0; i < numberOfSpawn; i++)
        {
            GameObject spawnedObject = spawneBot();

        }
        if(tryRadius)   tryRadiuses();
    }



    GameObject spawneBot()
    {

        GameObject spawnedObject = (GameObject)(Instantiate(objectToSpawn.gameObject, Vckrs.generateRandomPositionBetweenCircles(transform.position, radius1, radius2), Quaternion.LookRotation(transform.forward)));
        spawnedObjects.Add(spawnedObject);

        spawnedObject.transform.localScale = Vector3.one;
        spawnedObject.transform.parent = transform;
        spawnedObject.SetActive(true);
        return spawnedObject;
    }

    public void tryRadiuses()
    {
        for (int i = 0; i < 2000; i++)
        {
            Vckrs.testPosition(Vckrs.generateRandomPositionBetweenCircles(transform.position, radius1, radius2));

        }
    }

    // Update is called once per frame
    void Update()
    {


    }
}
