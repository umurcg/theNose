//#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

//This script only works in editor mode.
//It takes screen shot at start of level and save it as caption for this level. Those screenshots will  be used in moments section of main menu.
public class LevelScreenShotScript : MonoBehaviour {

    
    public int multiplier = 1;


	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void takeScreenShot()
    {

        string directory = /*"/Assets/Textures/MomentsImages/" + */(int)(GlobalController.getPreviousScene()) + ".jpg";


         
        Application.CaptureScreenshot(directory,multiplier);

        
        Debug.Log("Screen shot is saved to " + directory);
    }
}
//#endif
