using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;


//Draws an 3d arrow between owner object and a position.
public class ArrowScript : MonoBehaviour {

    public Material material;
    public GameObject cone;
    public float radiusOfCylinder = 1f;

    public Vector3 initialEndPointOffset;
    public float forceMultiplier=3f;

    
    Vector3 endPoint;
    GameObject cylinder;
    AutoMoveAndRotate amar;
    Rigidbody rb;
    Vector3 initialPos;
    Quaternion initialRot;

    Renderer rend;

    bool throwing=false;

    CameraFollower cf;
    LocalGravity gravity;

    private void OnDisable()
    {
        cf.lockCameraRotation(false);
    }

    // Use this f1or initialization
    void Start () {

        cf = CharGameController.getCamera().GetComponent<CameraFollower>();
        cf.lockCameraRotation(true);

        gravity = GetComponent<LocalGravity>();

        rb =GetComponent<Rigidbody>();
        amar = GetComponent<AutoMoveAndRotate>();
        rend = GetComponent<Renderer>();

        cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        //cylinder.transform.parent = transform.parent;

        Destroy(cylinder.GetComponent<Collider>());
        if (material != null) cylinder.GetComponent<Renderer>().material = material;

        endPoint = initialEndPointOffset + transform.position;

  

        updateStartPointAndEndPoint();
    }

    private void FixedUpdate()
    {
        
        updateStartPointAndEndPoint();

        //if (Input.GetMouseButtonDown(1))
        //{
        //    throwCoin();
        //}

    }

    void updateStartPointAndEndPoint()
    {

        endPoint = cone.transform.position;

        Vector3 startPoint;
        startPoint = transform.position;

        float length = Vector3.Distance(endPoint, startPoint);
        //cylinder.transform.parent = null;
        cylinder.transform.position = Vector3.Lerp(startPoint, endPoint, 0.5f);
        cylinder.transform.up = endPoint - startPoint;
        cylinder.transform.localScale = new Vector3(radiusOfCylinder, length, radiusOfCylinder);
        //cylinder.transform.parent = transform;

        Vector3 dir = endPoint - startPoint;
        dir = dir.normalized;
        cone.transform.LookAt(cone.transform.position+dir);
        

    }


    public void throwCoin()
    {

        initialPos = transform.position;
        initialRot = transform.rotation;

        rb.AddForce(forceMultiplier*(endPoint - transform.position), ForceMode.Impulse);
        rb.useGravity = true;
        gravity.enabled = true;
        //amar.enabled = true;

        cone.SetActive(false);
        cylinder.SetActive(false);

        throwing = true;

      
    }

    void reset()
    {
        Debug.Log("Resetting");

        rb.useGravity = false;
        gravity.enabled = false;
        rb.velocity = Vector3.zero;
        //amar.enabled = false;

        transform.position = initialPos;
        transform.rotation = initialRot;

        cone.SetActive(true);
        cylinder.SetActive(true);

        throwing = false;

        rb.angularVelocity = Vector3.zero;



    }
    // Update is called once per frame
    void Update () {
                
        if (throwing && !rend.isVisible) reset();
        

    }
}
