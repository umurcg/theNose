using UnityEngine;
using System.Collections;

public class MoveWithMouseV2 : MonoBehaviour {
    
    Camera mainCam;

    //public GameObject plane;
    public GameObject canvas;
    public bool lerp = true;
    public float lerpSpeed = 3f;
    Vector3 aim;

    //Object distance projected on forward direction of camera 
    float distanceFromCamera;


    // Use this for initialization
    void Start()
    {
        mainCam = CharGameController.getCamera().GetComponent<Camera>();

        //Vector3 camToObj=transform.position-mainCam.transform.position;
        //float distance = camToObj.magnitude;

        //float alpha = Vector3.Angle(camToObj, mainCam.transform.forward);
        //distanceFromCamera = Mathf.Sin(alpha * Mathf.Deg2Rad) * distance;

        //Debug.Log("Distance is " + distanceFromCamera);

    }

    // Update is called once per frame
    void Update()
    {
        if (lerp && aim != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, aim, Time.deltaTime * lerpSpeed);

            if (Vector3.Distance(transform.position, aim) < 0.01f) aim = Vector3.zero;

        }
    }

    private void OnMouseDrag()
    {

        Vector3 v1 = transform.position;
        Vector3 v2 = transform.position + mainCam.transform.right;
        Vector3 v3 = transform.position + mainCam.transform.up;

        UnityEngine.Plane plane = new UnityEngine.Plane(v1, v2, v3);

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if(plane.Raycast(ray,out rayDistance))
        {
            if (lerp)
            {
                aim = ray.GetPoint(rayDistance); /*+ offset*/;
            }
            else
            {
                transform.position = ray.GetPoint(rayDistance);/*+offset*/;
            }

        }

        //Vector3 mousePos = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y));

       
        
        //Vector3 offset= canvas.transform.position-mainCam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));

        //if (lerp)
        //{
        //    aim = mousePos /*+ offset*/;
        //}
        //else
        //{
        //    transform.position = mousePos/*+offset*/;
        //}


        //Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit))
        //{
        //    //Debug.Log(hit.transform.name);
        //    if (hit.transform.gameObject == plane)
        //    {
        //        //Debug.Log("Moving");
        //        if (lerp)
        //        {
        //            aim = hit.point;
        //        }
        //        else
        //        {
        //            transform.position = hit.point;
        //        }
        //    }
        //}


    }
}
