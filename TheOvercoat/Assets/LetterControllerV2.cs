using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//This script enable payer to write latter with keyboardd or letter writes it self with time
public class LetterControllerV2 : MonoBehaviour {


    public enum fillType { KeyboardInput, Time };
    public int numberOfCharacterInOneStep=1;
    public float timeOfOneStep = 1f;
    public fillType FillType;
    public TextAsset tr;
    public TextAsset eng;

    string letter;
    Text text;

    public GameObject messageReciever;
    public string message;

    Timer timer;

    int index;


    // Use this for initialization
    void Start () {
        text = GetComponentInChildren<Text>();
    
        text.text = "";

        if (FillType == fillType.Time) timer = new Timer(timeOfOneStep);

        if (GlobalController.Instance.getLangueSetting() == GlobalController.Language.ENG)
        {
            letter = eng.text;
        }
        else
        {
            letter = tr.text;
        }

	}
	
	// Update is called once per frame
	void Update () {

        if (index >= letter.Length)
        {
            sendMessage();
            return;
        }

        if (FillType == fillType.Time)
        {
            if (timer.ticTac(Time.deltaTime))
            {
                index += numberOfCharacterInOneStep;
                text.text = letter.Substring(0, index);
            }
            
        }
        else
        {
            if (Input.anyKeyDown)
            {
                index += numberOfCharacterInOneStep;
                text.text = letter.Substring(0, index);
            }
        }


    }

    [ContextMenu ("Finish letter")]
    void sendMessage()
    {
        Debug.Log("finished letter");
        if (messageReciever != null)
            messageReciever.SendMessage(message);
        enabled = false;

    }

}
