using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class RunFromPlayer : MonoBehaviour {

    public enum runDirection { x, z}
    public runDirection RunDirection = runDirection.z;

    public float speed=5f;
    public float forwardValue=15;
    public float navMeshSampleRaidus = 5f;

    public string message;
    public GameObject eventReviever;
    public float catchDuration = 5f;


    GameObject player;
    NavMeshAgent nma;

    float originalSpeed;

    float catchTimer;


	// Use this for initialization
	void Start () {

        player = CharGameController.getActiveCharacter();
        nma = GetComponent<NavMeshAgent>();

        originalSpeed = nma.speed;
        
    }
	
	// Update is called once per frame
	void Update () {

        if (catchTimer > 0)
        {
            //Debug.Log(catchTimer);
            catchTimer += Time.deltaTime;

            if (catchTimer > catchDuration)
            {
                Debug.Log("Message");
                eventReviever.SendMessage(message);
                catchTimer = 0;
                enabled = false;
            }

        }

        

	}

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Entered");
        if (col.transform.gameObject == player)
        {
            Timing.RunCoroutine(_run());
            catchTimer = 0.0001f;
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.gameObject == player)
        {
            //Stop timer
            catchTimer = 0;
        }

    }
   IEnumerator<float> _run()
    {

        nma.Resume();
        nma.speed = speed;

        Vector3 destination = transform.position + player.transform.forward * forwardValue;
        //Sample dest
        NavMeshHit nmh;
        if(NavMesh.SamplePosition(destination,out nmh, navMeshSampleRaidus, nma.areaMask))
        {
            //If dest is almost in navmesh leave it like that
        }
        else
        {
            Debug.Log("Failed to sample position on navmesh");
            if (RunDirection == runDirection.x)
            {
                destination = transform.position + Vector3.right*forwardValue;
            }else
            {
                destination = transform.position + Vector3.forward * forwardValue;

            }

        }


        nma.SetDestination(destination);


        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs.waitUntilStop(gameObject));
        yield return Timing.WaitUntilDone(handler);

        nma.speed = originalSpeed;
    }

    


}
