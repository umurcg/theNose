using UnityEngine;
using System.Collections;

public class ClickTrigger: MonoBehaviour {

   

    GameObject player;
    NavMeshAgent agent;

    bool isInTrigger = false;
    bool isMoving = false;
	CharacterMouseLook mouseLookScript;
    // public static bool isTriggersActive = true;

	public float radius=5f;

   

    // Use this for initialization
    void Start () {
	
	}


    void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        agent = player.GetComponent<NavMeshAgent>();
    
		mouseLookScript = player.GetComponent<CharacterMouseLook> ();

    }

    // Update is called once per frame
    void Update()
    {

		if (Vector3.Distance (player.transform.position, transform.position)<radius) {
			if (isMoving) {

				agent.Stop();


				mouseLookScript.LookTo (transform.position, 1f);
				isMoving = false;



				IClickAction iclick = GetComponent<IClickAction> ();
				iclick.Action ();
			}

			isInTrigger = true;
		}



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
                            
							  
							IClickAction iclick = GetComponent<IClickAction> ();
							iclick.Action ();
//                            gameObject.GetComponent<SubtitleController>().startSubtitle();
//                            if (ifDesroyItself)
//								Destroy(this);
                        	

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


			mouseLookScript.LookTo (transform.position, 1f);

            isMoving = false;

				IClickAction iclick = GetComponent<IClickAction> ();
				iclick.Action ();
//                gameObject.GetComponent<SubtitleController>().startSubtitle();
//                if (ifDesroyItself)
//                    Destroy(gameObject.GetComponent<ScriptClickTrigger>());
            

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
