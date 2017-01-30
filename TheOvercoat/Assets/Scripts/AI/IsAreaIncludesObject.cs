using UnityEngine;
using System.Collections;

//This scripts incudes functions related to checking wheter or not an area includes object.

public class IsAreaIncludesObject : MonoBehaviour{
    public string objectTag;


    //public GameObject debugPos;
    //public float debugRadius;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //print(isInclude(debugPos.transform.position, debugRadius));

	}

    public static int howManyObjectsInSphere(Vector3 pos, float radius, string tag)
    {

        Collider[] cols = Physics.OverlapSphere(pos, radius);
        int result = 0;

        foreach (Collider col in cols)
        {
            if (col.tag == tag)
                result++;
            //print(col.name);
        }
        //print(result);
        return result;
    }


    public static bool isIncludeInSphere(Vector3 pos, float radius, string objectTag)
    {
        bool result = false;
        Collider[] cols = Physics.OverlapSphere(pos, radius);

        foreach (Collider col in cols)
        {
            result = result || col.tag == objectTag;
        }

        return result;
    }


    public bool isInclude(Vector3 pos, float radius)
    {
        bool result = false;
        Collider[] cols= Physics.OverlapSphere(pos, radius);

        foreach (Collider col in cols)
        {
           result = result || col.tag == objectTag;
        }

        return result;
    }



}
