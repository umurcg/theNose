using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


//While ui elements doesnt detected by corsorimage script this script will frce cis to change cursor to button cursor when mouse is over.
public class ButtonCursorSetter : MonoBehaviour {

    CursorImageScript cis;
    public Texture2D cursorImage;

	// Use this for initialization
	void Start () {
        cis = CharGameController.getOwner().GetComponent<CursorImageScript>();

        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry enterEvent = new EventTrigger.Entry();
        enterEvent.eventID = EventTriggerType.PointerEnter;
        enterEvent.callback.AddListener((eventData) => { changeCursor(); });
        trigger.triggers.Add(enterEvent);

        EventTrigger.Entry exitEvent = new EventTrigger.Entry();
        exitEvent.eventID = EventTriggerType.PointerExit;
        exitEvent.callback.AddListener((eventData) => { recoverCursor(); });
        trigger.triggers.Add(exitEvent);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void changeCursor()
    {
        cis.externalTexture = cursorImage;
        //Debug.Log("Mouse entered");
    }

    public void recoverCursor()
    {
        if(cis)
        cis.resetExternalCursor();
    }

    private void OnDestroy()
    {
        recoverCursor();
    }
}
