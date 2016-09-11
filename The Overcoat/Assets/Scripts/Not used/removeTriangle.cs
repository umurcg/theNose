using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class removeTriangle : MonoBehaviour {
    Mesh mesh;
    List<int> indices;
    List<int> removedIndeces;
    // Use this for initialization
    void Start () {

        //mesh = GetComponent<MeshFilter>().mesh;
        //indices = new List<int>(mesh.triangles);
        removedIndeces = new List<int>();
        Vector3 pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       
     


        if (Input.GetMouseButton(0)||Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "RemovableMesh")
            {

                mesh = transform.GetComponent<MeshFilter>().mesh;
                indices = new List<int>(mesh.triangles);
                Vector3[] vertices = mesh.vertices;
                
                int index = hitInfo.triangleIndex;

                


                deleteTriangle(hitInfo.triangleIndex);
                removedIndeces.Add(index);
                    
                
            }

        }
    }


    void deleteTriangle(int index)
    {
        
        index = index * 3;

        indices.RemoveRange(index, 3);
 
        Destroy(this.gameObject.GetComponent<MeshCollider>());
        
        mesh.triangles = indices.ToArray();
        this.gameObject.AddComponent<MeshCollider>();
    }
}
