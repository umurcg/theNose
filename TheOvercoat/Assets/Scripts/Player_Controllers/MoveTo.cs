using UnityEngine;
using System.Collections;


public class MoveTo : MonoBehaviour
{
    public GameObject prefab;
    NavMeshAgent agent;
    public GameObject[] aims;

    public LayerMask ignoreMasks;

    public bool debug;


    void Awake()
    {

    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    public void Stop() {
        agent.Stop();
    }


    public void setDestination(int aim) {

        agent.Resume();
        agent.destination = aims[aim].transform.position;

    }

    void Update()
    {
        ////Event e = Event.current;
        if (debug) {
            setDestination(0);
            debug = false;
        }

        if (Input.anyKey)
        {
        
            //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) || 
            //    Input.GetKey(KeyCode.UpArrow)  || Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.LeftArrow))
            if(Input.GetAxis("Horizontal")!=0||Input.GetAxis("Vertical")!=0)
            {
                //print("keyboard");
                if (agent.isOnNavMesh)
                    agent.Stop();


            }   else if (Input.GetMouseButtonDown(0))
            {
                //print("mouse");
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, ignoreMasks))

                {
                    //Debug.Log(hit.transform.name);
                    if (hit.transform.CompareTag("Floor"))
                    {
                        //Debug.Log("Walking");
                        agent.Resume();

                        if (agent.isOnNavMesh)
                        {


                            agent.destination = hit.point;
                        }
                    }
                }

            }



        }

    }

}