using UnityEngine;
using System.Collections;

//This script is based on LerpThroughObjects algorithms.
//It lerps through objects to last objects of aim.
// It is written special to camera but it can be implemented to other objects too.
//It has reverse functionality which reverses all movement that is made.
//It is written for camera movements.

public class FlyCameraBetween : MonoBehaviour {

    public float speed = 1f;
    public float curvutare = 3f;
    public GameObject[] Aims;
    public float tolerance = 0.1f;
    public float aimedSize;
    float initialSize;

    Camera cam;
    public GameObject ForwardAimedRotationObject;
    public Vector3 ForwardAimedRotation;
    public GameObject BackwardAimedRotationObject;
    public Vector3 BackwardAimedRotation;

    Vector3 initialPosition;
    Vector3 aimedPosition;
    Vector3 curveDir;
    int index = -1;
    float ratio;

    CameraFollower cf;




    public bool debugfly=false;
    public bool debugreverse = false;
    // Use this for initialization
    void Start () {
        cf = GetComponent<CameraFollower>();
    }

    void Awake()
    {
        cam = GetComponent<Camera>();
        initialSize = cam.orthographicSize;
    }
	
	// Update is called once per frame
	void Update () {

        if (debugfly)
        {
            fly();
            debugfly = false;
        }

        if (debugreverse)
        {
            reverseFly();
            debugreverse = false;
        }

	}

    Vector3 findCurveDir(Vector3 baseVec, Vector3 outsideVec, Vector3 startPoint)
    {

        float angle = Vector3.Angle(baseVec, outsideVec);
        float dist = Mathf.Cos((Mathf.PI / 180) * angle) * Vector3.Magnitude(outsideVec);
        Vector3 nearestPoint = startPoint + baseVec.normalized * dist;
        return startPoint + outsideVec - nearestPoint;
    }


    bool findNextAim(bool reversed)
    {

           
        index = reversed ? index-1 : index+1;
        if (index == Aims.Length || index < 0)
        {
            return false;
        }

        aimedPosition = Aims[index].transform.position;
        initialPosition = transform.position;
        ratio = 0;
        if (Aims[index].transform.childCount > 0)
            {
                GameObject curveObj = Aims[index].transform.GetChild(0).gameObject;
                curveDir = findCurveDir(aimedPosition - initialPosition, curveObj.transform.position - initialPosition, initialPosition);

            }
            else
            {

                curveDir = Vector3.zero;
            }
            return true;
      
    }


    public IEnumerator LerpThrough(bool reversed)
    {
        //Rotation declare

        Vector3 finalAim;
        Quaternion aimRot;
        float size0;
        float size1;
        if (reversed)
        {
            
            finalAim = Aims[0].transform.position;
            aimRot = (BackwardAimedRotationObject == null) ? Quaternion.LookRotation(BackwardAimedRotation) : Quaternion.LookRotation(BackwardAimedRotationObject.transform.position - finalAim);
            size0 = aimedSize;
            size1 = initialSize;
        }
        else
        {
            finalAim= Aims[Aims.Length - 1].transform.position;
            aimRot = (ForwardAimedRotationObject == null) ? Quaternion.LookRotation(ForwardAimedRotation) : Quaternion.LookRotation(ForwardAimedRotationObject.transform.position - finalAim);

            size1 = aimedSize;
            size0 = initialSize;

        }
        Quaternion initialRot = transform.rotation;
        float totalDist = Vector3.Distance(transform.position, finalAim);
        

        index = reversed ? Aims.Length  : -1;
        if (findNextAim(reversed) == false)
            yield break;
        initialPosition = reversed? Aims[Aims.Length-1].transform.position : transform.position;
        print(index);
        ratio = 0;

        

        while (true)
        {

            //Rotation Lerp
 
            transform.rotation = Quaternion.Slerp(initialRot, aimRot, (totalDist-Vector3.Distance(transform.position, finalAim)) / totalDist);

            //Camera Ortho Size
            cam.orthographicSize = Mathf.Lerp(size0, size1, (totalDist - Vector3.Distance(transform.position, finalAim)) / totalDist);

            ratio += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(initialPosition, aimedPosition, ratio);

            if (curvutare != 0 && curveDir.magnitude > 0)
            {
                if (ratio < 0.5f)
                {
                    transform.position = transform.position + curveDir * curvutare * Mathf.Sin(Mathf.PI * ratio);

                }
                else
                {
                    transform.position = transform.position + curveDir * curvutare * Mathf.Sin(Mathf.PI * (1 - 2 * (ratio - 0.5f)) / 2);
                }
            }

            if (Vector3.Distance(transform.position, aimedPosition) < tolerance)
            {
                transform.position = aimedPosition;
                if (findNextAim(reversed) == false)
                {

                    if (reversed)
                    {

                        if (cf)
                            cf.enabled = true;
                    }
                    print("finished");
                    yield break;
                }

            }
           
            yield return null;

        }
        

    }

    public void fly()
    {

        if (cf)
            cf.enabled = false;
        StartCoroutine(LerpThrough(false));

    }

    public void reverseFly()
    {

        //if (cf)
        //    cf.enabled = enabled;
        StartCoroutine(LerpThrough(true));

    }

}
