using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//To Do
//Handle same start letter.

public class WhoIsTalking : MonoBehaviour
{
    //Singleton
    public static WhoIsTalking self;

    public Dictionary<string, GameObject> characters;
    public GameObject[] gameobjectsArray;
    Text text;
    GameObject baloon;
    public bool printLog = false;
    // public GameObject cameraGo;
    Camera cameraComponent;


    // Use this for initialization
    void Start()
    {
        if (self != null)
        {
            Destroy(this);
        }
        else
        {
            self = this;
        }

        characters = new Dictionary<string, GameObject>();
        text = GetComponent<Text>();
        if (gameobjectsArray.Length > 0)
            foreach (GameObject go in gameobjectsArray)
            {
                if (go != null)
                {
                    characters.Add(go.name, go);
                }
            }

        //Add player character to dictionary
        GameObject player = CharGameController.getActiveCharacter();
        if (player)
            characters.Add(player.name, player);


        baloon = transform.GetChild(0).gameObject;
        baloon.SetActive(false);
        cameraComponent = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (baloon == null)
        {
            baloon = transform.GetChild(0).gameObject;

        }

        if (text.text == "")
        {
            baloon.SetActive(false);

        }
        else if (text.text == "???")
        {
            //TO DO ???
        }
        else
        {
            string key = text.text.Split(':')[0].Replace("-", string.Empty);
            //Debug.Log("talking: " + key);

            //I dont know :(
            if (characters == null) return;

            if (characters.ContainsKey(key))
            {
               
                baloon.SetActive(true);
                if (cameraComponent == null) return;
                Vector2 ActualPosition = cameraComponent.WorldToScreenPoint(characters[key].transform.position);
                Vector2 newPosition = new Vector2(ActualPosition.x + Screen.width / 32, ActualPosition.y + Screen.height / 16);
                baloon.transform.position = newPosition;
            }
            else
            {

                baloon.SetActive(false);


            }
        }

        if (printLog)
        {
            foreach (KeyValuePair<string, GameObject> entry in characters)
            {
                Debug.Log(entry.Key);
                printLog = false;
            }
        }


    }

    public void addCharacterToDict(GameObject obj, string Name)
    {
        if (!characters.ContainsKey(name) && !characters.ContainsValue(obj))
        {
            characters.Add(Name, obj);
        }
    }

    public void setCameraComponent(Camera cam)
    {
        cameraComponent = cam;
    }

}
