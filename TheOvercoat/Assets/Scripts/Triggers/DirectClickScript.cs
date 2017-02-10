using UnityEngine;
using System.Collections;


//This script triggered when object is clicked.
//It won't take any other action like forcing player to move another position.


public class DirectClickScript : MonoBehaviour {
    IDirectClick idc;
	// Use this for initialization
	void Start () {
        idc = GetComponent<IDirectClick>();
	}
	
	// Update is called once per frame
	void Update () {

      

        if (Input.GetMouseButtonDown(0))
        {


            //Debug.Log("Clicked");
            RaycastHit hit;
            if (Camera.main == null) return;
            Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.transform.name);
                if(hit.transform.gameObject==gameObject){
                    if (idc == null)
                    {
                        print("There is no idirectClick script");
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
