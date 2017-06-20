using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class AskPrompt : MonoBehaviour {

    public GameObject yesButton, noButton;
    public Text promtText;
    EnableDisableUI edui;

    UnityAction[] yesActions;

    UnityAction[] noActions;

    private void Awake()
    {

        edui = GetComponent<EnableDisableUI>();
        noButton.GetComponent<Button>().onClick.AddListener(destroyPrompt);
        
    }

      

    // Use this for initialization
    void Start () {
        
   

    }
	
	// Update is called once per frame
	void Update () {

        if (yesActions != null && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Calling yes action");
            foreach (UnityAction act in yesActions)
                act();
            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            destroyPrompt();
            if (noActions!=null)
            {
                foreach (UnityAction act in noActions)
                    act();
            }
        }
        
	
	}

    public void setPromptText(string text)
    {
        promtText.text = text;
    }
    
    void destroyPrompt()
    {
        edui.deactivateAndDestroy();
    }

    public void assignYesFunctionalities(UnityAction[] functions)
    {
        foreach(UnityAction func in functions)
            yesButton.GetComponent<Button>().onClick.AddListener(func);
        yesActions = functions;

    }


    public void assignNoFunctionalities(UnityAction[] functions)
    {
        foreach (UnityAction func in functions)
            noButton.GetComponent<Button>().onClick.AddListener(func);
        noActions = functions;

    }

}
