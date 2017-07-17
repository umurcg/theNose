using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//While ui elements doesnt detected by corsorimage script this script will frce cis to change cursor to button cursor when mouse is over.
public class ButtonCursorSetter : MonoBehaviour {

    //This boolean will be used when a action from click will be taken. Before taking action this boolean will be checked wheter or not use trying to click on a button or just
    //playing game.
    public static bool mouseIsOverButton = false;

    CursorImageScript cis;
    public Texture2D cursorImage;
    Button buttonComp;

    bool mousePressed = false;
    bool recover = false;

    // Use this for initialization
    void Start () {
        cis = CharGameController.getOwner().GetComponent<CursorImageScript>();

        buttonComp = GetComponent<Button>();

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

        if (Input.GetMouseButtonDown(0)) mousePressed = true;
        if (Input.GetMouseButtonUp(0)) mousePressed = false;

        if(recover && !mousePressed)
        {
            if (cis)
                cis.resetExternalCursor();

            recover = false;
        }

	}

    public void changeCursor()
    {
        if (buttonComp)
        {
            if (buttonComp.IsInteractable())
            {
                cis.externalTexture = cursorImage;
               
            }
        }
        else
        {
            cis.externalTexture = cursorImage;
        }

        mouseIsOverButton = true;

    }

    public void recoverCursor()
    {
        recover = true;

        mouseIsOverButton = false;
    }

    private void OnDisable()
    {
        if (cis)
            cis.resetExternalCursor();

    }

    private void OnDestroy()
    {
        if (cis)
            cis.resetExternalCursor();

    }
}
