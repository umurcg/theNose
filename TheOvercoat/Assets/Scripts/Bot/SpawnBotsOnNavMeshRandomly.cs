using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


//This is fucking smart
public class SpawnBotsOnNavMeshRandomly : MonoBehaviour {

    
    public int spawnNumber;
    public Transform[] objects;
    List<GameObject> spawnedObjects;
    public float minSpeed = 1;
    public float maxSpeed = 4;
    public float radius;

    // Use this for initialization
    void Start()
    {
        spawnedObjects = new List<GameObject>();
        if (objects.Length > 0)
        {
            for (int i = 0; i < spawnNumber; i++)
            {
                GameObject spawnedObject=spawneBot();
                moveToInside(spawnedObject);
            }
        }

    }

    public List<GameObject>  getSpawnedObjects()
    {
        return spawnedObjects;
    }

    GameObject spawneBot()
    {
        Transform botType = objects[Random.Range(0, objects.Length - 1)];
        GameObject spawnedObject = (GameObject)(Instantiate(botType.gameObject, getRandomPosOnMesh(botType.gameObject), Quaternion.LookRotation(transform.forward), gameObject.transform));
        spawnedObjects.Add(spawnedObject);
        WalkToFarestOfRoadBot wtfrb = spawnedObject.GetComponent<WalkToFarestOfRoadBot>();
        if (wtfrb)
            wtfrb.obj = transform.gameObject;
        UnityEngine.AI.NavMeshAgent nma = spawnedObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (nma != null)
        {

            nma.speed = Random.Range(minSpeed, maxSpeed);
        }
        return spawnedObject;
    }



    Vector3 getRandomPosOnMesh(GameObject obj, int numberOfTry=50)
    {
        //SphereCollider sc = GetComponent<SphereCollider>();

        while (numberOfTry > 0)
        {
            Vector3 pos = getRandomPosInCircle(transform.position, radius);
            UnityEngine.AI.NavMeshHit nmh;
            UnityEngine.AI.NavMeshAgent nma = obj.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();

            //If it is last try then search in 1000
            float radiusToSearch = (numberOfTry == 1) ? 1000 : 10;

            if (UnityEngine.AI.NavMesh.SamplePosition(pos, out nmh, radiusToSearch, nma.areaMask))
            {
                return nmh.position;
      
            }
      
            numberOfTry -= 1;
        }


        Debug.Log("Couldn't find proper position on navmesh");
        return Vector3.zero;

    }

    private enum way {forward, back, right, left , empty}

    // Objects most likely spawn at edges of masks while it finds nearest poaint to randomly generated point
    //This scirpt is for changing position of spawned objects.
    //It searches new point far from 5 unit from object
    //If that oint on nev mask it changes it positions to there.
    void moveToInside(GameObject obj)
    {
        float distanceToObj = 10f ;

        Vector3 pos = obj.transform.position;

        Vector3 forwardPos = pos + obj.transform.forward * distanceToObj;
        Vector3 backwardPos = pos + obj.transform.forward * -distanceToObj;
        Vector3 rightPos = pos + obj.transform.right * distanceToObj;
        Vector3 leftPos = pos + obj.transform.right * -distanceToObj;

        Vector3[] nearPositions = new Vector3[4];/*{forwardPos,backwardPos,rightPos,leftPos };*/
        nearPositions[(int)way.forward]=forwardPos;
        nearPositions[(int)way.back] = backwardPos;
        nearPositions[(int)way.right] = rightPos;
        nearPositions[(int)way.left] = leftPos;

        way outOfBoundWay=way.empty;

        //First find which way is not in navmesh if you couldn't fine it return null
        for (int i=0;i<nearPositions.Length;i++)
        {
            UnityEngine.AI.NavMeshHit nmh;
            UnityEngine.AI.NavMeshAgent nma = obj.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            if (UnityEngine.AI.NavMesh.SamplePosition(nearPositions[i], out nmh, distanceToObj*2, nma.areaMask))
            {
                //If founded position is near of current position this mean this way is note include in navMesh
                if (Vector3.Distance(nmh.position, obj.transform.position) < distanceToObj/2)
                {
                    outOfBoundWay = (way)i;
                    //obj.transform.position = nmh.position;
                    //Debug.Log("Found new pos");
                    //return;
                }
            }
       
        }

        //if there is no out of bound return null
        if (outOfBoundWay==way.empty) return;

        obj.transform.position-=(nearPositions[(int)outOfBoundWay] - obj.transform.position);

     

    }

    Vector3 getRandomPosInCircle(Vector3 center, float radius)
    {
        Vector2 randomPos = Random.insideUnitCircle * radius;
        return (new Vector3(randomPos.x, 0, randomPos.y)) + center;

    }

    public void destroyAllBots()
    {
        if (spawnedObjects != null)
        {
            foreach (GameObject obj in spawnedObjects)
            {
                Destroy(obj);
            }
            spawnedObjects.Clear();
        }

  

        Destroy(this);
    }

}


[CustomEditor(typeof(SpawnBotsOnNavMeshRandomly))]
public class SpawnBotsOnNavMeshRandomlyEditor : Editor
{

    private SpawnBotsOnNavMeshRandomly script;

    public void OnSceneGUI()
    {
        script = this.target as SpawnBotsOnNavMeshRandomly;
        Handles.color = Color.red;
        Handles.DrawWireDisc(script.transform.position + (script.transform.forward) // position
                                      , Vector3.up                   // normal
                                      , script.radius);                              // radius
    }
}



