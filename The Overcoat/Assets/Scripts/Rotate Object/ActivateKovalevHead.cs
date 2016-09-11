using UnityEngine;
using System.Collections;

public class ActivateKovalevHead : MonoBehaviour {
    objectRotateMouse script;
    GameObject[] gameobjects;
    bool delay=false;
    float timer;
    float enableScriptDelay = 2;
    // Use this for initialization
	void Start () {
        script = GetComponent<objectRotateMouse>();
        script.enabled = false;
        foreach (GameObject go in gameobjects)
        {
            go.SetActive(false);
            delay = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (delay)
        {
            timer += Time.deltaTime;
        }
        if (timer > enableScriptDelay) script.enabled = true;
	}

    public void active()
    {
        foreach (GameObject go in gameobjects)
        {
            go.SetActive(true);
            delay = true;
        }
     
        
                
    }

}
