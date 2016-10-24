using UnityEngine;
using System.Collections;

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
