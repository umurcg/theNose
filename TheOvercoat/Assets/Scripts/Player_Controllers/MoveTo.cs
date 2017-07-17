using UnityEngine;
using System.Collections;


public class MoveTo : MonoBehaviour
{
    public GameObject prefab;
    UnityEngine.AI.NavMeshAgent agent;
    //public GameObject[] aims;

    public LayerMask ignoreMasks;

    public bool printShootedObjects;


    void Awake()
    {

    }
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

    }

    public void Stop() {
        agent.Stop();
    }


    //public void setDestination(int aim) {

    //    agent.Resume();
    //    agent.destination = aims[aim].transform.position;

    //}

    void Update()
    {
        //////Event e = Event.current;
        //if (debug) {
        //    setDestination(0);
        //    debug = false;
        //}

        //Debug.Log(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);



        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //print("keyboard");
            if (agent.isOnNavMesh)
                agent.Stop();


        }
       

    if (Input.anyKey)
        {

            //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) || 
            //    Input.GetKey(KeyCode.UpArrow)  || Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.LeftArrow))

        
            if (Input.GetMouseButtonDown(0) && !ButtonCursorSetter.mouseIsOverButton)
            {
                //print("mouse");
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, ignoreMasks))

                {
                    //Debug.Log(Vckrs.nameTagLayer(hit.transform.gameObject));
                    if (hit.transform.CompareTag("Floor"))
                    {
                        //Debug.Log("Walking");
                        agent.Resume();

                        if (agent.isOnNavMesh)
                        {

                            //Debug.Log("Walk");
                            agent.destination = hit.point;
                        }
                    }
                }

            }



        }

    }

}