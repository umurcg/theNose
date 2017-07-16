using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This sript changes cursor to texture. It disables cis while doing that. So it is new approach for cursor system.
public class CursorOnMouseOver : MonoBehaviour {

    public Texture2D texture;
    CursorImageScript cis;

	// Use this for initialization
	void Start () {
       cis= CharGameController.getOwner().GetComponent<CursorImageScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        cis.setExternalTexture(texture);
        
        
    }

    private void OnDisable()
    {
        cis.resetExternalCursor();
    }

    private void OnMouseExit()
    {
        cis.resetExternalCursor();
    }


}
