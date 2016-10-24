using UnityEngine;
using System.Collections;

public class AreThereAnyObjectInPath : MonoBehaviour {
    public float capsuleRadius = 5f;
    public string objectTag;

    //public GameObject debugObject;
	// Use this for initialization
	void Start () {


    }
	
	// Update is called once per frame
	void Update () {
        //if (debugObject != null)
        //    print(AreThereAny(transform, debugObject.transform.position));
	}

    public bool AreThereAny(Transform tr, Vector3 pos)
    {
        bool result = false;
        Collider[] cols = Physics.OverlapCapsule(pos,tr.position,capsuleRadius);

        foreach (Collider col in cols)
        {
            result = result || col.tag == objectTag;
        }

        return result;
    }
}
