using UnityEngine;
using System.Collections;

public class ScriptClickTriggerOld : MonoBehaviour {
    public TextAsset txtEng;
    public TextAsset txtTr;
   

    Subtitle SubtitleScript;
    GameObject player;
    NavMeshAgent agent;
    SphereCollider collider;
    bool isInTrigger = false;
    bool isMoving = false;

    public static bool isTriggersActive = true;

    public bool ifDesroyItself=false;

    // Use this for initialization
    void Start () {
	
	}


    void Awake()
    {

        SubtitleScript = GameObject.FindGameObjectWithTag("Subtitle").GetComponent<Subtitle>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = player.GetComponent<NavMeshAgent>();
        collider = GetComponent<SphereCollider>();

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

                    if (isInTrigger&&isTriggersActive)
                    {
                        SubtitleScript.startDialouge(txtTr);
                        if (ifDesroyItself)
                            Destroy(this);
                    } else if (!isInTrigger&&isTriggersActive)
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

        if (isMoving&&isTriggersActive)
        {
            SubtitleScript.startDialouge(txtTr);
            agent.Stop();
            isMoving = false;
            if (ifDesroyItself)
                Destroy(this);
        }
        isInTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        isInTrigger = false;
    }
    void OnTriggerStay(Collider other)
    {
        
           if (other.transform.tag == "Player" && Input.GetKeyUp(KeyCode.Space) && isTriggersActive)
        {

            isMoving = false;
            SubtitleScript.startDialouge(txtTr);
            if (ifDesroyItself)
                Destroy(this);

        }


    }


}
