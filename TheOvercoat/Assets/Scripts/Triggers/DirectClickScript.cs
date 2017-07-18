using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//This script triggered when object is clicked.
//It won't take any other action like forcing player to move another position.


public class DirectClickScript : MonoBehaviour {
    IDirectClick idc;
    Text charSubt;
    Camera mainCam;
	// Use this for initialization
	void Start () {
        idc = GetComponent<IDirectClick>();
        charSubt= SubtitleFade.subtitles["CharacterSubtitle"];
        mainCam = CharGameController.getMainCameraComponent();
    }
	
	// Update is called once per frame
	void Update () {

      
        //If char subtitile is active  then dont listen mouse
        if (charSubt.text=="" && Input.GetMouseButtonDown(0))
        {


            //Debug.Log("Clicked");
            RaycastHit hit;
            //if (Camera.main == null) return;
            //Ray ray = new Ray(mainCam.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log("Raycasted");
                //Vckrs.printNameTagLayer(hit.transform.gameObject);
                if(hit.transform.gameObject==gameObject){
                    Debug.Log(hit.transform.name);
                    if (idc == null)
                    {
                        Debug.Log("There is no idirectClick script");
                    }
                    else
                    {
                        idc.directClick();
                    }
                }


            }
           
        }
	}
}
