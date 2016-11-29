using UnityEngine;
using System.Collections;

//This script disables mesh renderer while transformin.
public class HideWhileTransforming : MonoBehaviour {
    public GameObject subject;
    public float tolerance=0.5f;
    Vector3 pos;
    bool enable;
    MeshRenderer mr;
	// Use this for initialization
	void Start () {
        mr = GetComponent<MeshRenderer>();
        pos = subject.transform.position;
        if (subject == null)
            subject = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        //print(Vector3.Distance(subject.transform.position, pos));
        if (Vector3.Distance(subject.transform.position, pos) > tolerance)
        {
            if (enable)
            {
                mr.enabled = false;
                enable = false;
            }
        }else
        {
            if (!enable)
            {
                mr.enabled = true;
                enable = true;
            }
        }
        pos = subject.transform.position;
	}
}
