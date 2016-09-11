using UnityEngine;
using System.Collections;

public class ClickTrigger: MonoBehaviour {

   

    GameObject player;
    NavMeshAgent agent;

    bool isInTrigger = false;
    bool isMoving = false;

    // public static bool isTriggersActive = true;


   

    // Use this for initialization
    void Start () {
	
	}


    void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        agent = player.GetComponent<NavMeshAgent>();
 

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {

                    if (isInTrigger)
                    {
                        if (gameObject.GetComponent<SubtitleController>() != null)
                        {
							  
							IClickAction iclick = GetComponent<IClickAction> ();
							iclick.Action ();
//                            gameObject.GetComponent<SubtitleController>().startSubtitle();
//                            if (ifDesroyItself)
//								Destroy(this);
                        }

                    } else if (!isInTrigger)
                    {
                        

                        agent.Resume();
                        agent.destination = transform.position;  
                        isMoving = true;
                
                    }


                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        

        if (isMoving)
        {
           
            agent.Stop();
            isMoving = false;
            if (gameObject.GetComponent<SubtitleController>() != null)
            {
				IClickAction iclick = GetComponent<IClickAction> ();
				iclick.Action ();
//                gameObject.GetComponent<SubtitleController>().startSubtitle();
//                if (ifDesroyItself)
//                    Destroy(gameObject.GetComponent<ScriptClickTrigger>());
            }

        }
        isInTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        isInTrigger = false;
    }
    void OnTriggerStay(Collider other)
    {

          isInTrigger = true;


    }


}
