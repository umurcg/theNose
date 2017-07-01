using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;


//This script controls drunk man in drunkmangame
//There is two status, player is seen and non seen
//If player is beign seen, drunk man tries to shoot player with bottles
//If player is not seen that drunk man explore area randomly with raycasting his forward direction untile he sees player.
//Also if player shoots owner or shoots somewhere in walk area, status will be changed to player is seen
//status changes to non seen from seen when player get out from walk area trigger;
public class DrunkManAI : MonoBehaviour {

    public bool playerIsSeen=false;
    public GameObject walkArea;
    public GameObject bottlePrefab;
    public GameObject handPos;
    public float maxAngle = 70;
    public float minAngle = 30;
    public DrunkManGameController dmgc;

    public string shootAnimationName;

    public float maxDistWhenCatchPlayer = 10f;

    GameObject player;
    CanSeeYou cya;

    public float timeBetweenTalks;
    Timer talkTimer;

    public float timeBetweenWalks;
    Timer walkTimer;

    public float timeShoots;
    Timer shootTimer;

    Animator anim;


    SubtitleCaller sc;
    Vector3 prevPos;

    NavMeshAgent agent;


    EnterTrigger et;

    // Use this for initialization
    void Start () {
        cya = GetComponent<CanSeeYou>();
        sc = GetComponent<SubtitleCaller>();

        et = walkArea.GetComponent<EnterTrigger>();

        talkTimer = new Timer(timeBetweenTalks);
        walkTimer = new Timer(timeBetweenWalks);
        shootTimer = new Timer(timeShoots);

        prevPos = transform.position;

        agent = GetComponent<NavMeshAgent>();

        player = CharGameController.getActiveCharacter();

        anim = GetComponent<Animator>();
        

	}
	
	// Update is called once per frame
	void Update () {


        if (playerIsSeen)
        {

            shootPlayer();
        }
        else
        {

            searchPlayer();
        }

        prevPos = transform.position;
	}
    

    void searchPlayer()
    {

        if (talkTimer.ticTac(Time.deltaTime))
        {
            sc.callRandomSubtTime(0);
        }

        //Debug.Log(Vector3.Distance(transform.position, prevPos));

        if (Vector3.Distance(transform.position,prevPos)<=0.001f)
        {
            if (walkTimer.ticTac(Time.deltaTime))
            {
                //Debug.Log("Generating pos");
                agent.SetDestination(generatePos());
            }
        }
     

    }


    void shootPlayer()
    {
        Quaternion aimRot=Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, aimRot,Time.deltaTime);

        if (Vector3.Distance(player.transform.position, transform.position) > maxDistWhenCatchPlayer)
        {
            agent.SetDestination(player.transform.position);

        }

        if (shootTimer.ticTac(Time.deltaTime))
        {
            Timing.RunCoroutine(shoot(player.transform.position));
        }

        
    }


    Vector3 generatePos()
    {
        Vector3 castedPos;
        Vckrs.findNearestPositionOnNavMesh(Vckrs.generateRandomPositionInBox(walkArea),agent.areaMask,100, out castedPos);
        return castedPos;
    }


    public void sawPlayer()
    {
        if (!et.isPlayerInside()) return;

        Debug.Log("Saw player");
        cya.enabled = false;
        playerIsSeen = true;
    }

    public void lostPlayer()
    {
        Debug.Log("Lost player");
        cya.enabled = true;
        playerIsSeen = false;
    }

    IEnumerator<float> shoot(Vector3 pos)
    {
        GameObject bottle = Instantiate(bottlePrefab);
        bottle.transform.parent = handPos.transform;
        bottle.transform.localPosition = Vector3.zero;
        


                       
        Rigidbody rb = bottle.GetComponent<Rigidbody>();


        //Random shoot angle
        float shootAngle = Random.Range(minAngle, maxAngle);
        //float shootAngle = (maxAngle - minAngle) * (1 - (velocity / 100)) + minAngle;

        //Debug.Log(shootAngle);

        // Selected angle in radians
        float angle = shootAngle * Mathf.Deg2Rad;

        //// Positions of this object and the target on the same plane
        Vector3 planarTarget = planarPos(pos);
        Vector3 planarPostion = planarPos(bottle.transform.position);



        //cml.enabled = false;

        anim.SetTrigger(shootAnimationName);

        yield return 0;

        //Debug.Log(anim.GetCurrentAnimatorStateInfo(3).IsName("Throw"));


        //float animFrame = 50;

        while (!anim.GetCurrentAnimatorStateInfo(3).IsName("Throw") || anim.GetCurrentAnimatorStateInfo(3).normalizedTime < 0.7f)
        {
            yield return 0;
        }


        //// Distance along the y axis between objects
        float yOffset = bottle.transform.position.y - pos.y;

        Vector3 finalVelocity = calculateVelocity(angle, planarTarget, bottle, yOffset);

        //float angle = 0.5f * Mathf.Asin(Physics.gravity.magnitude * distance / (Mathf.Pow(shootSpeed, 2)));
        //Debug.Log("Angle is " + angle*Mathf.Rad2Deg);
        //Vector3 shootDirection = (planarTarget - planarPostion).normalized * Mathf.Cos(angle) + Vector3.up * Mathf.Sin(angle);
        //Vector3 finalVelocity = shootDirection * shootSpeed;

        bottle.transform.parent = null;

        bottle.tag = "Untagged";

        //Make non kinematic before applying force
        rb.isKinematic = false;
        rb.useGravity = true;

        RockScript rs = bottle.GetComponent<RockScript>();
        rs.creator = gameObject;
        rs.enabled = true;

        rs.reciever = dmgc.gameObject;

        yield return 0;

        rb.AddForce(finalVelocity * rb.mass, ForceMode.Impulse);

        //bottle.GetComponent<Collider>().isTrigger = false;

        //cml.enabled = true;

        yield break;

    }

    Vector3 planarPos(Vector3 pos) { return new Vector3(pos.x, 0, pos.z); }


    private Vector3 calculateVelocity(float angle, Vector3 pos, GameObject bottle, float yOffset)
    {
        Vector3 planarTarget = planarPos(pos);
        Vector3 planarPostion = planarPos(bottle.transform.position);

        //// Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);


        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * Physics.gravity.magnitude * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));


        // From
        //float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);
        // To (added "* (p.x > transform.position.x ? 1 : -1)")
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (pos.x > bottle.transform.position.x ? 1 : -1);

        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
        return finalVelocity;
    }

}
