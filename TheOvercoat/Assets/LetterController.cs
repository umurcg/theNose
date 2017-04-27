using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//This scripts shows letters one by one when user types anthing
//TODO set initial positions according to letter width and height
//TODO Look to gaps
public class LetterController : MonoBehaviour {

    public enum fillType {KeyboardInput, Time };
    public fillType FillType;

    public int numberOfLines;
    public GameObject lineObject;

    public float linePivotYratio,linePivotXratio;

    public List<int> skipLines=new List<int>();

    Image[] lines;
    int currentLineIndex=0;

    public float fillSpeed = 1f;
    public GameObject messageReciever;
    public string message;

    RectTransform rt;


    // Use this for initialization
    void Start () {
                
        rt =  GetComponent<RectTransform>();
        lines = new Image[numberOfLines];
        int numberOfSkip = 0;

        for (int i = 0; i < numberOfLines; i++)
        {
           if( skipLines.Contains(i)) numberOfSkip++;

            GameObject spawnedObj=Instantiate(lineObject) as GameObject;
            spawnedObj.SetActive(true);
            spawnedObj.transform.parent = transform;

            RectTransform srt = spawnedObj.GetComponent<RectTransform>();
            float height = srt.sizeDelta.y;

            float x = linePivotXratio*Screen.width + rt.sizeDelta.x / 2;
            float y = linePivotYratio*Screen.height + rt.sizeDelta.y - (i+numberOfSkip )* height;

            //Debug.Log( x.ToString()+" "+ y.ToString());

            srt.position = new Vector3(x,y);

            lines[i] = spawnedObj.GetComponent<Image>();
            
        }

        ////Get children
        //lines = new Image[transform.childCount];
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    lines[i] = transform.GetChild(i).GetComponent<Image>();
        //}
        //Debug.Log(lines.Length);
        
	}
	
	// Update is called once per frame
	void Update () {
        if (currentLineIndex >= lines.Length)
        {
            finishedLetter();
            return;
        }

        if ((FillType==fillType.KeyboardInput&& Input.anyKeyDown) || (FillType == fillType.Time)) {
            Debug.Log("pressed");
            Image currentLine = lines[currentLineIndex];
            currentLine.fillAmount -= Time.deltaTime * fillSpeed;

            if (currentLine.fillAmount<=0)
            {
                currentLine.fillAmount = 0;
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
