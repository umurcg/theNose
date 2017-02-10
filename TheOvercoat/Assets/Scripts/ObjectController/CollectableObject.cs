using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//_CollectableObject.cs
//_Dependent to: 

//This script enable player to collect parent object with mouse click.
//Collected objects saved to a static List of this class.
//If "Hand On" is enabled then collected object attached to hands of player.


public class CollectableObject : MonoBehaviour, IClickAction {
	public static List<GameObject> collected;
	// Use this for initialization
	public bool onHand=false;
	Transform rightHand;
	Transform leftHand;
    
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

        List<Transform> children = Vckrs.getAllChildren(player);

        foreach (Transform c in children)
        {
            if (c.name == "Hand_R")
            {
                rightHand = c;
            }
            else if (c.name == "Hand_L")
            {
                leftHand = c;
            }


        }        
        

		foreach (GameObject placeholder in placeholders){
			placeholder.SetActive(true);
		}

    }
	
	// Update is called once per frame

	void Collect(){
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




	void Update () {



//		if (Input.GetMouseButtonUp (0)) {
//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//			RaycastHit hit;
//
//			if (Physics.Raycast (ray, out hit)) {
//				
//				if (hit.transform == transform) {
//					
//					Collect ();
//				}
//
//			}
//	
//		}
	}


   

}
