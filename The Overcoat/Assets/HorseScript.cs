using UnityEngine;
using System.Collections;

public class HorseScript : MonoBehaviour, IClickAction {

    public GameObject[] aims;
    public bool debugButton=false;
    NavMeshAgent nma;
    

	// Use this for initialization
	void Awake() {  
        nma = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {





	
	}

    public void Action()
    {
        FlyCameraBetween fcb = Camera.main.gameObject.GetComponent<FlyCameraBetween>();
        if (fcb)
        {
            fcb.fly();
        }

    }

    public void setDes(Vector3 pos)
    {
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(pos, out myNavHit, 100, nma.areaMask))
        {
            nma.SetDestination(myNavHit.position);
          
        }

        debugButton = false;

    }

    public void goToAim(int index)
    {
        if (index < aims.Length)
        {
            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(aims[index].transform.position, out myNavHit, 100, nma.areaMask))
            {
                nma.SetDestination(myNavHit.position);

            }

            debugButton = false;
        }
    }
}
