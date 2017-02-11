using UnityEngine;
using System.Collections;

//_RandomWalkBot.cs
//Dependent to: RoadObject, NavMeshAgent

//This script makes object randomly walk on RoadObject.It generates random position on road and set agent destination to that position when object finish its path.WaitbetweenWalks makes object wait when it finish its path before starting to new path.Tolerance affects finish condition of obj.
//Also interruptAndWaşlAgain can be called outside to change destination of object from outside.

public class RandomWalkBot : GameController {
	public GameObject obj;

	protected NavMeshAgent nma;
	protected Vector3 lastCheckedPos;
	protected float timer=0;

	public float waitBetweenWalks;
	public float tolerance;
    // Use this for initialization

    public override void Start () {
        base.Start();
		nma = GetComponent<NavMeshAgent> ();
			
	}
	
	// Update is called once per frame
	public virtual void Update () {

		if (timer <= 0) {
			timer = 0;
			WalkIfNotWalking ();
		} else {
			timer -= Time.deltaTime;
		}
		//timer == 0 ? WalkIfNotWalking() : timer -= Time.deltaTime;
		

	}

	public virtual void interruptAndWalkAgain(){
		nma.Stop();
		WalkIfNotWalking ();
	}

	public void WalkIfNotWalking(){
		if (checkIsMoving () == false) {

            if (nma.isOnNavMesh)
            {
                Vector3 dest = GetARandomTreePos();
                nma.Resume();
                nma.destination = dest;
                //Debug.Log("Destination is " + dest.ToString());
            }else
            {
                Debug.Log("Is not on mesh");
            }
			timer = waitBetweenWalks;
		}
	}


	protected bool checkIsMoving(){
		//if (lastCheckedPos == null) {
		//	lastCheckedPos = transform.position;
		//	return false;
		//} else {
			if (Vector3.Distance( transform.position, lastCheckedPos)>tolerance) {
				lastCheckedPos = transform.position;
				return true;
			} else {
				return false;
			//}
		}

	}

   protected Vector3 GetARandomTreePos(){

		Mesh planeMesh = obj.GetComponent<MeshFilter>().mesh;
		Bounds bounds = planeMesh.bounds;

		float minX = obj.transform.position.x - obj.transform.localScale.x * bounds.size.x * 0.5f;
		float maxX = obj.transform.position.x+ obj.transform.localScale.x  * bounds.size.x * 0.5f;
		float minZ = obj.transform.position.z- obj.transform.localScale.z * bounds.size.z * 0.5f;
		float maxZ = obj.transform.position.z+ obj.transform.localScale.z * bounds.size.z * 0.5f;
		Vector3 newVec = new Vector3(Random.Range (maxX, minX),
			transform.position.y,
			Random.Range (maxZ, minZ));
		return newVec;
	}

    public override void activateController()
    {
        gameObject.SetActive(true);
    }
    public override void deactivateController()
    {

        gameObject.SetActive(false);

    }

}
