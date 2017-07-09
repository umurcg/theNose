using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class SculpturerAI : GameController, IClickAction {

    public float runDistance = 10f;
    public float navMeshSampleRaidus = 1f;

    public float timeBetweenRandomMovements = 5f;
    public float randomMovementRadius = 30;
    public float randomWalkSpeed = 3f;
    public float runFromPlayerSpeed = 5f;
    public float timeBetweenSubtitles = 10f;
    public float subtDuration=3f;

    public GameObject kova;

    Animator anim;

    float subtTimer;
    float timer;
    NavMeshAgent nma;

    Vector3 prevPos;
    float tolerance = 0.01f;

    public GameObject alci;
    bool oneHeykelIsLeft = false;
    bool shooting = false;
    public float timeBetweenShots = 5f;
    float shotTimer;


    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    // Use this for initialization

    SubtitleController sub;
	public override void Start () {
        base.Start();
        nma = GetComponent<NavMeshAgent>();
        timer = timeBetweenRandomMovements;
        prevPos = transform.position;
        sub= GetComponent<SubtitleController>();
        subtTimer = timeBetweenSubtitles;
        shotTimer = timeBetweenShots;

        //Timing.RunCoroutine(shootAlci());

	}
	
	// Update is called once per frame
	void Update () {


        if (oneHeykelIsLeft)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0 && !shooting) Timing.RunCoroutine( shootAlci());
        }

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
            if (timer <= 0 && !shooting)
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
        NavMesh.SamplePosition(randomDirection, out hit, randomMovementRadius,nma.areaMask);
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
        //string randomSubt = sub.subtitleTexts[Random.Range(0, sub.subtitleTexts.Length)];
        //subtitle.text = randomSubt;

        sc.callRandomSubtTime(0);

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

    IEnumerator<float> shootAlci()
    {
        nma.Stop();
        shooting = true;

        handlerHolder = Timing.RunCoroutine(Vckrs._lookTo(gameObject, player, 1f));
        yield return Timing.WaitUntilDone(handlerHolder);

        kova.SetActive(true);

        anim.SetTrigger("Spill");


        //while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Spill") || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        //{
        //    Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsName("Spill"));
        //    Debug.Log(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //    yield return 0;
        //}
        Timing.WaitForSeconds(1f);
        
        

        alci.transform.position = transform.position;
        alci.transform.rotation = transform.rotation;

        ParticleSystem ps = alci.GetComponent<ParticleSystem>();
        ps.Play();

        while (ps.isPlaying) yield return 0;


        shooting = false;
        shotTimer = timeBetweenShots;
        kova.SetActive(false);

        yield break;
    }

    public void shootWithAlci(bool shoot)
    {
        Debug.Log("Shoot with alci");
        oneHeykelIsLeft = shoot;
        GetComponent<SphereCollider>().radius = 2;
    }

    public override void Action()
    {
        //base.Action();

        transform.parent.gameObject.GetComponent<SculpturerGameController>().outerSpeech();
        transform.tag = "Untagged";
    }

    


}
