using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursorWhenMouseOver : MonoBehaviour {


    CursorImageScript cis;
    public Texture2D text;

	// Use this for initialization
	void Start () {
        cis = CharGameController.getOwner().GetComponent<CursorImageScript>();	
	}

    private void OnMouseEnter()
    {
        if (text) cis.setExternalTexture(text);
    }

    private void OnMouseExit()
    {
        cis.resetExternalCursor();
    }

    private void OnDisable()
    {
        cis.resetExternalCursor();
    }

}
