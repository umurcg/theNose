using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns bots between to circles with testing positions wether or not colliding with and object having indicated tag
/// So it prevents to spawn a bot in a mesh collider
/// </summary>
public class MazeBotSpawner : SpawnBotsBetweenToCircles {

    public string restrictedTag;
    public TriggerTest tt;
    public int numberOfTry = 50;


    protected override GameObject spawneBot()
    {
        int trial = 0;
        bool foundSuitablePos = false;

        Vector3 spawnPos=Vector3.zero;

        while (!foundSuitablePos && trial < numberOfTry)
        {
            //Generate position
            spawnPos = Vckrs.generateRandomPositionBetweenCircles(transform.position, radius1, radius2);
            
            if (objectNma)
            {
                if (Vckrs.findNearestPositionOnNavMesh(spawnPos, objectNma.areaMask, 20f, out spawnPos))
                {
                    //Debug.Log("Found pos. Mask: " + (objectNma.areaMask).ToString());
                }

            }

            tt.gameObject.transform.position = spawnPos;

            if (tt.tag != restrictedTag)
            {
                foundSuitablePos = true;
            }
            else
            {
                Debug.Log("Unsuitable posiiton looking for new one");
            }

            trial++;

        }

        if (trial >= numberOfTry) Debug.Log("Trial exceeds maximum number of try so instantiating bot in unsuitable posiition");


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

}
