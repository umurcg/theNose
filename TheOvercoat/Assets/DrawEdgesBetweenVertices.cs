using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MovementEffects;



//Put this script to parent object of vertices objects
//This scripts creates a game that player can conect each vertices with  mouse. It makes these connetions with line renderer.
public class DrawEdgesBetweenVertices : MonoBehaviour {

    MeshToVertices mto;
    int[] triangles;
    public float width = 0.3f;
    GameObject selectedVertex;
    Dictionary<LineRendererCylinder,GameObject[]> lrs_vertexPairs;
    
    //Use this dictionary for preventing duplicating connections
    //Dictionary<int, int> connectedPairs;

    Camera mainCam;

    public int numberOfEdges;
    int drawnEdges=0;

    public Material edgeMaterial;

    public GameObject messageReciever;
    public string edgeIsDrawnMessage;
    public string allEdgesAreDrawnMessage;

    private void Awake()
    {
        lrs_vertexPairs = new Dictionary<LineRendererCylinder, GameObject[]>();
        //connectedPairs = new Dictionary<int, int>();
    }

    private void Start()
    {
        mto = GetComponent<MeshToVertices>();
        triangles = mto.getTriangles();
        mainCam = CharGameController.getCamera().GetComponent<Camera>();

        for (int i = 0; i < triangles.Length; i += 3)
            Debug.Log(triangles[i] + " " + triangles[i + 1] + " " + triangles[i + 2]);

        ////Thanks to euler
        //numberOfEdges = triangles.Length/3 + mto.getVertices().Length - 2;
        //Debug.Log("number of vertex "+ mto.getVertices().Length+" number of faces "+triangles.Length/3);
    }

    private void Update()
    {

        //Ray cast for showing pointed vertex
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);



        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "ActiveObject")
            {
                if (selectedVertex == null)
                {
                    //Create line renderer
                    selectedVertex = hit.transform.gameObject;
                    GameObject lrObject = new GameObject();
                    lrObject.transform.parent = selectedVertex.transform;
                    LineRendererCylinder lr = lrObject.AddComponent<LineRendererCylinder>();
                    lr.setMaterial(edgeMaterial);
                    lr.SetPosition(0, selectedVertex.transform.position);
                    lr.radiusOfCylinder = width;
                    GameObject[] vertexPair = new GameObject[2];
                    vertexPair[0] = selectedVertex;
                    lrs_vertexPairs.Add(lr, vertexPair);

                }
                else
                {
                    //Check vertex wether it is connected to previous selected vertex or not

                    int v1 = int.Parse(selectedVertex.name);
                    int v2 = int.Parse(hit.transform.name);


                    if (isConnected(v1,v2) && !isEdgeDrawn(hit.transform.gameObject,selectedVertex))
                    {
                        Debug.Log("Connected");
                        connectSelecteVertexToAnotherVertex(hit.transform.gameObject);
                        

                        if (numberOfEdges != 0)
                        {
                            drawnEdges++;
                            if (drawnEdges == numberOfEdges) shapeIsDrawn();
                        }

                        messageReciever.SendMessage(edgeIsDrawnMessage);

                    }
                    else
                    {
                        Debug.Log("Not Connected");
                        clearSelectedVertex();
                    }
                }

            }
            else
            {
                //Empty selected vertex
                clearSelectedVertex();
            }


        }
        else if (Input.GetMouseButtonDown(0)) clearSelectedVertex();

        updateLineRenderePositions();



    }

    private void shapeIsDrawn()
    {
        messageReciever.SendMessage(allEdgesAreDrawnMessage);
    }

    void clearSelectedVertex()
    {
        if (selectedVertex == null) return;
        LineRendererCylinder lr = getUncompletedLR(selectedVertex);

        if (!lr) return;

        lrs_vertexPairs.Remove(lr);
        Destroy(lr.gameObject);
        selectedVertex = null; 
    }

    //Gets uncompleted line renderer of vertex
    LineRendererCylinder getUncompletedLR(GameObject vertex)
    {
        LineRendererCylinder[] allLR = vertex.GetComponentsInChildren<LineRendererCylinder>();
        foreach(LineRendererCylinder lr in allLR)
        {
            if (lrs_vertexPairs[lr][1] == null) return lr;
        }
        Debug.Log("Couldn't find uncompletedLR");
        return null;
    }

    void updateLineRenderePositions()
    {
        foreach(KeyValuePair<LineRendererCylinder, GameObject[]> elem in lrs_vertexPairs)
        {
            //If both of vertexPairs is not assigned then pass this loop
            if (elem.Value[0] == null || elem.Value[1] == null) continue;
            elem.Key.setStartAndPos(elem.Value[0].transform.position, elem.Value[1].transform.position);
            //Debug.Log("Connecting other vertex. position is " + elem.Value[1].transform.position);
            //elem.Key.SetPosition(0, elem.Value[0].transform.position);
            //elem.Key.SetPosition(1, elem.Value[1].transform.position);

        }

        if (selectedVertex != null)
        {
            Vector3[] positions = new Vector3[2];
            positions[0] = selectedVertex.transform.position;
            positions[1] = mainCam.ScreenToWorldPoint(Input.mousePosition);//Input.mousePosition;
            getUncompletedLR(selectedVertex).setStartAndPos(positions[0],positions[1]);
        }
    }


    //Return wether or not one vertex is connected to another in triangles array
    bool isConnected(int a, int b)
    {
        Debug.Log(a + " " + b);
        if (a == b) return false;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            //Is a in this triangle
            //Debug.Log(triangles[0] + " " + triangles[1] + " " + triangles[2]);
            bool isInTriangle = (triangles[i+0] == a || triangles[i+1] == a || triangles[i+2] == a);
            if (isInTriangle)
            {
                //Debug.Log(a + " is in triangle. b is "+b+" this triangle"+ triangles[0]+" "+ triangles[1]+" "+ triangles[2]);
                //Is b in this triabgle
                if ((triangles[i+0] == b || triangles[i+1] == b || triangles[i+2] == b))
                {
                    return true;
                }
            }
        }

        return false;

    }

    //Return wheter and edge drawn between two vertices
    bool isEdgeDrawn(GameObject a, GameObject b)
    {
        //Get all line renderers in children
        List<LineRendererCylinder> lrs = new List<LineRendererCylinder>();
        lrs.AddRange(a.GetComponentsInChildren<LineRendererCylinder>().ToList());
        lrs.AddRange(b.GetComponentsInChildren<LineRendererCylinder>().ToList());

        Debug.Log("Number of line renderer " + lrs.Count);

        //Look for all line renderers to check wether or not its game objects are a and b. If they are then return true else return false
        foreach(LineRendererCylinder lr in lrs)
        {
            GameObject[] pair = lrs_vertexPairs[lr];
            if ((pair[0] == a && pair[1] == b) || (pair[1] == a && pair[0] == b))
            {
                Debug.Log("This edge is already drawn");
                return true;
            }
        }

        return false;

    }

    bool isEdgeDrawn(int a,int b)
    {
        //Debug.Log("Vertex 1 is " + a + " 2 is " + b);
        return isEdgeDrawn(transform.GetChild(a).gameObject, transform.GetChild(b).gameObject);
    }

    void connectSelecteVertexToAnotherVertex(GameObject otherVertex)
    {
        if(selectedVertex==null)
        {
            Debug.Log("You shouldnt call this method when there is no selected vertex");
            return;
        }
        LineRendererCylinder lr =getUncompletedLR(selectedVertex);
        lrs_vertexPairs[lr][1] = otherVertex;
        selectedVertex = null;
        //connectedPairs.Add(int.Parse(lrs_vertexPairs[lr][0].name), int.Parse(lrs_vertexPairs[lr][0].name));
    }

    public int getRemainedNumberOfEdges()
    {
        return numberOfEdges - drawnEdges;
    }

    public void giveHint()
    {
        Timing.RunCoroutine(_giveHint());
    }


    IEnumerator<float> _giveHint()
    {
        //Debug.Log(getRemainedNumberOfEdges());
        if (getRemainedNumberOfEdges() <= 0)
        {
            //Debug.Log("No edge is left");
            yield break;
        }

        GameObject[] foundPair=null;

        //Look for random pairs that is connected but not drawn
        while (foundPair == null)
        {
            Debug.Log("Searching pair");
            int randomIndex = Random.Range(0, triangles.Length / 3);
            if (!isEdgeDrawn(triangles[randomIndex], triangles[randomIndex + 1])) {
                foundPair = new GameObject[2];
                foundPair[0] = transform.GetChild(triangles[randomIndex]).gameObject;
                foundPair[1] = transform.GetChild(triangles[randomIndex]+1).gameObject;
            }else if(!isEdgeDrawn(triangles[randomIndex], triangles[randomIndex + 2]))
            {
                Debug.Log("Second for");
                foundPair = new GameObject[2];
                foundPair[0] = transform.GetChild(triangles[randomIndex]).gameObject;
                foundPair[1] = transform.GetChild(triangles[randomIndex] + 2).gameObject;
            }

            yield return 0;
        }

        GameObject lrObject = new GameObject();
        lrObject.transform.parent = foundPair[0].transform;
        LineRendererCylinder lr = lrObject.AddComponent<LineRendererCylinder>();
        lr.setMaterial(edgeMaterial);
        lr.setStartAndPos(foundPair[0].transform.position, foundPair[1].transform.position);

        lr.radiusOfCylinder = width;
        lrs_vertexPairs.Add(lr, foundPair);



        yield break;
    }

}
