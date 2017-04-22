using UnityEngine;
using System.Collections;

//Draw cylinder with start point and end point like line renderer
public class LineRendererCylinder : MonoBehaviour {

    GameObject cylinder;
    public Material mat;
    public Vector3 startPoint = new Vector3(0, 0, 0);
    public Vector3 endPoint = new Vector3(0, 0, 1);
    public float radiusOfCylinder=1f;
    //public GameObject start, end;

    private void Awake()
    {
        
        cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.transform.parent = transform;
        Destroy(cylinder.GetComponent<Collider>());
        if (mat != null) cylinder.GetComponent<Renderer>().material = mat;

        updateStartPointAndEndPoint();

        ////For demosnteration;
        //start = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //end = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //start.transform.position = startPoint;
        //end.transform.position = endPoint;
    }

    public void setMaterial(Material mat)
    {
        this.mat = mat;
        cylinder.GetComponent<Renderer>().material = mat;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	}

    void updateStartPointAndEndPoint()
    {

        float length = Vector3.Distance(endPoint, startPoint);
        //cylinder.transform.parent = null;
        cylinder.transform.position = Vector3.Lerp(startPoint, endPoint, 0.5f);
        cylinder.transform.up = endPoint - startPoint;
        cylinder.transform.localScale = new Vector3(radiusOfCylinder,length/2,radiusOfCylinder);
        //cylinder.transform.parent = transform;

    }

    public void setStartAndPos(Vector3 start, Vector3 end)
    {
        startPoint = start;
        endPoint = end;
        updateStartPointAndEndPoint();
    }

    //This method has same functionality with the method in lineRenderer but you can not use index except 0 and 1
    public void SetPosition(int index,Vector3 pos)
    {
        
        if (!(index == 1 || index == 0)) return;

        if (index == 0)
        {
            startPoint = pos;
        }else if(index==1)
        {
            //Debug.Log("Assigning end posint as "+pos);
            endPoint = pos;
        }
    }
     
}
