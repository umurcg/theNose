using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine.AI;

public class SpawnBotsBetweenToCircles : MonoBehaviour {

    public GameObject objectToSpawn;
    protected NavMeshAgent objectNma;
    public int numberOfSpawn;
    protected List<GameObject> spawnedObjects;
    public float radius1;
    public float radius2;
    public bool tryRadius;

    

    // Use this for initialization
    protected virtual void Start()
    {
        objectNma = objectToSpawn.GetComponent<NavMeshAgent>();

        spawnedObjects = new List<GameObject>();
        for (int i = 0; i < numberOfSpawn; i++)
        {
            GameObject spawnedObject = spawneBot();

        }

        

        if(tryRadius)   tryRadiuses();
    }



    protected virtual GameObject spawneBot()
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
#if UNITY_EDITOR

[UnityEditor.CustomEditor(typeof(SpawnBotsBetweenToCircles))]
public class SpawnBotsBetweenToCirclesEditor : UnityEditor.Editor
{

    private SpawnBotsBetweenToCircles script;

    public void OnSceneGUI()
    {
        script = this.target as SpawnBotsBetweenToCircles;
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(script.transform.position + (script.transform.forward) // position
                                      , script.transform.forward                       // normal
                                      , script.radius1);                              // radius


        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc(script.transform.position + (script.transform.forward) // position
                                      , script.transform.forward                       // normal
                                      , script.radius2);                              // radius
    }
}

#endif