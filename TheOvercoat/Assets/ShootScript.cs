using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A clean script that enables user to collect and shoot with owner object.
[RequireComponent (typeof(Rigidbody))]
public class ShootScript : MonoBehaviour, IClickAction {

    static GameObject collectedObject;
    public CharGameController.hand Hand;
    public float scale = 1f;

    public string shootButton;



    public void Action()
    {
        if (collectedObject != null) return;

        transform.parent = CharGameController.getHand(Hand).transform;
        transform.localScale = scale * Vector3.one;
        collectedObject = gameObject;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (collectedObject == gameObject)
        {

            if (Input.GetButtonDown(shootButton))
            {
                //shoot
            }

        }
        

	}


    //void shoot()
    //{

    //}


    //IEnumerator<float> shoot(Vector3 pos)
    //{

    //    Rigidbody rb = gameObject.GetComponent<Rigidbody>();


    //    //Random shoot angle
    //    float shootAngle =/* Random.Range(minAngle, maxAngle);*/ 45f;
    //    //float shootAngle = (maxAngle - minAngle) * (1 - (velocity / 100)) + minAngle;

    //    //Debug.Log(shootAngle);

    //    // Selected angle in radians
    //    float angle = shootAngle * Mathf.Deg2Rad;

    //    //// Positions of this object and the target on the same plane
    //    Vector3 planarTarget = planarPos(pos);
    //    Vector3 planarPostion = planarPos(bottle.transform.position);



    //    //cml.enabled = false;

    //    anim.SetTrigger(shootAnimationName);

    //    yield return 0;

    //    //Debug.Log(anim.GetCurrentAnimatorStateInfo(3).IsName("Throw"));


    //    //float animFrame = 50;

    //    while (!anim.GetCurrentAnimatorStateInfo(3).IsName("Throw") || anim.GetCurrentAnimatorStateInfo(3).normalizedTime < 0.7f)
    //    {
    //        yield return 0;
    //    }


    //    //// Distance along the y axis between objects
    //    float yOffset = bottle.transform.position.y - pos.y;

    //    Vector3 finalVelocity = calculateVelocity(angle, planarTarget, bottle, yOffset);

    //    //float angle = 0.5f * Mathf.Asin(Physics.gravity.magnitude * distance / (Mathf.Pow(shootSpeed, 2)));
    //    //Debug.Log("Angle is " + angle*Mathf.Rad2Deg);
    //    //Vector3 shootDirection = (planarTarget - planarPostion).normalized * Mathf.Cos(angle) + Vector3.up * Mathf.Sin(angle);
    //    //Vector3 finalVelocity = shootDirection * shootSpeed;

    //    bottle.transform.parent = null;

    //    bottle.tag = "Untagged";

    //    //Make non kinematic before applying force
    //    rb.isKinematic = false;
    //    rb.useGravity = true;

    //    RockScript rs = bottle.GetComponent<RockScript>();
    //    rs.creator = gameObject;
    //    rs.enabled = true;

    //    rs.reciever = dmgc.gameObject;

    //    yield return 0;

    //    rb.AddForce(finalVelocity * rb.mass, ForceMode.Impulse);

    //    //bottle.GetComponent<Collider>().isTrigger = false;

    //    //cml.enabled = true;

    //    yield break;

    //}

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
