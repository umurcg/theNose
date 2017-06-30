using UnityEngine;
using System.Collections;


//Let user move owner object in 2d
//You should create a plane for 2d movement and assign it
public class MoveWithMouse : MonoBehaviour {


    Camera mainCam;

    public GameObject plane;
    public bool lerp=true;
    public float lerpSpeed = 3f;
    Vector3 aim;

	// Use this for initialization
	void Start () {
        mainCam = CharGameController.getCamera().GetComponent<Camera>();


        
	}
	
	// Update is called once per frame
	void Update () {
        if (lerp && aim!=Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, aim, Time.deltaTime * lerpSpeed);

            if (Vector3.Distance(transform.position, aim) < 0.01f) aim = Vector3.zero;    

        }
	}

    private void OnMouseDrag()
    {

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)){
            //Debug.Log(hit.transform.name);
            if (hit.transform.gameObject == plane)
            {
                //Debug.Log("Moving");
                if (lerp)
                {
                    aim = hit.point;
                }
                else
                {
                    transform.position = hit.point;
                }
            }
        }
        

    }


}
