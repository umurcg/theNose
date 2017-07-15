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

    public Dictionary<string, List<GameObject>> characters;
    public GameObject[] gameobjectsArray;
    Text text;
    GameObject baloon;
    public bool printLog = false;
    // public GameObject cameraGo;
    Camera cameraComponent;

    GameObject player;


    private void Awake()
    {
 
    }

    // Use this for initialization
    void Start()
    {
        if (self != null)
        {
            Destroy(this);
        }
        else
        {
            //Debug.Log("Creating new static object");
            self = this;
        }

        characters = new Dictionary<string, List<GameObject>>();
        text = GetComponent<Text>();
        if (gameobjectsArray.Length > 0)
            foreach (GameObject go in gameobjectsArray)
            {
                if (go != null)
                {
                    addCharacterToDict(go, go.name);
                    //characters.Add(go.name, go);
                }
            }




        //Add player character to dictionary
        player = CharGameController.getActiveCharacter();
        if (player)
        {
            if(!characters.ContainsKey(player.name))
            characters.Add(player.name,new List<GameObject>() { player });


        }else
        {
            Debug.Log("Player is null");
        }

        baloon = transform.GetChild(0).gameObject;
        baloon.SetActive(false);
        cameraComponent = Camera.main;

    }



    // Update is called once per frame
    void Update()
    {
        //foreach(KeyValuePair<string,GameObject> obj in characters)
        //{
        //    Debug.Log(obj.Key + "" + obj.Value.name);
        //}

        if (baloon == null)
        {
            baloon = transform.GetChild(0).gameObject;

        }

        if (text.text == "")
        {

         
            if(baloon.activeSelf) baloon.SetActive(false);
            mumbling(null);

        }
        else
        {
            string key = text.text.Split(':')[0].Replace("-", string.Empty);
            //Debug.Log("talking: " + key);

            //I dont know :(
            if (characters == null) return;

            if (characters.ContainsKey(key))
            {

                mumbling(getCharacter(key));

                if (cameraComponent == null) return;

                if (baloon.activeSelf == false) baloon.SetActive(true);

                Vector2 ActualPosition = cameraComponent.WorldToScreenPoint(getCharacter(key).transform.position);
                Vector2 newPosition = new Vector2(ActualPosition.x + Screen.width / 32, ActualPosition.y + Screen.height / 16);
                baloon.transform.position = newPosition;
            }
            else
            {


                if (baloon.activeSelf)  baloon.SetActive(false);
                mumbling(null);



            }
        }

        if (printLog)
        {
            foreach (KeyValuePair<string, List<GameObject>> entry in characters)
            {
                Debug.Log(entry.Key);
                printLog = false;
            }
        }


    }

    GameObject lastMumbledPerson;

    void mumbling(GameObject mumbleOwner)
    {
        if(mumbleOwner==null)
        {
            ConversationAudio.deactivateAudioConv();
            lastMumbledPerson = null;
            return;
        }

        if (lastMumbledPerson != null && lastMumbledPerson != mumbleOwner) ConversationAudio.deactivateAudioConv();        

        //Activate mumble sound
        ConversationAudio ca = mumbleOwner.GetComponent<ConversationAudio>();
        if (ca)
        {
            ca.activateAudioConv();
            lastMumbledPerson = ca.gameObject;
        }

    }

    public void addCharacterToDict(GameObject obj, string Name)
    {
        //Debug.Log("Trying to register " + Name + " for " + obj.name);
        List<GameObject> list = null;

        if (!characters.ContainsKey(Name))
        {
            //Debug.Log("Creaating new list for " + obj.name);
            list = new List<GameObject>();

        }
        else
        {
            //Debug.Log("Addint character to existing list for " + obj.name);
            list = characters[Name];
        }

        if (!list.Contains(obj)) list.Add(obj);


        characters[Name] = list;

        //if (!characters.ContainsKey(name) && !characters.ContainsValue(obj))
        //{
        //    characters.Add(Name, obj);
        //    //Debug.Log("Registering " + obj.name + " to subtittle list");
        //} else if(characters.ContainsKey(name) && !characters.ContainsValue(obj))
        //{
        //    Debug.Log("Replecaed " + obj.name);
        //    characters[name] = obj;
        //}
    }

    public void addCharacterToDict(string Name,GameObject obj)
    {
        addCharacterToDict(obj, Name);
    }

    public void setCameraComponent(Camera cam)
    {
        cameraComponent = cam;
    }


    //Gets character game object from dictionary. If key has value list having only one elemnt it returns it.
    //Else it calculates nearest character to player and returns it.
    public GameObject getCharacter(string key)
    {
        if (!characters.ContainsKey(key) || characters[key].Count==0) return null;

        if (characters[key].Count == 1) return characters[key][0];

        float minDistance = Mathf.Infinity;
        GameObject nearestChar = null;

        List<GameObject> value = characters[key];

        foreach (GameObject obj in value)
        {
            float dist = Vector3.Distance(player.transform.position, obj.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearestChar = obj;
            }
            
        }

        return nearestChar;

    }

    //Returns whole character array if key is exists.
    public List<GameObject> getAllCharacter(string key)
    {
        if (!characters.ContainsKey(key)) return null;

        return characters[key];
    }

    public void removeCharacter(string key, GameObject obj)
    {
        if (!characters.ContainsKey(key)) return;

        List<GameObject> objs = characters[key];

        if (objs.Contains(obj)) characters[key].Remove(obj);
    }

}
