using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawEdgesBetweenVertices : MonoBehaviour {

    LineRenderer lr;
    int currentVertexCount = 1;
    Dictionary<int, GameObject> vertexGameObjectDict = new Dictionary<int, GameObject>();

    GameObject[] vertices;
    int[] triangles;

    int lastClickedVertex=-1;

    bool followingMouse = false;

    // Use this for initialization
    void Start () {
        lr = GetComponent<LineRenderer>();

        vertices= GetComponent<MeshToVertices>().getVertices();
        triangles = GetComponent<MeshToVertices>().getTriangles();
    }
	
	// Update is called once per frame
	void Update () {


        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(ray, out hit))

            {

                if (hit.transform.tag == "Vertex")
                {
                    int index = returnIndex(hit.transform.gameObject);

                    //if (lastClickedVertex != -1 && isConnected(lastClickedVertex, index))
                    

                        //Change color of vertex
                        Renderer rend = hit.transform.GetComponent<Renderer>();
                        Material m = rend.material;
                        Color col = m.color;
                        col.b = 1;
                        m.color = col;
                        rend.material = m;

                        currentVertexCount++;
                        lr.SetVertexCount(currentVertexCount);
                        //lr.SetPosition(currentVertexCount - 2, hit.transform.position);
                        vertexGameObjectDict.Add(currentVertexCount - 2, hit.transform.gameObject);

                    AddRemoveMouseToLineEnd(true);

                    lastClickedVertex=index;

                }
            }
            else
            {
                AddRemoveMouseToLineEnd(false);
            }
        }

        if (currentVertexCount <= 1) return;

        updateLinePositions();
        
        if(followingMouse)
        lr.SetPosition(currentVertexCount - 1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

    }

    void updateLinePositions()
    {
        foreach (KeyValuePair<int, GameObject> kV in vertexGameObjectDict)
        {
            lr.SetPosition(kV.Key, kV.Value.transform.position);

        }
    }


    //Return wether or not one vertex is connected to another in triangles array
    bool isConnected(int a, int b)
    {
        for(int i = 0; i < triangles.Length; i += 3)
        {
            //Is a in this triangle
            bool isInTriangle = (triangles[0] == a || triangles[1] == a || triangles[2] == a);
            if (isInTriangle)
            {
                //Is b in this triabgle
                if((triangles[0] == b || triangles[1] == b || triangles[2] == b))
                {
                    return true;
                }else
                {
                    return false;
                }
            }
        }

        return false;

    }

    //Return index of vertex
    int returnIndex(GameObject vertex)
    {
        for(int i = 0; i < vertices.Length; i++)
        {
            if (vertex == vertices[i]) return i;
        }

        return -1;
    }

    //Addes mouse position to line end or removes it
    void AddRemoveMouseToLineEnd(bool add)
    {
        if (followingMouse == add) return;

        if (add)
        {            
            currentVertexCount++;

        }else
        {
            currentVertexCount--;
        }
        
        lr.SetVertexCount(currentVertexCount);
        followingMouse = add;
    }


}
