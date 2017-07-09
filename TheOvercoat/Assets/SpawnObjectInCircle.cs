using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class SpawnObjectInCircle : MonoBehaviour
{

    public GameObject prefab;
    public int spawnNumber;
    public bool castNavmesh;
    public float radius;

    //Random rotation on Y axis
    public bool randomRotation = false;

    void Start()
    {

   

        for (int i = 0; i < spawnNumber; i++)
        {
            spawn();
        }



    }


    void spawn()
    {
        Vector3 spawnPos = Vckrs.getRandomPosInCircle(transform.position, radius, Plane.XZ);
        //Debug.Log("Garabage pos is " + spawnPos);
        UnityEngine.AI.NavMeshHit hit;


        if (UnityEngine.AI.NavMesh.SamplePosition(spawnPos, out hit, 100f, UnityEngine.AI.NavMesh.AllAreas/*NavMesh.GetAreaFromName("Street")*/))
        {
            GameObject spawnedOBJ = Instantiate(prefab);
            spawnedOBJ.transform.position = hit.position;
            spawnedOBJ.transform.parent = transform;

            if (randomRotation) spawnedOBJ.transform.Rotate(Vector3.up, Random.Range(0f, 360f));

        }
        else
        {
            Debug.Log("FAILED TO INSTANTIATE");
        }

    }



}



[CustomEditor(typeof(SpawnObjectInCircle))]
public class SpawnObjectInCircleEditor : Editor
{

    private SpawnObjectInCircle script;

    public void OnSceneGUI()
    {
        script = this.target as SpawnObjectInCircle;
        Handles.color = Color.blue;
        Handles.DrawWireDisc(script.transform.position + (script.transform.forward) // position
                                      , Vector3.up                    // normal
                                      , script.radius);                              // radius
    }
}


