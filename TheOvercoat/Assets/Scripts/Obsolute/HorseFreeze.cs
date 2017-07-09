using UnityEngine;
using System.Collections;

public class HorseFreeze : MonoBehaviour {
    UnityEngine.AI.NavMeshAgent nma;
    public GameObject carierFront;
    public GameObject carierBack;
    Rigidbody carierBackcc;
    Rigidbody carierFrontcc;
    Rigidbody cc;

   
	// Use this for initialization
	void Start () {
        nma = GetComponent<UnityEngine.AI.NavMeshAgent>();
        cc = GetComponent<Rigidbody>();
        carierBackcc = carierBack.GetComponent<Rigidbody>();
        carierFrontcc = carierFront.GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void freeze()
    {
        cc.constraints = RigidbodyConstraints.FreezeAll;
        carierBackcc.constraints = RigidbodyConstraints.FreezeAll;
        carierFrontcc.constraints = RigidbodyConstraints.FreezeAll;
        nma.enabled = false;
    }
    public void release()
    {
        cc.constraints = RigidbodyConstraints.None;
        carierBackcc.constraints = RigidbodyConstraints.None;
        carierFrontcc.constraints = RigidbodyConstraints.None;
        nma.enabled = enabled;
    }

}
