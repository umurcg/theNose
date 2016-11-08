using UnityEngine;
using System.Collections;

//It moves game object between way points.
//It has finish function special for players.

public class Pacing : MonoBehaviour {
    public  enum targetType {Vector3,GameObject }
    public targetType TargetType;
    public float speed = 2f;
    public bool makeFirstPositionSubjectPosition = true;
    float originalSpeed;
    PlayerComponentController pcc;
    NavMeshAgent nma;
    public Vector3[] position;
    public GameObject[] goPositions;
    int index = 0;
    Vector3 prevPosition;
    float tolerance = 0.01f;
    float timer;

	// Use this for initialization
	void Start () {
        if(makeFirstPositionSubjectPosition)
        if (TargetType == targetType.GameObject)
        {

            goPositions[0].transform.position = transform.position;
        }else
        {
            position[0] = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timer>0)
        timer -= Time.deltaTime;
        if(!pcc)
        pcc = GetComponent<PlayerComponentController>();
        if (pcc)
            pcc.StopToWalk();
        if (!nma)
        {
            nma = GetComponent<NavMeshAgent>();
            originalSpeed = nma.speed;
            nma.speed = speed;
        }
        //    print(isWalking());


        if (!isWalking())
        {
    
            newDestionation();
        }

        prevPosition = transform.position;

    }

    void newDestionation()
    {
       // print("newDest");
        if (TargetType == targetType.GameObject)
        {

            nma.SetDestination(goPositions[index].transform.position);
            if (index + 1 >= goPositions.Length)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }
        else
        {



            nma.SetDestination(position[index]);
            if (index + 1 >= position.Length)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }

        timer = 0.5f;
    }

    bool isWalking()
    {
        //print(Vector3.Distance(transform.position, prevPosition));
        if (Vector3.Distance(transform.position, prevPosition) < tolerance&&timer<=0)
        {
            return false;
        }
        return true;
    }

    void finishPacing()
    {
        if(pcc!=null)
        pcc.ContinueToWalk();
        nma.Stop();
        nma.speed = originalSpeed;
        this.enabled = false;

    }

}
