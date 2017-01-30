using UnityEngine;
using System.Collections;

//This scripts moves owner object throug multiple objects with curvutare.
//If curvutare reqired, a curvutare object must added to aimed obnjects.
//Each curvutare object represent the direction of curvature between each transition.
//But right now curvutare changes are not sharp. It will be smoothed.
//There will be a tween script for more general purposes.

public class LerpThroughObjects : MonoBehaviour {

    public float speed = 1f;
    public float curvutare = 3f;
    public GameObject[] Aims;
    public float tolerance = 0.05f;
    Vector3 initialPosition;
    Vector3 aimedPosition;
    Vector3 curveDir;
    int index = -1;
    float ratio;

    // Use this for initialization


    public bool debug = false;
    public GameObject debugObject;
    

    //To Do smooth transitions between aims.

	void Start () {
        StartCoroutine(LerpThrough());
        //findNextAim();

	}
	
	// Update is called once per frame
	void Update () {
     

        //if (Vector3.Distance(transform.position, aimedPosition) < tolerance)
        //{
        //    transform.position = aimedPosition;
        //    findNextAim();
        //}

        //ratio +=Time.deltaTime * speed;
        //transform.position = Vector3.Lerp(initialPosition, aimedPosition, ratio);

        //if (curvutare != 0&&curveDir.magnitude>0)
        //{
        //    if (ratio < 0.5f)
        //    {
        //        transform.position = transform.position + curveDir * curvutare * Mathf.Sin( Mathf.PI*ratio );

        //    }
        //    else
        //    {
        //        transform.position = transform.position + curveDir * curvutare * Mathf.Sin(Mathf.PI*(1 - 2 * (ratio - 0.5f))/2);
        //    }
        //}



        //if (debug)
        //{
        //    GameObject childObj = Aims[index].transform.GetChild(0).gameObject;

        //    Vector3 pos = findCurveDir(aimedPosition - initialPosition, childObj.transform.position - initialPosition, initialPosition);
        //    Instantiate(debugObject, transform.position+ pos, childObj.transform.rotation);
        //    debug = false;
        //}



    }

    public IEnumerator LerpThrough()
    {
        index = -1;
        ratio = 0;
        if(findNextAim()==false)
            yield break;
        while(true)
        {

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
                if (findNextAim() == false)
                    yield break;
            }
            yield return null;

        }


    }

    Vector3 findCurveDir(Vector3 baseVec, Vector3 outsideVec, Vector3 startPoint)
    {
        
        float angle = Vector3.Angle(baseVec, outsideVec);
        float dist = Mathf.Cos((Mathf.PI / 180) *angle) * Vector3.Magnitude(outsideVec);
        Vector3 nearestPoint= startPoint+ baseVec.normalized*dist;
        return startPoint + outsideVec - nearestPoint;
    }


    bool findNextAim()
    {
        if (index + 1 < Aims.Length)
        {
            index++;
            aimedPosition = Aims[index].transform.position;
            initialPosition = transform.position;
            ratio = 0;
            if (Aims[index].transform.childCount > 0)
            {
                GameObject curveObj = Aims[index].transform.GetChild(0).gameObject;
                curveDir = findCurveDir(aimedPosition - initialPosition, curveObj.transform.position - initialPosition, initialPosition);

            }else
            {
                
                curveDir = Vector3.zero;
            }
            return true;
        } else
        {
            return false;
            //this.enabled = false;
        }
    }

}
