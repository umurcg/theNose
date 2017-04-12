using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeykelController : MonoBehaviour {

    public GameObject rock;
    public float mintimeBetweenShots=5f;
    public float maxtimeBetweenShots = 7f;
    public float minShootAngle = 10f;
    public float maxShootAngle = 60f;
    public int damageAmount = 5;
    GameObject player;
    SculpturerGameController gameController;

    GameObject master;
    public float capsuleRadius=1f;
    bool dontShootMaster = true;

    float timer;
    

    private void Awake()
    {
        //Register to parent game controller
        gameController = transform.parent.gameObject.GetComponent<SculpturerGameController>();
        gameController.registerHeykel(this);

        master = gameController.sculpturer;

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
        if (timer <= 0)
        {
            shootPlayer();
   
        }

        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

    }


    void shootPlayer()
    {

        Vector3 spawnPosition = transform.position + transform.forward; 

        float shootAngle = Random.Range(minShootAngle, maxShootAngle);

        Vector3 p = player.transform.position;
        float gravity = Physics.gravity.magnitude;
        //// Selected angle in radians
        float angle = shootAngle * Mathf.Deg2Rad;

        //// Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(spawnPosition.x, 0, spawnPosition.z);

        //Check master and if he is on my shoot direction Dont shoot master!
        Vector3 planarMaster = master.transform.position;
        planarMaster.y = 0;
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
            return;
        }

        GameObject spawnedRock = spawnRock();
        Rigidbody rb = spawnedRock.GetComponent<Rigidbody>();

        //// Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        //// Distance along the y axis between objects
        float yOffset = transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // From
        //float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);
        // To (added "* (p.x > transform.position.x ? 1 : -1)")
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (p.x > transform.position.x ? 1 : -1);

        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        //float angle = 0.5f * Mathf.Asin(Physics.gravity.magnitude * distance / (Mathf.Pow(shootSpeed, 2)));
        //Debug.Log("Angle is " + angle*Mathf.Rad2Deg);
        //Vector3 shootDirection = (planarTarget - planarPostion).normalized * Mathf.Cos(angle) + Vector3.up * Mathf.Sin(angle);
        //Vector3 finalVelocity = shootDirection * shootSpeed;

        rb.AddForce(finalVelocity* rb.mass, ForceMode.Impulse);

        timer = Random.Range(mintimeBetweenShots, maxtimeBetweenShots);

    }

    public GameObject spawnRock()
    {
        GameObject spawnedRock = Instantiate(rock) as GameObject;
        spawnedRock.transform.position = transform.position + transform.forward;
        RockScript rs = spawnedRock.GetComponent<RockScript>();
        rs.creator = gameObject;
        rs.reciever = gameController.gameObject;

        return spawnedRock;
    }

    public void OnCollisionEnter(Collision collision)
    {
        RockScript rs = collision.gameObject.GetComponent<RockScript>();
        if (collision.transform.tag == "Fire" && rs!=null && rs.creator!=gameObject)
        {
            Explode();
        }
    }

    public void Explode()
    {
        Debug.Log("I died " + gameObject.name);
        gameController.removeHeykel(this);
        Destroy(gameObject);
    }

    public void shootMaster(bool shoot)
    {
        dontShootMaster = !shoot;
    }

}
