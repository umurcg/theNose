using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class dotdotdot : MonoBehaviour {
    public float dotSpeed=0.3f;
    int dotNumber=0;
    Text text;
    float timer;
    // Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;
        if (timer > dotSpeed)
        {
            if (dotNumber <3)
            {
                dotNumber++;
                timer = 0;
            }
            else
            {
                dotNumber = 0;
            }
            
        }

        switch (dotNumber)
        {
            case 0:
                text.text = "";
                break;
            case 1:
                text.text = ".";
                break;
            case 2:
                text.text="..";
                break;
            case 3:
                text.text = "...";
                break;
        }


	}
}
