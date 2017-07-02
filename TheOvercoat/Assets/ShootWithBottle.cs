using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class ShootWithBottle : MonoBehaviour {

    //public GameObject bottle;
    public string shootModeAxis;
    public string shootAnimationName="Shoot";
    GameObject handPosition;
    Camera cam;
    public LayerMask areaMask;
    Animator anim;

    public float minAngle = 10;
    public float maxAngle = 80;
    float velocity = 0;
    public float velocityChangeSpeed = 1f;
    bool velocityIncresing = true;

    GameObject player;
    CharacterMouseLook cml;
    public DrunkManGameController dmgc;

	// Use this for initialization
	void Start () {
        handPosition = CharGameController.getHand(CharGameController.hand.LeftHand);
        cam = CharGameController.getCamera().GetComponent<Camera>();

        player = CharGameController.getActiveCharacter();

        cml =player.GetComponent<CharacterMouseLook>();

        anim=player.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {


        if (velocityIncresing)
        {
            velocity += Time.deltaTime * velocityChangeSpeed;
            if (velocity > 100)
            {
                velocity = 100;
                velocityIncresing = false;
            }
        }
        else
        {

            velocity -= Time.deltaTime * velocityChangeSpeed;
            if (velocity <0)
            {
                velocity = 0;
                velocityIncresing = true;
            }

        }

        dmgc.setVelocityUI(velocity / 100);

        //Debug.Log(velocity);

        if(Input.GetButtonDown(shootModeAxis) && handPosition.transform.childCount > 0)
        {
            RaycastHit hit;
            Ray r = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(r,out hit, 1000f,~areaMask))
            {
                //if(hit.transform.tag=="Floor"|| hit.transform.ta)
                //{
                   Timing.RunCoroutine(shoot(hit.point));
                //}
            }


            //shoot(pos);
        }


	}

    IEnumerator<float> shoot(Vector3 pos)
    {
        GameObject bottle = handPosition.transform.GetChild(0).gameObject;
        Rigidbody rb = bottle.GetComponent<Rigidbody>();


        //Random shoot angle
        //float shootAngle = Random.Range(minAngle, maxAngle);
        float shootAngle = (maxAngle - minAngle) * (1-(velocity / 100)) + minAngle;

        //Debug.Log(shootAngle);

        // Selected angle in radians
        float angle = shootAngle * Mathf.Deg2Rad;

        //// Positions of this object and the target on the same plane
        Vector3 planarTarget = planarPos(pos);
        Vector3 planarPostion = planarPos(bottle.transform.position);

        ////Check master and if he is on my shoot direction Dont shoot master!
        //Vector3 planarMaster = planarPos(master.transform.position);
        //float AA = Vector3.Distance(planarTarget, planarPostion);
        //float B = capsuleRadius;
        //float BB = B * 2;

        //Debug.Log((Vector3.Distance(planarMaster, planarTarget)
        //    + Vector3.Distance(planarMaster, planarPostion)) + " " + (AA + BB / 2));
        //if (dontShootMaster && Vector3.Distance(planarMaster, planarTarget)
        //    + Vector3.Distance(planarMaster, planarPostion) < (AA + BB / 2))
        //{
        //    //Debug.Log("Dont shoot master!");
        //    //Master is front of me. So dont shoot!
        //    shooting = false;
        //    Destroy(spawnedRock);
        //    yield break; ;
        //}


        cml.enabled = false;

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

        //Make non kinematic before applying force
        rb.isKinematic = false;
        rb.useGravity = true;

        bottle.GetComponent<RockScript>().enabled = true;

        yield return 0;

        rb.AddForce(finalVelocity * rb.mass, ForceMode.Impulse);

        //bottle.GetComponent<Collider>().isTrigger = false;

        cml.enabled = true;

        yield break;

    }

    Vector3 planarPos(Vector3 pos) { return new Vector3(pos.x, 0, pos.z); }


    private Vector3 calculateVelocity(float angle, Vector3 pos, GameObject bottle ,float yOffset)
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
