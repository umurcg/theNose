using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;

//Spawns object in circle within radius
//It keeps spawned objet count in constant number
//also you can cast spawn position to navmash area
public class ObjectSpawnerContinously : MonoBehaviour {

    public GameObject prefab;
    public int spawnNumber;
    public bool castNavmesh;
    public float radius;

    //Random rotation on Y axis
    public bool randomRotation = false;

    List<GameObject> spawnedObjects;

    // Use this for initialization
    void Start() {

        spawnedObjects = new List<GameObject>();

        for(int i = 0; i < spawnNumber; i++)
        {
            spawn();
        }



    }


    void spawn()
    {
        Vector3 spawnPos = Vckrs.getRandomPosInCircle(transform.position, radius,Plane.XZ);
        //Debug.Log("Garabage pos is " + spawnPos);
        UnityEngine.AI.NavMeshHit hit;
        

        if (UnityEngine.AI.NavMesh.SamplePosition(spawnPos, out hit, 100f, UnityEngine.AI.NavMesh.AllAreas/*NavMesh.GetAreaFromName("Street")*/))
        {

            if (prefab == null) Debug.Log("prefdab is null");
            GameObject spawnedOBJ=Instantiate(prefab);
            spawnedOBJ.transform.position = hit.position;
            spawnedObjects.Add(spawnedOBJ);
            spawnedOBJ.transform.parent = transform;
            SendMessage sm=spawnedOBJ.AddComponent<SendMessage>();
            sm.type = triggerType.OnDestroy;
            sm.recievers = new GameObject[] { gameObject };
            sm.message = "objectIsDestroyed";
            sm.sendWithObject = true;

            if (randomRotation) spawnedOBJ.transform.Rotate(Vector3.up, Random.Range(0f, 360f));

        }
        else
        {
            Debug.Log("FAILED TO INSTANTIATE");
        }

    }

    public void objectIsDestroyed(GameObject obj)
    {
        spawnedObjects.Remove(obj);
        spawn();
    }

    public List<GameObject> getSpawnedObjects()
    {
        return spawnedObjects;
    }

}


#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(ObjectSpawnerContinously))]
    public class ObjectSpawnerContinouslyEditor : UnityEditor.Editor
    {

        private ObjectSpawnerContinously script;

        public void OnSceneGUI()
        {
            script = this.target as ObjectSpawnerContinously;
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(script.transform.position + (script.transform.forward) // position
                                          , Vector3.up                    // normal
                                          , script.radius);                              // radius
        }
    }


#endif