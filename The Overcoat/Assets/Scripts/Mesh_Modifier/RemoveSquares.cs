using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//_RemoveSquares.cs
//_Dependent to:
//_To do: Split functionalities to different scripts.

//This script enables player to remove squares from mesh of object with clicks.
//It also adds a texture on mesh randomely.
//When all squares deleted it calls a cinema director cutscene.



    
    public class RemoveSquares : MonoBehaviour {


  

    Mesh mesh;
    List<int> indices;
    public Object scar;
    Vector3 hitPosition;
    Vector3 normalh;

    int triNumber;
    
    List<int> removedInd;
    public GameObject camObj;
    Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = camObj.GetComponent<Camera>();
        removedInd = new List<int>();

        Vector3 pos = transform.position;
        mesh = GetComponent<MeshFilter>().mesh;
        indices = new List<int>(mesh.triangles);

            triNumber = mesh.triangles.Length;
    }

    // Update is called once per frame
    void Update()
    {
         //   print(mesh.triangles.Length);

            if (mesh.triangles.Length <= triNumber/2)
        {
            finish();
        }



        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "RemovableMesh")
            {

                //these are for spawning
                hitPosition = hitInfo.point;
                normalh = hitInfo.normal;

                int index = hitInfo.triangleIndex;
              
                indices = new List<int>(mesh.triangles);
                Vector3[] vertices = mesh.vertices;

                Vector3 p0 = vertices[indices[index * 3 + 0]];
                Vector3 p1 = vertices[indices[index * 3 + 1]];
                Vector3 p2 = vertices[indices[index * 3 + 2]];

                float edge1 = Vector3.Distance(p0, p1);
                float edge2 = Vector3.Distance(p0, p2);
                float edge3 = Vector3.Distance(p1, p2);

                Vector3 shared1;
                Vector3 shared2;
                if (edge1 > edge2 && edge1 > edge3)
                {
                    shared1 = p0;
                    shared2 = p1;

                } else if (edge2 > edge1 && edge2 > edge3)
                {
                    shared1 = p0;
                    shared2 = p2;
                }
                else
                {
                    shared1 = p1;
                    shared2 = p2;

                }

                mesh = transform.GetComponent<MeshFilter>().mesh;


                int v1 = findVertex(shared1);
                int v2 = findVertex(shared2);

                int otherTri = findTriangle(vertices[v1], vertices[v2], index);
                deleteSquare(index, otherTri);

                    //deleteTriangle(hitInfo.triangleIndex);
                    //removedIndeces.Add(index);
                    //removedIndeces.Add(otherTri);

                    generateScar(hitPosition,hitInfo.normal);
                }



        }


    }


    int findTriangle(Vector3 v1, Vector3 v2, int notTriIndex)
    {
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        int j = 0;

        while (j < triangles.Length)
        {
            if (j / 3 != notTriIndex)
            {
                if (vertices[triangles[j]] == v1 && (vertices[triangles[j + 1]] == v2 || vertices[triangles[j + 2]] == v2))
                    return j / 3;
                else if (vertices[triangles[j]] == v2 && (vertices[triangles[j + 1]] == v1 || vertices[triangles[j + 2]] == v1))
                    return j / 3;
                else if (vertices[triangles[j + 1]] == v2 && (vertices[triangles[j]] == v1 || vertices[triangles[j + 2]] == v1))
                    return j / 3;
                else if (vertices[triangles[j + 1]] == v1 && (vertices[triangles[j]] == v2 || vertices[triangles[j + 2]] == v2))
                    return j / 3;


            }
            j += 3;

        }
        return -1;
    }



    int findVertex(Vector3 v)
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i] == v)
            {
                return i;
            }
        }
        return -1;
    }








        //void deleteSquare(int index1, int index2)
        //{


        //    Destroy(this.gameObject.GetComponent<MeshCollider>());
        //    mesh = transform.GetComponent<MeshFilter>().mesh;

        //    List<int> triangles = mesh.triangles.ToList<int>();

        //    for (int i = 0, j = 1, k = 2; i < triangles.Count; i += 3, j += 3, k += 3)
        //    {

        //        if (triangles[i] == index1 || triangles[i] == index2 || triangles[j] == index1 || triangles[j] == index2 || triangles[k] == index1 || triangles[k] == index2)
        //        {
        //            triangles.RemoveRange(i,3);

        //        }


                
        //    }
        //    mesh.triangles = triangles.ToArray();
        //    this.gameObject.AddComponent<MeshCollider>();
        //}

        void deleteSquare(int index1, int index2)
        {

            Destroy(this.gameObject.GetComponent<MeshCollider>());
            mesh = transform.GetComponent<MeshFilter>().mesh;

            int[] old = mesh.triangles;
            int[] newt = new int[mesh.triangles.Length - 3];
            int i = 0;
            int j = 0;

            while (j < mesh.triangles.Length)
            {
                if (j != index1 * 3 && j != index2 * 3)
                {
               
                    newt[i++] = old[j++];
                    newt[i++] = old[j++];
                    newt[i++] = old[j++];
                }
                else
                {
                 
                    j += 3;
                }
            }

            mesh.triangles = newt;


            this.gameObject.AddComponent<MeshCollider>();
        }




        void deleteTriangle(int index)
    {

        index = index * 3;

        indices.RemoveRange(index, 3);

        Destroy(this.gameObject.GetComponent<MeshCollider>());

        mesh.triangles = indices.ToArray();
        this.gameObject.AddComponent<MeshCollider>();
    }

    void generateScar(Vector3 pos, Vector3 normal)
    {
        GameObject obj = (GameObject)Instantiate(scar, pos, Quaternion.FromToRotation(Vector3.forward, normal));
        obj.transform.parent = transform;

    }

    List<int> findNeighbours(Mesh m, int index)
    {
        int[] triangles = m.triangles;
        List<int> neighbours = new List<int>();
        for (int i = 0; i < triangles.Length; i += 3)
        {
            if (triangles[i] == index || triangles[i + 1] == index || triangles[i + 2] == index)
            {
                if (triangles[i] != index && !neighbours.Contains(triangles[i])) neighbours.Add(triangles[i]);
                if (triangles[i + 1] != index && !neighbours.Contains(triangles[i + 1])) neighbours.Add(triangles[i + 1]);
                if (triangles[i + 2] != index && !neighbours.Contains(triangles[i + 2])) neighbours.Add(triangles[i + 2]);
            }
        }
        return neighbours;
    }

    bool checkSmall360(int index, List<int> neighbours, Mesh m)
    {
        List<Vector3> vectors = new List<Vector3>();
        float angle = 0;
        for (int i = 0; i < neighbours.Count; i++)
        {
            vectors.Add(m.vertices[neighbours[i]] - m.vertices[index]);
        }

        for (int i = 1; i < vectors.Count; i++)
        {
            angle += Vector3.Angle(vectors[i], vectors[i - 1]);
        }
        print(angle);
        return (angle < 360);
    }

    double IsVertexOnMeshBorder(int VertexId, Mesh TheMesh)
    {
        double SumOfAngles = 0;
        int[] triangles = TheMesh.triangles;
        Vector3[] vertices = TheMesh.vertices;
        for (int i = 0, j = 1, k = 2; i < triangles.Length; i += 3, j += 3, k += 3)
        {
            // Check to see if one of the triangle's points is the vertex we are checking.
            // If it is, we add the angle between the two triangle edges that terminate
            // at the vertex. Triangle edges should go around the triangle in clockwise
            // order assuming the normals are facing outward.
            if (triangles[i] == VertexId)
            {
                Vector3 EdgeA = vertices[triangles[k]] - vertices[VertexId];
                Vector3 EdgeB = vertices[triangles[j]] - vertices[VertexId];
                SumOfAngles += Vector3.Angle(EdgeA, EdgeB);
            }
            else if (triangles[j] == VertexId)
            {
                Vector3 EdgeA = vertices[triangles[i]] - vertices[VertexId];
                Vector3 EdgeB = vertices[triangles[k]] - vertices[VertexId];
                SumOfAngles += Vector3.Angle(EdgeA, EdgeB);
            }
            else if (triangles[k] == VertexId)
            {
                Vector3 EdgeA = vertices[triangles[j]] - vertices[VertexId];
                Vector3 EdgeB = vertices[triangles[i]] - vertices[VertexId];
                SumOfAngles += Vector3.Angle(EdgeA, EdgeB);
            }
        }
        // You might want to allow for some inaccuracy here since you're dealing with floating point numbers
        // return SumOfAngles < 359 ? true : false;
        return SumOfAngles;
    }


    void finish()
    {
        CallCoroutine cc = GetComponent<CallCoroutine>();
        cc.call(); 
        Destroy(transform.parent.gameObject);

    }

}
