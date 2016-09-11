using UnityEngine;
using System.Collections;

public class SubtitleEnterTrigger : MonoBehaviour {


    public bool ifDesroyItself = true;
    //NavMeshAgent agent;

    GameObject player;
    void Awake()
    {



    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
       // agent = player.GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player")
        {
           // agent.Stop();
            gameObject.GetComponent<SubtitleController>().startSubtitle();
            if (ifDesroyItself)
                Destroy(this);

        }

    }

}
