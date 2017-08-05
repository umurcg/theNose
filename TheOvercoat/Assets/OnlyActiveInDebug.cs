using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyActiveInDebug : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!Debug.isDebugBuild) gameObject.SetActive(false);
	}
}
