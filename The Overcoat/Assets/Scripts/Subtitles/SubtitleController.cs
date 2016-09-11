using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubtitleController : MonoBehaviour {
    public string[] subtitleTexts;
    public GameObject subtitle;
    Text text;

	PlayerComponentController pcc;

    public bool ifDesroyItself = true;
    int index;


    // Use this for initialization
    void Start () {
        text = subtitle.GetComponent<Text>();
        index = -1;

		pcc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentController>();

    }
	
	// Update is called once per frame
	void Update () {

        if (index != -1)
        {
            if (index < subtitleTexts.Length)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    index++;
                   
                    if (index < subtitleTexts.Length)
                        text.text = subtitleTexts[index];
                }
            }
            else
            {
                text.text = "";
				pcc.ContinueToWalk ();

                if (ifDesroyItself)
                {
                    Destroy(gameObject.GetComponent<SubtitleController>());
     
                }
            }
        }

	}

   public void startSubtitle()
    {
 
        text.text = subtitleTexts[0];
        index = 0;
		pcc.StopToWalk ();

    }
}
