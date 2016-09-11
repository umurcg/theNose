using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NonInteractiveSubtitle : MonoBehaviour {
  // public TextAsset tAssest;
   public bool isBusy = false;
    string[] text;
    float[] duration;
    public TextAsset[] TextAssets;

    Text Subtitle;

    

    //bool change = false;
    float timer;
    int index=0;

    // Use this for initialization
    void Awake () {
        // setSbt(Subtitle);
        Subtitle = this.GetComponent<Text>();
        isBusy = false;

	}
    
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(text.Length);
        if (isBusy)
        {
            timer -= Time.deltaTime;
            if (timer <= 0&&index+1<text.Length)
            {
                
                Subtitle.text = text[index];
                timer = duration[index];
                index++;
            }
            else if(timer<=0&&index+1==text.Length)
            {
                index = 0;
                timer = 0;
                isBusy = false;
                Subtitle.text = "";
            }

        }

	}

   void setSubt(TextAsset ta)
    {
       
       string[] textArray = ta.text.Split('\n');
        text = new string[textArray.Length];
        duration = new float[textArray.Length];
        for(int i = 0; i < textArray.Length; i++)
        {
            string[] str= textArray[i].Split('-');
            if (str.Length == 2) {

                text[i] = str[0];
                duration[i] = float.Parse(str[1]);
                //    Debug.Log(text[i] + " " + duration[i]);
              //  Debug.Log(textArray.Length);
               
        } 
       }


        isBusy = true;
        index = 0;

    }


    public void setSubtFromOutside(int tAssetIndex)
    {
        if (tAssetIndex < TextAssets.Length)
        {
            setSubt(TextAssets[tAssetIndex]);
        }
    }

}
