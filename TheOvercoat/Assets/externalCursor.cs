using UnityEngine;
using System.Collections;

//Puts external cursor when object is activated and removes it when object is deacitvated.
public class externalCursor : MonoBehaviour {

    CursorImageScript cis;
    public Texture2D cursor;
	
    // Use this for initialization
	void Start () {
	
	}

    private void OnEnable()
    {

        if (cis == null) cis = CharGameController.getOwner().GetComponent<CursorImageScript>();
        cis.externalTexture = cursor;
    }

    private void OnDisable()
    {
        if (cis != null) cis.resetExternalCursor();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
