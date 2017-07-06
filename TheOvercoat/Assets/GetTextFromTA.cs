using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent (typeof(Text))]
public class GetTextFromTA : MonoBehaviour {

    public static List<GetTextFromTA> allTexts;

    public TextAsset textAsset;
    Text t;

    private void OnEnable()
    {
        if (allTexts == null) allTexts = new List<GetTextFromTA>();
        allTexts.Add(this);
    }

    private void OnDisable()
    {
        allTexts.Remove(this);
    }

    // Use this for initialization
    void Awake () {

        t = GetComponent<Text>();
        getString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void getString()
    {
        t.text = Vckrs.getStringAccordingToLanguage((Language)GlobalController.Instance.getLangueSetting(), textAsset);
    }
}
