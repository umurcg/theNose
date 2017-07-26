using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class RunFromPlayer : MonoBehaviour {


    public float speed=5f;
    public float forwardValue=15;
    public float navMeshSampleRaidus = 5f;

    public string message;
    public GameObject eventReviever;
    public float catchDuration = 5f;

    //Number of obstacle in sphere collider to catch nose
    //public int numberOfObstacleToFinish=5;

    GameObject player;
    UnityEngine.AI.NavMeshAgent nma;

    float originalSpeed;

    float catchTimer;

    public bool sendMessage;

    //int numberOfObstacleInCollider;
    bool obstacleInSphere = false;
    bool playerInSphere = false;


	// Use this for initialization
	void Start () {

        player = CharGameController.getActiveCharacter();
        nma = GetComponent<UnityEngine.AI.NavMeshAgent>();

        originalSpeed = nma.speed;
        
    }
	
	// Update is called once per frame
	void Update () {

        if (sendMessage)
        {
            finish();
        }

        if (catchTimer > 0)
        {
            Debug.Log(catchTimer);
            catchTimer += Time.deltaTime;

            if (catchTimer > catchDuration)
            {
                finish();
            }
        }



    }

    private void finish()
    {
        //Debug.Log("Message");
        eventReviever.SendMessage(message);
        catchTimer = 0;
        enabled = false;
    }


    void OnTriggerEnter(Collider col)
    {

        if (col.transform.tag == "Obstacle")
        {
            //Debug.Log("Obstacle In Sphere");
            obstacleInSphere = true;
        }

        //Debug.Log("Entered");
        if (col.transform.gameObject == player)
        {
            //Debug.Log("PLayer In Sphere");
            playerInSphere = true;
            Timing.RunCoroutine(_run());
        }

        //Debug.Log("Player is in sphere " + playerInSphere + " obstacle in sphere " + obstacleInSphere);

        if (obstacleInSphere && playerInSphere)
        {
            
            catchTimer = 0.0001f;
        } 

    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Obstacle")
        {
            //Debug.Log("Obstacle is not in Sphere");
            obstacleInSphere = false;
        }

        //Debug.Log("Entered");
        if (col.transform.gameObject == player)
        {
            //Debug.Log("Player is not in Sphere");
            playerInSphere = false;
        }

        if (!obstacleInSphere || !playerInSphere)
        {
            //Stop timer
            catchTimer = 0;
        }
        Debug.Log("Player is in sphere " + playerInSphere + " obstacle in sphere " + obstacleInSphere);
    }
   IEnumerator<float> _run()
    {

        nma.Resume();
        nma.speed = speed;

        Vector3 destination = transform.position + player.transform.forward * forwardValue;
        //Sample dest
        UnityEngine.AI.NavMeshHit nmh;
        if(UnityEngine.AI.NavMesh.SamplePosition(destination,out nmh, navMeshSampleRaidus, nma.areaMask))
        {
            //If dest is almost in navmesh leave it like that
        }
        else
        {

            //Try to run in z direction
            destination = transform.position + Vector3.right * forwardValue;
            if (UnityEngine.AI.NavMesh.SamplePosition(destination, out nmh, navMeshSampleRaidus, nma.areaMask))
            {
                //If dest is almost in navmesh leave it like that
            } else
            {
                //Try to run in x direction if z is not valid, I hope it is valid else fuck it i am out.

                destination = transform.position + Vector3.forward * forwardValue;

            }


        }


        nma.SetDestination(destination);


        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs.waitUntilStop(gameObject));
        yield return Timing.WaitUntilDone(handler);

        nma.speed = originalSpeed;
    }

    


}
