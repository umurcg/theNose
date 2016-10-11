using UnityEngine;
using System.Collections;

public class ReflectionProbeScript : MonoBehaviour {
    ReflectionProbe rp;
	// Use this for initialization
	void Awake () {
        rp = GetComponent<ReflectionProbe>();
        rp.RenderProbe();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void reflect()
    {
        rp.RenderProbe();
    }
}
