using UnityEngine;
using System.Collections;

public class lerpTrial : MonoBehaviour {
    public Transform aim;


	// Use this for initialization
	void Start () {
        LerpStandAlone.lerp(transform, aim, 3f, 0.1f);


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
