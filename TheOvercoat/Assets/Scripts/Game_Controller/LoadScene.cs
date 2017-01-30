using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour {

    public enum LoadMethod { Index,SceneName};
    public LoadMethod loadMethod;

    public int index;
    public string sceneName;

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
    
    public void Load()
    {
       

        if (loadMethod == LoadMethod.Index)
        {
            SceneManager.LoadScene(index);
        }else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}
