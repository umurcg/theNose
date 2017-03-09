using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class TutorailCanvas : MonoBehaviour {

    
    public bool debug;
    

	// Use this for initialization
	void Start () {
        //GetComponent<Canvas>().worldCamera = Camera.current;
	}
	
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
      
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
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
     
            GameObject child = transform.GetChild(index).gameObject;
            child.SetActive(true);
            yield return Timing.WaitForSeconds(duration);
            child.SetActive(false);
        
    }
}
