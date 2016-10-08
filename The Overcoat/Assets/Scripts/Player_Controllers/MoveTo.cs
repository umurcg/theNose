using UnityEngine;
using System.Collections;


public class MoveTo : MonoBehaviour
{
    //public GameObject prefab;
    NavMeshAgent agent;
	public GameObject[] aims;


	public bool debug;

    
    void Awake()
    {
    
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }
  
	public void Stop(){
		agent.Stop ();
	}


	public void setDestination(int aim){

		agent.Resume ();
		agent.destination = aims[aim].transform.position;

	}

    void Update()
	{

		if (debug) {
			setDestination (0);
			debug = false;
		}

	
		if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.S)) {
			if (agent.isOnNavMesh)
				agent.Stop ();
		

    
		if (Input.GetMouseButtonDown (0)) {


                //RaycastHit[] hits = Physics.RaycastAll (Camera.main.ScreenPointToRay (Input.mousePosition));


                //	foreach (RaycastHit hit in hits) {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {

                    if (hit.transform.CompareTag("Floor"))
                    {

                        agent.Resume();

                        if (agent.isOnNavMesh)
                            agent.destination = hit.point;

                    }
                    //       Instantiate(prefab, hit.transform.position, Quaternion.identity);
                }
    
		}
    
	}
	}
}