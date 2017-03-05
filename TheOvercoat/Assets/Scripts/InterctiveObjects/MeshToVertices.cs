using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


//It transofrms mesh to vertices
public class MeshToVertices : MonoBehaviour {

    
    public GameObject verticesObject;
    public string verticesTag;
    public float scale;
    Mesh mesh;

    GameObject[] verticesAsGameObject;

    // Use this for initialization
    void Awake () {
        mesh = GetComponent<MeshFilter>().mesh;
        
        drawVertices(transform.position, mesh);

        //Hide mesh rendderer
        GetComponent<MeshRenderer>().enabled = false;
	}

    // Update is called once per frame
    void Update()
    {


    }


    void drawVertices(Vector3 origin, Mesh m)
    {
        //Remove doubles of vertices
        Vector3[] vertices = removeDuplicates(m.vertices);

        //Create a list holding instantiated objects for vertices
        List<GameObject> spawnedObjects = new List<GameObject>();

        //Instantiate obj for each vertices according to their position
        foreach (Vector3 vertexPos in vertices)
        {
            //print(vertexPos);
            GameObject obj= Instantiate(verticesObject);
            obj.transform.position = origin+vertexPos;
            obj.transform.parent = transform;
            spawnedObjects.Add(obj);
            obj.transform.tag = verticesTag;
        }


        //Set rotation same as mesh
        Vector3 center = transform.position;//any V3 you want as the pivot point.

        for (int i = 0; i < spawnedObjects.Count; i++)
        {//vertices being the array of vertices of your mesh
            spawnedObjects[i].transform.position = transform.rotation * (spawnedObjects[i].transform.position - center) + center;

            //Scale
            spawnedObjects[i].transform.position *= scale;
        }

        verticesAsGameObject = spawnedObjects.ToArray();

    }

    

    Vector3[] removeDuplicates(Vector3[] array)
    {
        List<Vector3> list = new List<Vector3>(array);
        List<Vector3> newList = list.Distinct().ToList();
        return newList.ToArray();
               

    }

    public int[] getTriangles()
    {
        return mesh.triangles;
    }

    public GameObject[] getVertices()
    {
        return verticesAsGameObject;
    }

}
