using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using MovementEffects;

public class LoadScene : MonoBehaviour {

    //public enum LoadMethod { Index,SceneName};
    //public LoadMethod loadMethod;
    public bool fade = true;
    public GlobalController.Scenes Scene = GlobalController.Scenes.None;
    //public int index;
    //public string sceneName;

    //Test
    public bool debugLoad;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (debugLoad)
        {
            Load();
            debugLoad = false;
        }
    }
    

    [ContextMenu ("Load")]
    public void Load()
    {
        Timing.RunCoroutine(_Load());
    }

    public virtual IEnumerator<float> _Load()
    {
        if (fade && blackScreen.script!=null)
        {
            IEnumerator<float> handler = blackScreen.script.fadeOut();
            yield return Timing.WaitUntilDone(handler);
        }


        SceneManager.LoadScene((int)Scene);
        //if (loadMethod == LoadMethod.Index)
        //{
        //    SceneManager.LoadScene(index);
        //}else
        //{
        //    SceneManager.LoadScene(sceneName);
        //}
        yield break;
    }

}
