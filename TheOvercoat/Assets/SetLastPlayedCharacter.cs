using UnityEngine;
using System.Collections;

public class SetLastPlayedCharacter : MonoBehaviour {

    public TextAsset characters;

    Vector3 relativePos;
    GameObject charObj;
    // Use this for initialization
    void Start () {
        if (GlobalController.Instance.sceneList.Count == 0) return;
        string character = characters.text.Split('\n')[GlobalController.Instance.sceneList[GlobalController.Instance.sceneList.Count - 1]];
        Debug.Log("Character is " + character);
        charObj=CharGameController.setCharacter(character.Trim());
        relativePos = transform.position - charObj.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (charObj == null) charObj = CharGameController.getActiveCharacter();

        transform.position = charObj.transform.position + relativePos;
	
	}
}
