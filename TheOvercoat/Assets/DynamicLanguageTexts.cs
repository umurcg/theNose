using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//This script gets language and change text according to that language.
public class DynamicLanguageTexts : MonoBehaviour {

    //Hold them so if user change the language you will update all of them easily
    public static List<DynamicLanguageTexts> currentVisibleTexts;

    static string finishSignature = "-----";
    public TextAsset textAsset;
    
    public int textID;
    Text textComp;

    private void OnEnable()
    {
        if (currentVisibleTexts == null) currentVisibleTexts = new List<DynamicLanguageTexts>();
        currentVisibleTexts.Add(this);
    }

    private void OnDisable()
    {
        if (currentVisibleTexts != null && currentVisibleTexts.Contains(this)) currentVisibleTexts.Remove(this);
    }

    void Awake()
    {
        textComp = GetComponent<Text>();

        //If can't find text searchg it on children and assign first found component
        if (!textComp) textComp = GetComponentInChildren<Text>();

    }

	// Use this for initialization
	void Start () {
        updateText();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void updateText()
    {
        string[] section= extractTextFromID(textID);
        if (section == null) return;

        string text = findText(section, GlobalController.Instance.getLangueSetting());
        if (text == null) return;

        textComp.text = text;
    }

    public static void updateAllCurrentVisibleTexts()
    {
        if (currentVisibleTexts == null) return;

        Debug.Log("Update all current visible texts and dlt length 's "+currentVisibleTexts.Count);

        foreach(DynamicLanguageTexts dlt in currentVisibleTexts)
        {
            Debug.Log("Updating " + dlt.gameObject.name);
            dlt.updateText();
        }
    }

    string findText(string[] section, GlobalController.Language lan)
    {
            foreach(string s in section)
             {
                //Debug.Log(s.Substring(0, lan.ToString().Length));
                if (lan.ToString() == s.Substring(0, lan.ToString().Length))
                {
                    //Debug.Log(s);
                    return s.Substring(lan.ToString().Length+1, s.Length-lan.ToString().Length-1);
                }
            }
        return null;
    }

    //Returns line of input string if it is exist
    string[] extractTextFromID(int id)
    {
        string wholeFile = textAsset.text;
        string[] lines = wholeFile.Split('\n');

        string line = id.ToString();

        int idLine = 0;
        int finishLine = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            if (string.Compare(line, lines[i].TrimEnd()) == 0)
            {
                //Found ID
                idLine = i;
            }
        }

        for(int i=idLine; i < lines.Length; i++)
        {
            if (string.Compare(finishSignature, lines[i].TrimEnd()) == 0)
            {
                //Found finish
                finishLine = i;
                break;
            }
        }

        //Debug.Log("idline " + idLine + " finishLine " + finishLine);

        idLine += 1;
        finishLine -= 1;

        if (finishLine != -1 && idLine < finishLine)
        {
            string[] wholeText = new string[finishLine - idLine + 1];
            for(int i = 0; i < wholeText.Length; i++)
            {
                wholeText[i] = lines[idLine + i];
                //Debug.Log(wholeText[i]);
            }
            return wholeText;
        }

        Debug.Log("Coduln't fine line " + line + " in text asset");
        return null;
    }

}
