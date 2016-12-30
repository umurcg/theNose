using UnityEngine;
using System.Collections;

public class OverrideDenemesiChild : OverrideDenemesiParent {

	// Use this for initialization
	public override void Start () {
        base.Start();
        print("hello i am child");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
