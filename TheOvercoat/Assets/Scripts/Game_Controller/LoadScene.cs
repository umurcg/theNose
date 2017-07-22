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



    public bool loadWithLoadingScreen = true;

    //public GameObject canvasObj;
    public GameObject loadingScreenPefab;
    public float minLoadingScreenDur = 3f;

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


    public void Load(float delay)
    {
        Timing.RunCoroutine(_Load(delay));
    }

    public virtual IEnumerator<float> _Load(float delay)
    {
        yield return Timing.WaitForSeconds(delay);
        Load();
    }

    public virtual IEnumerator<float> _Load()
    {
        if (fade && blackScreen.script!=null)
        {
            IEnumerator<float> handler = blackScreen.script.fadeOut();
            yield return Timing.WaitUntilDone(handler);
        }


        if (loadWithLoadingScreen)
        {


            //Instantiate Loading Screen
            Debug.Log("Creating loading page");
            GameObject spawned=Instantiate(loadingScreenPefab);

            if(blackScreen.script!=null)
                blackScreen.script.gameObject.SetActive(false);

            yield return Timing.WaitForSeconds(minLoadingScreenDur);

            AsyncOperation async = SceneManager.LoadSceneAsync((int)Scene);

            while (!async.isDone) yield return 0;

            Destroy(spawned);
            //Destroy Loading Screen
        }
        else
        {

            SceneManager.LoadScene((int)Scene);

        }
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
