using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This scripts makes functionality of running from a game object.
//Normally owner of scripts randomly moves in a circle.
//If chaser get near of owner, 
//Scripts creates a new circle far from chaser with amount between newCircleMinDist and MaxDist randomly.
//After that change of circle character's speed becomes 5 for 4 seconds.


public class RunRandomlyFromObject :RandomWalkBot {

    public GameObject road;
    public GameObject chaser;

    public float randomCircleRadius;
    //center to center
    public float minDist;
    public float newCirclePositionMinDistance;
    public float newCirclePositionMaxDistance;


    public float waitForRunFromCircle=2f;
    float timer2;

    Vector3 center;

    public bool stuck = false;
    float stuckTimer;


    bool moving = false;


    //if (timer <= 0)
    //{
    //    Instantiate(debug, GetARandomPositionInTorus(transform.position), transform.rotation);
    //    timer = 0.1f;
    //} else
    //{
    //    timer -= Time.deltaTime;
    //}


    void Start()
    {
         nma= GetComponent<NavMeshAgent>();
        center = transform.position;
  
    }


	
	// Update is called once per frame
	public override void Update () {
        moving = checkIsMoving();

        if (!moving&&!stuck)
        {
            stuck = true;
            stuckTimer = 2.5f;
        }
        if (stuckTimer > 0)
        {
            stuckTimer -= Time.deltaTime;
            if (stuckTimer <= 0)
            {
                stuckTimer = 0;
                stuck = false;
            }
        }


        
        if (Vector3.Distance(transform.position, chaser.transform.position) < minDist && timer2<=0)
        {
            
            center= GetARandomPositionInTorus(chaser.transform.position);

            nma.Stop();
            nma.Resume();
            nma.speed = 8f;
            WalkAroundCenterIfNotWalking();

            timer2 = waitForRunFromCircle;
        }

        if (timer2 > 0)
        {
            timer2 -= Time.deltaTime;
            if (timer2 <= 0)
            {
                timer2 = 0;
                nma.speed = 3;
            }
        }

        if (timer <= 0)
        {
         
            timer = 0;
            WalkAroundCenterIfNotWalking();
        }
        else
        {
            timer -= Time.deltaTime;
        }



    }


    void WalkAroundCenterIfNotWalking()
    {
        Vector3 position;
        if (/*checkIsMoving()*/ !moving)
        {
            if (nma.isOnNavMesh)
            {
                position = GetARandomPositionInAroundPosition(center);

                bool avaible = true;
                IsAreaIncludesObject iaio = GetComponent<IsAreaIncludesObject>();
                AreThereAnyObjectInPath ataop = GetComponent<AreThereAnyObjectInPath>();

                if (/*iaio != null &&*/ ataop != null)
                    avaible = !iaio.isInclude(position, randomCircleRadius)&&!ataop.AreThereAny(transform,position);


                if (avaible)
                {
           
                nma.destination = position;
                timer = waitBetweenWalks;
                }
            }
            }
    }


    //debug
    //This method for drawing path of way poişnts.


    Vector3 GetARandomPositionInTorus(Vector3 center)
    {

        Vector3 dest = transform.position;

        while (Vector3.Distance(dest, transform.position) < newCirclePositionMinDistance)
        {
            Vector3 randomPosition = Random.insideUnitSphere * newCirclePositionMaxDistance;
            randomPosition = new Vector3(randomPosition.x, transform.position.y, randomPosition.z);
            dest = randomPosition + center;
        }

       
        return dest;

     }

    protected Vector3 GetARandomPositionInAroundPosition(Vector3 position)
    {

        Vector3 randomPosition= Random.insideUnitSphere;
        Vector3 dest = position + randomPosition * randomCircleRadius;
        return dest;

    }





}
