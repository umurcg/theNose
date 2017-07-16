using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class TutorailCanvas : MonoBehaviour {

    public enum Tutorials {MouseMovement=0,KeyboardMovement=1, CameraRotator=2,InteractiveObjects=3,Subtitle=4,MouseRotate,MouseZoom,Map,Bird,Shoot }

    public Tutorials[] basicTutorials;

    //public GameObject[] basicTutorials;
    
    
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
            startBasicTutorial();
            debug = false;
        }
    }

    public void fullTutorial(float duration = 5f)
    {
        Timing.RunCoroutine(_fullTutorial(duration));
    }


    IEnumerator<float> _fullTutorial(float duration = 5f)
    {

        var tutorialIndexes=System.Enum.GetValues(typeof(Tutorials));

        foreach(var index in tutorialIndexes)
        {

            //Even it is full tutorial bird tutorial contains spoiler. So while main char is not bird, bird tutorial will be passed.
            if ( (int)index==(int)Tutorials.Bird && CharGameController.cgc != null && CharGameController.getActiveCharacter().name != "Bird")
            {
                Debug.Log("Passsing bird tutorial");
                continue;
            }


            GameObject child = transform.GetChild((int)index).gameObject;
            child.SetActive(true);
            yield return Timing.WaitForSeconds(duration);
            child.SetActive(false);
        }

    }

    public void startBasicTutorial( float duration = 5f)
    {
        Debug.Log("Starting full tutorial");
        Timing.RunCoroutine(_startBasicTutorial( duration));
    }


    IEnumerator<float> _startBasicTutorial(float duration = 5f)
    {
      
        for (int i = 0; i < basicTutorials.Length; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(true);
            yield return Timing.WaitForSeconds(duration);
            child.SetActive(false);
        }
    }

    public void startTutorial(Tutorials tutorial, float duration = 5f) {
        Timing.RunCoroutine(_startTutorial(tutorial,duration));
    }

    IEnumerator<float> _startTutorial(Tutorials tutorial, float duration = 5f)
    {

            GameObject child = transform.GetChild((int)tutorial).gameObject;
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

    public void startTutorial(Tutorials[] tutorials, float duration = 5f)
    {
        Timing.RunCoroutine(_startTutorial(tutorials, duration));
    }

    IEnumerator<float> _startTutorial(Tutorials[] tutorials, float duration = 5f)
    {        

        foreach(Tutorials t in tutorials)
        {
            IEnumerator<float> handler= Timing.RunCoroutine(_startTutorial(t, duration));
            yield return Timing.WaitUntilDone(handler);
        }
        yield break;

    }

}
