using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;


//_CollectableObject.cs
//_Dependent to: 

//This script enable player to collect parent object with mouse click.
//Collected objects saved to a static List of this class.
//If "Hand On" is enabled then collected object attached to hands of player.


public class CollectableObject : MonoBehaviour, IClickAction {
	public static List<GameObject> collected;
	// Use this for initialization
	public bool onHand=false;
    static Transform rightHand;
    static Transform leftHand;
    
	Transform player;
	public GameObject[] placeholders;
    public bool canBeCollectedAgain=false;
    public Vector3 unCollectPositionOffset;
    public Vector3 scale =new Vector3(0,0,0);
    Vector3 originalScale;
    Quaternion originalRotation;
    Transform parent;
    MeshCollider mc;
	Rigidbody rb;

	void Awake(){

	}

	void Start () {
		mc = GetComponent<MeshCollider> ();
		rb = GetComponent<Rigidbody> ();

		originalScale = transform.localScale;
		if (CollectableObject.collected == null)
			CollectableObject.collected = new List<GameObject> ();
		parent = transform.parent;

        player = CharGameController.getActiveCharacter().transform;

        originalRotation = transform.rotation;

        if (player == null)
        {
            Debug.Log("Player is null");
            enabled = false;
            return;
        }

        assignHands();

        //List<Transform> children = Vckrs.getAllChildren(player);

        //foreach (Transform c in children)
        //{
        //    if (c.name == "Hand_R")
        //    {
        //        rightHand = c;
        //    }
        //    else if (c.name == "Hand_L")
        //    {
        //        leftHand = c;
        //    }


        //}        


        foreach (GameObject placeholder in placeholders){
			placeholder.SetActive(true);
		}

    }
	
	// Update is called once per frame

	void Collect(){

        Debug.Log("Collecting");

		CollectableObject.collected.Add (gameObject);
		if (onHand == false) {
			gameObject.SetActive(false);
		} else {

			enableMeshCollider (false);

			if (rightHand.childCount == 0) {
				
				transform.parent = rightHand;

				transform.localPosition = unCollectPositionOffset;
			} else if (leftHand.childCount == 0) {
				transform.parent = leftHand;

				transform.localPosition =unCollectPositionOffset;

			} else {

				rightHand.GetChild (0).gameObject.SetActive(false);
				transform.parent = rightHand;
				transform.localPosition = unCollectPositionOffset;

			}
			 
			transform.localScale =originalScale+ scale;
            
		

		}

        gameObject.SetActive(true);

        //foreach (GameObject placeholder in placeholders){
        //    gameObject.SetActive(true);
        //}

        //Disable texture
        transform.tag = "Untagged";
        //MouseTexture mt = GetComponent<MouseTexture>();
        //if (mt != null)
        //    mt.checkTag();

        

	}

	public void UnCollect(Vector3 position){
		enableMeshCollider (true);
		transform.parent = parent;
		collected.Remove (gameObject);

        gameObject.SetActive(true);
        //foreach (GameObject placeholder in placeholders){
        //    //gameObject.SetActive(false);
        //}

		if (canBeCollectedAgain == false) {
            Destroy(this);
			this.enabled = false;
		}

		transform.localScale = originalScale;
        transform.rotation = originalRotation;

        transform.position = position;

        if (canBeCollectedAgain == true)
        {
            transform.tag = "ActiveObject";
        }
            
	}

	public void Action(){
		
		Collect ();
	}

	public void enableMeshCollider(bool b){

		if (mc != null) {
			mc.enabled = b;
		}
		if(rb!=null){
			rb.useGravity=b;
		}

	}

    public static IEnumerator<float> goAndCollectObject(NavMeshAgent agent, GameObject objectToCollect, Vector3 unCollectPositionOffset, bool onhand=true)
    {
        Vector3 posOnNavmesh = objectToCollect.transform.position;
        Vckrs.findNearestPositionOnNavMesh(posOnNavmesh,agent.areaMask,20f,out posOnNavmesh);

        agent.SetDestination(posOnNavmesh);
        agent.Resume();

        IEnumerator<float> handler= Timing.RunCoroutine(Vckrs.waitUntilStop(agent.gameObject));
        yield return Timing.WaitUntilDone(handler);

        if (CollectableObject.collected != null) CollectableObject.collected.Add(objectToCollect);

        MeshCollider ms = objectToCollect.GetComponent<MeshCollider>();

        assignHands();

        if (onhand == false)
        {
            objectToCollect.SetActive(false);
        }
        else
        {

            if (ms) ms.enabled = false;

            if (rightHand.childCount == 0)
            {

                objectToCollect.transform.parent = rightHand;

                objectToCollect.transform.localPosition = unCollectPositionOffset;
            }
            else if (leftHand.childCount == 0)
            {
                objectToCollect.transform.parent = leftHand;

                objectToCollect.transform.localPosition = unCollectPositionOffset;

            }
            else
            {

                rightHand.GetChild(0).gameObject.SetActive(false);
                objectToCollect.transform.parent = rightHand;
                objectToCollect.transform.localPosition = unCollectPositionOffset;

            }

            //transform.localScale = originalScale + scale;



        }

        agent.gameObject.SetActive(true);

        //foreach (GameObject placeholder in placeholders){
        //    gameObject.SetActive(true);
        //}


    }
   

    static void assignHands()
    {
        if (rightHand == null || leftHand == null)
        {
            rightHand = CharGameController.getHand(CharGameController.hand.RightHand).transform;
            leftHand = CharGameController.getHand(CharGameController.hand.LeftHand).transform;
        }
    }

}
