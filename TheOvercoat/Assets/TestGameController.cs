using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TestGameController : MonoBehaviour {



	// Use this for initialization
	void Awake () {


        if (GlobalController.Instance == null)
        {
            initialFunc();
        }
        else
        {

            print("scenes: ");
            foreach (int i in GlobalController.Instance.sceneList)
            {
                print(i);
            }

            if (GlobalController.Instance.sceneList.Contains(SceneManager.GetActiveScene().buildIndex))
            {
                wecomeAgain();

            }
            else
            {
                initialFunc();
            }
            //print("currentScene: " + SceneManager.GetActiveScene().buildIndex);

        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void initialFunc()
    {
        print("hi, you are coming here first time.");
      
    }

    void wecomeAgain()
    {
        print("welcomeAgain");
    }

    
}
