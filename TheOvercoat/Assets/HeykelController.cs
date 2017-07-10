using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class HeykelController : MonoBehaviour {

    public GameObject rock;
    //HandBone will be used ads parent of rock 
    public GameObject handBone;
    public float mintimeBetweenShots=5f;
    public float maxtimeBetweenShots = 7f;
    public float minShootAngle = 10f;
    public float maxShootAngle = 60f;
    public int damageAmount = 5;
    //Trigger name
    public string shootAnimationName = "Throw";
    //This boolean prevents update function call shoot while it is alrrady shooting
    bool shooting = false;
    ////This boolean is reqiered for animation callback. When this triggeres animation of shoot it waits a call from animation from right time to complete shoot. So in otherWords animation will make this boolean true
    ////And this script will continue to shoot
    //bool continueToShoot = false;

    GameObject player;
    SculpturerGameController gameController;

    GameObject master;
    public float capsuleRadius=1f;
    bool dontShootMaster = true;

    public   GameObject brokenSculpt;
    public GameObject particleObj;

    float timer;


    Animator anim;

    private void Awake()
    {
        //Register to parent game controller
        gameController = transform.parent.gameObject.GetComponent<SculpturerGameController>();
        gameController.registerHeykel(this);

        master = gameController.sculpturer;
        anim = GetComponent<Animator>();

    }

    // Use this for initialization
    void Start () {

        if (minShootAngle > maxShootAngle) minShootAngle = maxShootAngle;

        player = CharGameController.getActiveCharacter();
        timer = Random.Range(mintimeBetweenShots,maxtimeBetweenShots);

	}
	
	// Update is called once per frame
	void Update () {

        timer -= Time.deltaTime;
        if (timer <= 0 && !shooting)
        {
            //Debug.Log("shoot player");
            Timing.RunCoroutine(shootPlayer());
   
        }

        Quaternion lookRot = Quaternion.LookRotation((new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z)-transform.position),Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime*2);

        

    }


    IEnumerator<float> shootPlayer()
    {
        shooting = true;

        GameObject spawnedRock = spawnRock();
        Rigidbody rb = spawnedRock.GetComponent<Rigidbody>();

  
        //Random shoot angle
        float shootAngle = Random.Range(minShootAngle, maxShootAngle);

        // Selected angle in radians
        float angle = shootAngle * Mathf.Deg2Rad;

        //// Positions of this object and the target on the same plane
        Vector3 planarTarget = planarPos(player.transform.position);
        Vector3 planarPostion = planarPos(spawnedRock.transform.position);

        //Check master and if he is on my shoot direction Dont shoot master!
        Vector3 planarMaster = planarPos(master.transform.position);
        float AA = Vector3.Distance(planarTarget, planarPostion);
        float B = capsuleRadius;
        float BB = B * 2;

        //Debug.Log((Vector3.Distance(planarMaster, planarTarget)
        //    + Vector3.Distance(planarMaster, planarPostion)) + " " + (AA + BB / 2));
        if (dontShootMaster && Vector3.Distance(planarMaster, planarTarget)
            + Vector3.Distance(planarMaster, planarPostion) < (AA + BB / 2))
        {
            //Debug.Log("Dont shoot master!");
            //Master is front of me. So dont shoot!
            shooting = false;
            Destroy(spawnedRock);
            yield break; ;
        }

        anim.SetTrigger(shootAnimationName);

        yield return 0;

        //Debug.Log(anim.GetCurrentAnimatorStateInfo(1).IsName("Throw"));


        //float animFrame = 50;
        

        while (!anim.GetCurrentAnimatorStateInfo(1).IsName("Throw") || anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.7f)
        {
            yield return 0;
        }



        //// Distance along the y axis between objects
        float yOffset = spawnedRock.transform.position.y- player.transform.position.y;
        
        Vector3 finalVelocity = calculateVelocity(angle, planarTarget, planarPostion,yOffset);

        //float angle = 0.5f * Mathf.Asin(Physics.gravity.magnitude * distance / (Mathf.Pow(shootSpeed, 2)));
        //Debug.Log("Angle is " + angle*Mathf.Rad2Deg);
        //Vector3 shootDirection = (planarTarget - planarPostion).normalized * Mathf.Cos(angle) + Vector3.up * Mathf.Sin(angle);
        //Vector3 finalVelocity = shootDirection * shootSpeed;

        spawnedRock.transform.parent = null;

        //Make non kinematic before applying force
        rb.isKinematic = false;
        yield return 0;

        rb.AddForce(finalVelocity * rb.mass, ForceMode.Impulse);

        timer = Random.Range(mintimeBetweenShots, maxtimeBetweenShots);
        shooting = false;

        yield break;
    }

    //This function calculate required velocity for projectile between two points with given shoot angle.
    private Vector3 calculateVelocity(float angle, Vector3 planarTarget, Vector3 planarPostion, float yOffset)
    {
        //// Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);


        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * Physics.gravity.magnitude * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));


        // From
        //float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);
        // To (added "* (p.x > transform.position.x ? 1 : -1)")
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (player.transform.position.x > transform.position.x ? 1 : -1);

        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
        return finalVelocity;
    }


    public GameObject spawnRock()
    {
        GameObject spawnedRock = Instantiate(rock) as GameObject;
        spawnedRock.transform.parent = handBone.transform;
        spawnedRock.transform.localPosition = Vector3.zero;
        RockScript rs = spawnedRock.GetComponent<RockScript>();
        rs.creator = gameObject;
        rs.reciever = gameController.gameObject;

        //Debug.Log("Spawned");

        return spawnedRock;
    }

    //public void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Hit");
    //    RockScript rs = collision.gameObject.GetComponent<RockScript>();
    //    if (collision.transform.tag == "Fire" && rs!=null && rs.creator!=gameObject)
    //    {
    //        Explode();
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        //When rock exits trigger make its first collider as non trigger
        RockScript rs = other.gameObject.GetComponent<RockScript>();
        if (rs && rs.creator == gameObject)
        {
            SphereCollider rb = other.gameObject.GetComponents<SphereCollider>()[0];
            rb.isTrigger = false;
        }
    }

    public void Explode()
    {
        Debug.Log("I died " + gameObject.name);
        GameObject spawnedObj=Instantiate(brokenSculpt) as GameObject;
        spawnedObj.transform.localScale = transform.localScale;
        spawnedObj.transform.position = transform.position;
        spawnedObj.transform.rotation = transform.rotation;

        
        GameObject pso = Instantiate(particleObj) as GameObject;

        pso.SetActive(true);
        pso.transform.position = transform.position;


        ParticleSystem[] pss = pso.GetComponentsInChildren<ParticleSystem>();
        foreach( ParticleSystem ps in pss)
        {
            ps.Play();
        }
        


        gameController.removeHeykel(this);
        Destroy(gameObject);
    }

    public void shootMaster(bool shoot)
    {
        dontShootMaster = !shoot;
    }


    Vector3 planarPos(Vector3 pos) { return new Vector3(pos.x, 0, pos.z); }

    //public void setContinueShootTrue()
    //{
    //    continueToShoot = true;
    ////}

}
