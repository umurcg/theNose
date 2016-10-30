using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SubtitleController : MonoBehaviour {
    public string[] subtitleTexts;
    public string AllSubtitles;

    public GameObject subtitle;
    Text text;
    

	PlayerComponentController pcc;

    public bool ifDesroyItself = true;
    int index;


    // Use this for initialization
    void Start () {
        text = subtitle.GetComponent<Text>();
        if (text == null)
        {
            print("PUT THE SUBTITLE YOU FAGGOT");
        }
        index = -1;

		pcc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentController>();

    }

    public void AllToArray()
    {
        string[] subts = AllSubtitles.Split('\n');

        string[] z = new string[subtitleTexts.Length+subts.Length];
        subtitleTexts.CopyTo(z, 0);
        subts.CopyTo(z, subtitleTexts.Length);
        subtitleTexts = z;
        


    }

    public void ArrayToAll()
    {

        foreach (string s in subtitleTexts)
        {
            AllSubtitles = AllSubtitles + s + "\n";

        }

    }

    // Update is called once per frame
    void Update () {
      //  print("index: "+index+"array: "+subtitleTexts.Length);

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
                ClickTrigger.disabled = false;
    
                text.text = "";
                if(pcc!=null)
                pcc.ContinueToWalk ();



                ISubtitleFinishFunction sff = GetComponent<ISubtitleFinishFunction>();
                if (sff != null)
                {
                    //print("finishFunct");
                    sff.finishFunction();
                }
                    

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
        if (pcc != null)
            pcc.StopToWalk ();

        ClickTrigger.disabled = true;
    }


}
