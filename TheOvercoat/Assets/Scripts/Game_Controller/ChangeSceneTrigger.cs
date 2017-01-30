using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//This script change scene to sceneName.
//Also it is an enter trigger.
//But dont use it again.

public class ChangeSceneTrigger : MonoBehaviour {
	public  string sceneName;
    public int index;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeScene(){
		Debug.Log ("Scene changed.");

        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }else
        {
            changeSceneIndex();
        }
	}

    public void changeSceneIndex()
    {
        SceneManager.LoadScene(index);
    }

	public virtual void OnTriggerEnter(){
        print("entered");
        changeScene();
        
	
	}


}
