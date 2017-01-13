using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DrawMesh : MonoBehaviour {

    public GameObject cube;
    public GameObject verticesObject;
    public Vector3 forwardVector;

    LineRenderer lr;

	// Use this for initialization
	void Start () {
        lr = GetComponent<LineRenderer>();
        drawVertices(cube.transform.position, cube.GetComponent<MeshFilter>().mesh);
	}

    // Update is called once per frame
    void Update()
    {

        
        lr.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));



        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {

            if (Physics.Raycast(ray, out hit))

            {

                if (hit.transform.tag == "Vertex")
                {
                    Renderer rend = hit.transform.GetComponent<Renderer>();
                    Material m = rend.material;
                    Color col = m.color;
                    col.b = 1;
                    m.color = col;
                    rend.material = m;

                    lr.SetPosition(0, hit.transform.position);
                    

                }
            }
        }
    }



    void drawVertices(Vector3 origin, Mesh m)
    {
        //Vector3[] vertices = m.vertices;
        Vector3[] vertices = removeDuplicates(m.vertices);


        foreach (Vector3 vertexPos in vertices)
        {
            //print(vertexPos);
            GameObject obj= Instantiate(verticesObject);
            obj.transform.position = origin+vertexPos;
        }
    }

    
    Vector3[] removeDuplicates(Vector3[] array)
    {
        List<Vector3> list = new List<Vector3>(array);
        List<Vector3> newList = list.Distinct().ToList();
        return newList.ToArray();
               

    }

    //TODO Rotate
    Vector3[] rotateVerticesInY(Vector3[] vertices, float angle)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices[i].x * Mathf.Cos(Mathf.Deg2Rad * angle), vertices[i].y, vertices[i].z * Mathf.Sin(Mathf.Deg2Rad * angle));
        }
        return vertices;
    }

}
