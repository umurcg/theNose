using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeSceneTrigger : MonoBehaviour {
	public  string sceneName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeScene(){
		Debug.Log ("Scene changed.");
		SceneManager.LoadScene (sceneName);
	}

	public virtual void OnEnterTrigger(){
		changeScene();
	
	}


}
