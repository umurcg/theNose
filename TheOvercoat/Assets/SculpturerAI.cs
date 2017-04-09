using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class SculpturerAI : GameController {

    public float runDistance = 10f;
    public float navMeshSampleRaidus = 1f;

    public float timeBetweenRandomMovements = 5f;
    public float randomMovementRadius = 30;
    public float randomWalkSpeed = 3f;
    public float runFromPlayerSpeed = 5f;
    public float timeBetweenSubtitles = 10f;
    public float subtDuration=3f;
    float subtTimer;
    float timer;
    NavMeshAgent nma;

    Vector3 prevPos;
    float tolerance = 0.01f;
    // Use this for initialization

    SubtitleController sub;
	public override void Start () {
        base.Start();
        nma = GetComponent<NavMeshAgent>();
        timer = timeBetweenRandomMovements;
        prevPos = transform.position;
        sub= GetComponent<SubtitleController>();
        subtTimer = timeBetweenSubtitles;

	}
	
	// Update is called once per frame
	void Update () {

        subtTimer -= Time.deltaTime;
        if (subtTimer <= 0)
        {
            Timing.RunCoroutine(randomSubt());
            subtTimer = timeBetweenSubtitles;
        }


        if (Vector3.Distance(transform.position, prevPos) < 0.01f)
        {
            //Debug.Log("Not walking");
            //If not running then look at player
            Vector3 lookDirection = player.transform.position-transform.position;
            lookDirection.y = transform.position.y;
            Quaternion wantedRot= Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation= Quaternion.Slerp(transform.rotation, wantedRot, Time.deltaTime);



            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                moveRandomly();
            }
        }
        else
        {
            timer = timeBetweenRandomMovements;
        }
        
        
        prevPos = transform.position;

	}

    void moveRandomly()
    {
        nma.Resume();
        nma.speed = randomWalkSpeed;
        Vector3 randomDirection = Random.insideUnitSphere * randomMovementRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, randomMovementRadius, 1);
        Vector3 finalPosition = hit.position;
        nma.SetDestination(finalPosition);
        timer = timeBetweenRandomMovements;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            runFromPlayer();
        }
    }

    void runFromPlayer()
    {
        Debug.Log("run");

        nma.Resume();
        nma.speed = runFromPlayerSpeed;
        Vector3 destination = transform.position + player.transform.forward * runDistance;
        //Sample dest
        NavMeshHit nmh;
        if (NavMesh.SamplePosition(destination, out nmh, navMeshSampleRaidus, nma.areaMask))
        {
            //If dest is almost in navmesh leave it like that
            //Debug.Log("First attempt");
        }
        else
        {
            //Try to run in z direction
            destination = transform.position + transform.right * runDistance;
            if (NavMesh.SamplePosition(destination, out nmh, navMeshSampleRaidus, nma.areaMask))
            {
                //Debug.Log("Second attempt");
                //If dest is almost in navmesh leave it like that
            }
            else {

                destination = transform.position - transform.right * runDistance;
                if (NavMesh.SamplePosition(destination, out nmh, navMeshSampleRaidus, nma.areaMask))
                {
                    //Debug.Log("Second attempt");
                    //If dest is almost in navmesh leave it like that
                }

                else
                 {
                    //Try to run in x direction if z is not valid, I hope it is valid else fuck it i am out.
                    //Debug.Log("Third attempt");
                    destination = transform.position - Vector3.forward * runDistance;

                }
            }

        }

        nma.SetDestination(destination);

        timer = timeBetweenRandomMovements;

    }

    IEnumerator<float> randomSubt()
    {
        Debug.Log("Random subt");
        string randomSubt = sub.subtitleTexts[Random.Range(0, sub.subtitleTexts.Length)];
        subtitle.text = randomSubt;
        float lsubtTimer = subtDuration;
        while (lsubtTimer > 0)
        {
            lsubtTimer -= Time.deltaTime;
            yield return 0;
        }

        subtitle.text = "";
        Debug.Log("Finishing subt");

        yield break;
    }
}
