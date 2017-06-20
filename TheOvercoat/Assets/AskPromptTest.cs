using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AskPromptTest : MonoBehaviour {

    public GameObject prefab;

	// Use this for initialization
	void Start () {

        GameObject prompt=Instantiate(prefab) as GameObject;
        //prompt.transform.position = Vector3.zero;
        //prompt.transform.parent = null;
        //prompt.transform.localScale = Vector3.one;
        //prompt.transform.parent = transform.parent;

        //AskPrompt script= prompt.GetComponent<AskPrompt>();
        //script.yesButton.GetComponent<Button>().onClick.AddListener(yes);
        //script.noButton.GetComponent<Button>().onClick.AddListener(no);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void yes()
    {
        Debug.Log("yes");
    }

    void no()
    {
        Debug.Log("No");
    }


}
