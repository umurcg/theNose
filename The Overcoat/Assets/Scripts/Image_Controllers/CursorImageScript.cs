using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//Dependent to: Player(Owner)
//This script changes mouse image according to hitted object. 
//If object is note an active object, default image will be shown.

public class CursorImageScript : MonoBehaviour {

    //public float xOffset, yOffset = 10;
    public Texture2D defaultTexture;
	public Texture2D disabled;

    RaycastHit lastHit;

	GameObject player;
	MoveTo mt;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		mt = player.GetComponent < MoveTo> ();
	}
	
	// Update is called once per frame
	void Update () {
		

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
			
			if (!checkAvaiblity()) {
				Cursor.SetCursor(disabled, Vector2.zero, CursorMode.Auto);
			}

 
			if (hit.transform.tag == "ActiveObject"&&hit.transform.GetComponent<MouseTexture> ().texture!=null) {
				
				Cursor.SetCursor (hit.transform.GetComponent<MouseTexture> ().texture, Vector2.zero, CursorMode.Auto);

			}  else
            {
                                
                Cursor.SetCursor(defaultTexture, Vector2.zero, CursorMode.Auto);

            }


        }


    }

    


	bool checkAvaiblity(){
	 return	mt.enabled;
	}

}
