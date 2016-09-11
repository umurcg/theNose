using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WhoIsTalking : MonoBehaviour {
    public Dictionary<char, GameObject> gameObjects;
    public GameObject[] gameobjectsArray;
    Text text;
    GameObject baloon;
   // public GameObject cameraGo;
    Camera camera;

	// Use this for initialization
	void Start () {

        gameObjects = new Dictionary<char, GameObject>();
        text = GetComponent<Text>();
        if(gameobjectsArray.Length>0)
        foreach (GameObject go in gameobjectsArray)
        {
       
            gameObjects.Add(go.name[0], go);
        }
        baloon = transform.GetChild(0).gameObject;
        baloon.SetActive(false);
		camera = Camera.main;
    
    }
	
	// Update is called once per frame
	void Update () {
        if (text.text == "")
        {
            baloon.SetActive(false);

        }
        else
        {
            baloon.SetActive(true);
            Vector2 ActualPosition =  camera.WorldToScreenPoint(gameObjects[text.text[0]].transform.position);
            Vector2 newPosition = new Vector2(ActualPosition.x + Screen.width / 32, ActualPosition.y + Screen.height / 16);
            baloon.transform.position =newPosition;
        }

	}
}
