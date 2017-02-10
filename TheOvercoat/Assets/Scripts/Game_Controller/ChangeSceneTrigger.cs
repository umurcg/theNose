using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//This script change scene to sceneName.
//Also it is an enter trigger.
//But dont use it again.

public class ChangeSceneTrigger : ChangeScene {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public virtual void OnTriggerEnter(){
        print("entered");
        base.changeScene();
        
	
	}


}
