using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class TutorailCanvas : MonoBehaviour {

    public GameObject[] basicTutorials;
    
    
    public bool debug;

    //[System.Serializable]
    //public struct scenesAndTutorials
    //{
    //    public int sceneIndex;
    //    public GameObject[] tutorials;
    //}

    //public scenesAndTutorials[] sat;

    //Dictionary<int, int[]> sceneIndexToTutorialIndex;

	// Use this for initialization
	void Start () {

        //Initilize scene index to tutorials
        //sceneIndexToTutorialIndex = new Dictionary<int, int[]>();

        //sceneIndexToTutorialIndex[0] = new int[3] { 1, 2, 3 };


        //GetComponent<Canvas>().worldCamera = Camera.current;
        //Current episode


        //switch (index)
        //{
        //    case (0):




        ////}
        //Timing.RunCoroutine(activateTutorialAccordingToScene());

	}

    //IEnumerator<float> activateTutorialAccordingToScene()
    //{
        
    //    int index = GlobalController.Instance.sceneList.Count - 1;
    //    Debug.Log("scene is " + index);
    //    scenesAndTutorials mySat=new scenesAndTutorials();

    //    //Check if index in struct
    //    foreach(scenesAndTutorials s in sat)
    //    {
    //        if (s.sceneIndex == index) mySat = s;
    //    }

    //    if (mySat.tutorials == null || mySat.tutorials.Length == 0) yield break ;

    //    foreach (GameObject tut in mySat.tutorials)
    //    {
    //        IEnumerator<float> handler = Timing.RunCoroutine(_startTutorial(tut));
    //        yield return Timing.WaitUntilDone(handler);

    //    }



    //    yield break;
        
    //}

	
	// Update is called once per frame
	void Update () {
        if (debug == true)
        {
            startFullTutorial();
            debug = false;
        }
    }

    public void startFullTutorial( float duration = 5f)
    {
        Debug.Log("Starting full tutorial");
        Timing.RunCoroutine(_startFullTutorial( duration));
    }


    IEnumerator<float> _startFullTutorial(float duration = 5f)
    {
      
        for (int i = 0; i < basicTutorials.Length; i++)
        {
            GameObject child =basicTutorials[i].gameObject;
            child.SetActive(true);
            yield return Timing.WaitForSeconds(duration);
            child.SetActive(false);
        }
    }

    public void startTutorial(int index, float duration = 5f) {
        Timing.RunCoroutine(_startTutorial(index,duration));
    }

    IEnumerator<float> _startTutorial(int index, float duration = 5f)
    {
     
            GameObject child = basicTutorials[index];
            child.SetActive(true);
            yield return Timing.WaitForSeconds(duration);
            child.SetActive(false);
        
    }

    IEnumerator<float> _startTutorial(GameObject obj, float duration = 5f)
    {

     
        obj.SetActive(true);
        yield return Timing.WaitForSeconds(duration);
        obj.SetActive(false);

    }

    public void startTutorial(int[] indexes, float duration = 5f)
    {
        Timing.RunCoroutine(_startTutorial(indexes, duration));
    }

    IEnumerator<float> _startTutorial(int[] indexes, float duration = 5f)
    {        

        foreach(int i in indexes)
        {
            IEnumerator<float> handler= Timing.RunCoroutine(_startTutorial(i, duration));
            yield return Timing.WaitUntilDone(handler);
        }
        yield break;

    }

}
