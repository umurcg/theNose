using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof(Dropdown))]
public class DropdownLanguageController : DynamicLanguageTexts {

    public int[] indexForDPElements;


    Dropdown dp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void Awake()
    {
        dp = GetComponent<Dropdown>();
    }

    protected override void updateText()
    {
        for (int i = 0; i < indexForDPElements.Length; i++) {

            if (i >= indexForDPElements.Length) continue;

            string[] section = extractTextFromID(indexForDPElements[i]);
            if (section == null) return;

            if (GlobalController.Instance == null) return;

            string text = findText(section, GlobalController.Instance.getLangueSetting());
            if (text == null) return;

            dp.options[i].text = text;
        }
    }
}
