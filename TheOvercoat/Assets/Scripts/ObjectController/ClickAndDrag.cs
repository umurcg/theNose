using UnityEngine;
using System.Collections;

//_ClickAndDrag.cs
//_Dependent to:

//This script enable player to drag object with mouse.

public class ClickAndDrag : MonoBehaviour {


    static bool holdingAnObject = false;

    [HideInInspector]
    public bool touched=false;
	public float speed=10f;


    

	//public Vector3 offset ;


	//for child scripts


	// Use this for initialization
	protected void Start () {

	}
	
	// Update is called once per frame
	void Update () {


        
		if (Input.GetMouseButton (0)) {

			if (touched)
				dragObject();
			
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (!holdingAnObject && Physics.Raycast (ray, out hit)) {
				
				if (hit.transform == transform) {

                    holdingAnObject = true;
                    touched = true;
				}
			}
		

		}else if (Input.GetMouseButtonUp (0)) {

            holdingAnObject = false;
			touched = false;
		}

	}




	public virtual void dragObject(){

		//transform.position += new Vector3 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"), 0) * Time.deltaTime * speed;
			
		transform.position=Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition+Vector3.forward*GlobalController.cameraForwardDistance),Time.deltaTime*speed);

		//		if (Input.GetAxis ("Mouse X") !=0|| Input.GetAxis ("Mouse Y")!=0) {
		//			transform.position += (transform.up/3-transform.forward)* Time.deltaTime*speed;
		//		}
	}

}
