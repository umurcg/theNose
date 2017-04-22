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

    Dictionary<int,int> posToVertexId;
    int[] triangles;

    // Use this for initialization
    void Awake () {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Optimize();
        //Debug.Log(mesh.vertexCount);
        //for (int i = 0; i < mesh.triangles.Length; i++)
        //{

        //    Debug.Log(mesh.triangles[i]);
        //}

        posToVertexId = new Dictionary<int, int>();
        triangles = new int[mesh.triangles.Length];

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
        for(int i=0;i<vertices.Length;i++) //foreach (Vector3 vertexPos in vertices)
        {
            Vector3 vertexPos = vertices[i];
            //print(vertexPos);
            GameObject obj= Instantiate(verticesObject);
            obj.transform.position = origin+vertexPos;
            obj.transform.parent = transform;
            spawnedObjects.Add(obj);
            obj.transform.tag = verticesTag;
            obj.transform.name = i.ToString();

            //Debug.Log("assigning "+obj.transform.position);
            posToVertexId.Add((vertexPos + origin).GetHashCode(), i);
        }

        //Create triangles array considering removed vertices
        for(int i = 0; i < m.triangles.Length; i++)
        {
            int index = m.triangles[i];
            Vector3 vertexPos = m.vertices[index];
            //Debug.Log("seraihn "+(vertexPos + origin));
            triangles[i] = posToVertexId[(vertexPos + origin).GetHashCode()];
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
        //int[] clearedTriangle = new int[mesh.triangles.Length];
        //Dictionary<Vector3, int> posToId = new Dictionary<Vector3, int>();

        //for(int i = 0; i < mesh.triangles.Length; i++)
        //{
        //       if(!posToId.ContainsKey(mesh.vertices[mesh.triangles[i]]))
        //    {
        //        clearedTriangle[i] = mesh.triangles[i];
        //        posToId.Add(mesh.vertices[mesh.triangles[i]], mesh.triangles[i]);
        //    }else
        //    {
        //        clearedTriangle[i] = posToId[mesh.vertices[mesh.triangles[i]]];
        //    }
        //}

        //return clearedTriangle;
        return triangles;
    }

    public GameObject[] getVertices()
    {
        return verticesAsGameObject;
    }



}
