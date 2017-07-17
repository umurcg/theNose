using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Assingn funciton to buttons of camera prompt
public class CameraPromptListenerAssigner : MonoBehaviour {

    public Button perspective, ortogtaphic;
    

	// Use this for initialization
	void Start () {

        perspective.onClick.AddListener(setTypeAsPerso);
        ortogtaphic.onClick.AddListener(setTypeAsOrto);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void setTypeAsOrto()
    {
        Debug.Log("Setting as ortoo");
        CharGameController.setCameraType(CharGameController.cameraType.Ortographic);
    }

    void setTypeAsPerso()
    {
        CharGameController.setCameraType(CharGameController.cameraType.Perspective);
    }

}
