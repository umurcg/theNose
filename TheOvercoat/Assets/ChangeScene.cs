using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using MovementEffects;

public class ChangeScene : MonoBehaviour {
    public string sceneName;
    public int index;
    public bool fadeBeforeChange=true;

    public GlobalController.Scenes scenes=GlobalController.Scenes.None;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    virtual public void changeScene()
    {
        Timing.RunCoroutine(_changeScene());
    }

    virtual public IEnumerator<float> _changeScene()
    {

        IEnumerator<float> handler = blackScreen.script.fadeOut();
        yield return Timing.WaitUntilDone(handler);
        if (scenes != GlobalController.Scenes.None)
        {
            SceneManager.LoadScene((int)scenes);
        }else if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            changeSceneIndex();
        }
    }

    virtual public void changeSceneIndex()
    {
        SceneManager.LoadScene(index);
    }




}
