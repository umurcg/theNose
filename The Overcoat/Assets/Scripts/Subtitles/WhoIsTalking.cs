using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//To Do
//Handle same start letter.

public class WhoIsTalking : MonoBehaviour {
    public Dictionary<string, GameObject> gameObjects;
    public GameObject[] gameobjectsArray;
    Text text;
    GameObject baloon;
   // public GameObject cameraGo;
    Camera camera;

	// Use this for initialization
	void Start () {

        gameObjects = new Dictionary<string, GameObject>();
        text = GetComponent<Text>();
        if(gameobjectsArray.Length>0)
        foreach (GameObject go in gameobjectsArray)
        {
           
            gameObjects.Add(go.name.Replace(" ",string.Empty), go);
        }
        baloon = transform.GetChild(0).gameObject;
        baloon.SetActive(false);
		camera = Camera.main;
    
    }
	
	// Update is called once per frame
	void Update () {
        if (baloon == null)
        {
            baloon = transform.GetChild(0).gameObject;

        }

		if (text.text == "") {
			baloon.SetActive (false);

		} else if (text.text == "???") {
			//TO DO ???
		} else
        {
            string key = text.text.Split(':')[0].Replace(" ", string.Empty).Replace("-", string.Empty);

            if (gameObjects.ContainsKey(key)) {
                baloon.SetActive(true);
                Vector2 ActualPosition = camera.WorldToScreenPoint(gameObjects[key].transform.position);
                Vector2 newPosition = new Vector2(ActualPosition.x + Screen.width / 32, ActualPosition.y + Screen.height / 16);
                baloon.transform.position = newPosition;
            }
            else
            {
                print(key + " isn't included in dictionary.");
            }
        }

	}
}
