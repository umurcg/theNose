using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//This scripts shows letters one by one when user types anthing
public class LetterController : MonoBehaviour {

    public enum fillType {KeyboardInput, Time };
    public fillType FillType;

    Image[] lines;
    int currentLineIndex=0;

    public float fillSpeed = 1f;
    public GameObject messageReciever;
    public string message;


	// Use this for initialization
	void Start () {
        //Get children
        lines = new Image[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            lines[i] = transform.GetChild(i).GetComponent<Image>();
        }
        //Debug.Log(lines.Length);
        
	}
	
	// Update is called once per frame
	void Update () {
        if (currentLineIndex >= lines.Length) finishedLetter();

        if ((FillType==fillType.KeyboardInput&& Input.anyKeyDown) || (FillType == fillType.Time)) {
            Image currentLine = lines[currentLineIndex];
            currentLine.fillAmount += Time.deltaTime * fillSpeed;

            if (currentLine.fillAmount>=1)
            {
                currentLine.fillAmount = 1;
                currentLineIndex++;
            }
        }   

	}

    void finishedLetter()
    {
        Debug.Log("finished letter");
        if (messageReciever !=null) 
        messageReciever.SendMessage(message);
        enabled = false;

    }
}
