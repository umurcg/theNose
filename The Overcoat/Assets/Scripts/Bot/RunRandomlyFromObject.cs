using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CinemaDirector;

//This scripts makes functionality of running from a game object.
//Normally owner of scripts randomly moves in a circle.
//If chaser get near of owner, 
//Scripts creates a new circle in a box.
//After that change of circle character's speed becomes 5 for 4 seconds.


public class RunRandomlyFromObject: MonoBehaviour {

    public GameObject road;
    public GameObject chaser;
    
    public float randomCircleRadius;

    //center to center
    public float minDist;
    public GameObject box;
 
    public float tolerance = 0.001f;
   
    public float waitForRunFromCircle=2f;
    float timer2;

    Vector3 center;

    //This tag is for objects that will finish run. When current circle includes specific number of this object  cs will be called.
    public string finishObjectTag;
    public int maxNumber;

    NavMeshAgent nma;
    float timer;
    Vector3 lastPos;

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


    protected bool checkIsMoving()
    {
        if (lastPos == null)
        {
            lastPos = transform.position;
            return false;
        }
        else
        {
            if (Vector3.Distance(transform.position, lastPos) > tolerance)
            {
                lastPos = transform.position;
                return true;
            }
            else
            {
                return false;
            }
        }

    }


    // Update is called once per frame
    public void Update () {
        moving = checkIsMoving();
        //print(moving);
        //if (!moving&&!stuck)
        //{
        //    stuck = true;
        //    stuckTimer = stuckTime;
        //}


        //if (stuckTimer > 0&&stuck)
        //{

        //    //print(stuckTimer);
        //    stuckTimer -= Time.deltaTime;
        //    moving = checkIsMoving();

        //    if (moving)
        //    {
        //        stuckTimer = 0;
        //        stuck = false;
        //    }


        //if (stuckTimer <= 0)
        //{

        //        CinemaDirector.PausePlayCS ppcs = GetComponent<CinemaDirector.PausePlayCS>();
        //        if (ppcs)
        //            ppcs.Play();

        //}
        //}

        if (IsAreaIncludesObject.howManyObjectsInSphere(transform.position, randomCircleRadius, finishObjectTag) == maxNumber)
        {

            //CinemaDirector.PausePlayCS ppcs = GetComponent<CinemaDirector.PausePlayCS>();
            //if (ppcs)
            //    ppcs.Play();
            print("catched");
            this.enabled = false;
        }

        if (Vector3.Distance(transform.position, chaser.transform.position) < minDist && timer2<=0)
        {
            

            center = getRandomPositionInBox(box); /*GetARandomPositionInTorus(chaser.transform.position);*/
            center = new Vector3(center.x, transform.position.y, center.z);

            nma.Stop();
            nma.Resume();
            nma.speed = 8f;
            nma.SetDestination(center);
            //WalkAroundCenterIfNotWalking();


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

        //WalkAroundCenterIfNotWalking();



        lastPos = transform.position;
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
     
             
                avaible = !IsAreaIncludesObject.isIncludeInSphere(position, randomCircleRadius,"Player")&&!AreThereAnyObjectInPath.AreThere(transform,position,5f,"Player");


                if (avaible)
                {
           
                nma.destination = position;
                }
            }
            }
    }


    static Vector3 getRandomPositionInBox(GameObject box)
    {
        //BoxCollider bc = box.GetComponent<BoxCollider>;
        float width = box.transform.lossyScale.x;
        float depth = box.transform.lossyScale.z;
        float height = box.transform.lossyScale.y;
        Vector3 pos = box.transform.position;
        print(width+" "+ depth);
        float x = Random.Range(pos.x - width / 2, pos.x + width / 2);
        float z = Random.Range(pos.z - depth / 2, pos.z + depth / 2);
        float y = Random.Range(pos.y - height / 2, pos.y + height / 2);

        return (new Vector3(x, y, z));
                        
    }

    //debug
    //This method for drawing path of way poişnts.


    //Vector3 GetARandomPositionInTorus(Vector3 center)
    //{

    //    Vector3 dest = transform.position;

    //    while (Vector3.Distance(dest, transform.position) < newCirclePositionMinDistance)
    //    {
    //        Vector3 randomPosition = Random.insideUnitSphere * newCirclePositionMaxDistance;
    //        randomPosition = new Vector3(randomPosition.x, transform.position.y, randomPosition.z);
    //        dest = randomPosition + center;
    //    }

       
    //    return dest;

    // }

    protected Vector3 GetARandomPositionInAroundPosition(Vector3 position)
    {

        Vector3 randomPosition= Random.insideUnitSphere;
        Vector3 dest = position + randomPosition * randomCircleRadius;
        return dest;

    }





}
