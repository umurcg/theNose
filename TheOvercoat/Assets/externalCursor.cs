using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Puts external cursor when object is activated and removes it when object is deacitvated.
public class externalCursor : MonoBehaviour {

    CursorImageScript cis;
    public Texture2D cursor;
    public GamePadMouse gamePadScript;
	
    // Use this for initialization
	void Start () {
	
	}

    private void OnEnable()
    {

        if (cis == null) cis = CharGameController.getOwner().GetComponent<CursorImageScript>();
        cis.externalTexture = cursor;

        if (gamePadScript != null) gamePadScript.gamePadCursor.GetComponent<RawImage>().texture = cursor;
    }

    private void OnDisable()
    {
        if (cis != null) cis.resetExternalCursor();

        if (gamePadScript != null) gamePadScript.gamePadCursor.GetComponent<RawImage>().texture = null;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
