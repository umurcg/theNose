using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//This script enable payer to write latter with keyboardd or letter writes it self with time
public class LetterControllerV2 : MonoBehaviour {


    public enum fillType { KeyboardInput, Time };
    public fillType FillType;
    string letter;
    Text text;


    // Use this for initialization
    void Start () {
        text = GetComponentInChildren<Text>();
        letter = text.text;
        text.text = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
