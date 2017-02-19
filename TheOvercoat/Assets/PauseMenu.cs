using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    bool paused = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Cancel"))
        {
            switchMenu();
        }
	}

    void switchMenu()
    {
        if (paused)
        {
            activateMenu(false);
        }else
        {
            activateMenu(true);
        }
        paused = !paused;
    }

    void activateMenu(bool b)
    {
        Time.timeScale = b ? 0 : 1;

        GetComponent<RawImage>().enabled = b;
        GetComponentInChildren<Text>().enabled = b;
    }


}
