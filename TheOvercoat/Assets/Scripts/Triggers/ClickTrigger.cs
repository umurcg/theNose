using UnityEngine;
using System.Collections;

public class ClickTrigger: MonoBehaviour {

   

    GameObject player;
    NavMeshAgent agent;

    public bool isInTrigger = false;
    bool isMoving = false;


	CharacterMouseLook mouseLookScript;
    // public static bool isTriggersActive = true;

	public float radius=5f;
	IClickAction[] iclicks;

    public static bool disabled = false;

    // Use this for initialization
    void Start () {
	
	}


    void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        agent = player.GetComponent<NavMeshAgent>();
    
		mouseLookScript = player.GetComponent<CharacterMouseLook> ();

		iclicks = GetComponents<IClickAction> ();

    }

    // Update is called once per frame
    void Update()
    {

        //if (disabled&&this.enabled==true)
        //{
        //    this.enabled = false;
        //} else if (disabled ==false&& this.enabled == false)
        //{
        //    this.enabled = true;
        //}




		if (Input.anyKeyDown && isMoving) {
			isMoving = false;
		} else if (isMoving) {
			if (agent.destination != transform.position) {
				agent.destination = transform.position;
			}

		}

		if (Vector3.Distance (player.transform.position, transform.position) < radius) {
			if (isMoving) {

				agent.Stop ();


				mouseLookScript.LookTo (transform.position, 1f);
				isMoving = false;


                callAction();
            }

			isInTrigger = true;
		} else {
			isInTrigger = false;
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
                        //			print ("is in treifereknmjfoanfoa");       

                        callAction();

                        //iclick.Action ();
                        //			print (transform.name);
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


            callAction();
            //                gameObject.GetComponent<SubtitleController>().startSubtitle();
            //                if (ifDesroyItself)
            //                    Destroy(gameObject.GetComponent<ScriptClickTrigger>());


        }
        isInTrigger = true;
		//print ("Trigger "+transform.name+" entered");

    }

    void OnTriggerExit(Collider other)
    {
        isInTrigger = false;
		//print ("Trigger "+transform.name+" exit ");
    }


    void callAction()
    {

        //if(iclick!=null)
        //	iclick.Action ();

        if (iclicks != null)
            foreach (IClickAction ic in iclicks)
                ic.Action();

        ISubtitleTrigger ist = GetComponent<ISubtitleTrigger>();
        if (ist != null)
        {
            ist.callSubtitle();
        }
        

    }

}
