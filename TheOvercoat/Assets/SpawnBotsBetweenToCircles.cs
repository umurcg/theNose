﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.AI;

public class SpawnBotsBetweenToCircles : MonoBehaviour {

    public GameObject objectToSpawn;
    NavMeshAgent objectNma;
    public int numberOfSpawn;
    List<GameObject> spawnedObjects;
    public float radius1;
    public float radius2;
    public bool tryRadius;

    // Use this for initialization
    void Start()
    {
        objectNma = objectToSpawn.GetComponent<NavMeshAgent>();

        spawnedObjects = new List<GameObject>();
        for (int i = 0; i < numberOfSpawn; i++)
        {
            GameObject spawnedObject = spawneBot();

        }

        

        if(tryRadius)   tryRadiuses();
    }



    GameObject spawneBot()
    {

        Vector3 spawnPos = Vckrs.generateRandomPositionBetweenCircles(transform.position, radius1, radius2);
        if (objectNma)
        {
           if( Vckrs.findNearestPositionOnNavMesh(spawnPos, objectNma.areaMask, 20f, out spawnPos))
            {
                //Debug.Log("Found pos. Mask: " + (objectNma.areaMask).ToString());
            }

        }



        //GameObject spawnedObject = (GameObject)(Instantiate(objectToSpawn.gameObject,, Quaternion.LookRotation(Vector3.right,Vector3.up)));
        GameObject spawnedObject = Instantiate(objectToSpawn) as GameObject;
        spawnedObjects.Add(spawnedObject);

        spawnedObject.transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        spawnedObject.transform.position = spawnPos;
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


[CustomEditor(typeof(SpawnBotsBetweenToCircles))]
public class SpawnBotsBetweenToCirclesEditor : Editor
{

    private SpawnBotsBetweenToCircles script;

    public void OnSceneGUI()
    {
        script = this.target as SpawnBotsBetweenToCircles;
        Handles.color = Color.red;
        Handles.DrawWireDisc(script.transform.position + (script.transform.forward) // position
                                      , script.transform.forward                       // normal
                                      , script.radius1);                              // radius


        Handles.color = Color.blue;
        Handles.DrawWireDisc(script.transform.position + (script.transform.forward) // position
                                      , script.transform.forward                       // normal
                                      , script.radius2);                              // radius
    }
}
