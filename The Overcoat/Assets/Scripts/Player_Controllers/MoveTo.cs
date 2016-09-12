using UnityEngine;
using System.Collections;


public class MoveTo : MonoBehaviour
{
    //public GameObject prefab;
    NavMeshAgent agent;

    
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
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.S))
        {
            agent.Stop();
        }

    
            if (Input.GetMouseButtonDown(0))
            {

                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                {
                   
                    if (hit.transform.CompareTag("Floor"))
                    {
                    agent.Resume();


                    agent.destination = hit.point;
                
                }
                    //       Instantiate(prefab, hit.transform.position, Quaternion.identity);

    
            }
        }
    }
}