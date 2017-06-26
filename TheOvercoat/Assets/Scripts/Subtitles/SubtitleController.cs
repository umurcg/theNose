using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class SubtitleController : MonoBehaviour {

    
    static string folderName = "Subtitles";
    static string finishSignature = "-----";
    string fileName;

    public TextAsset textAsset;

    public string[] subtitleTexts;
    public string AllSubtitles;


    protected Text text;
    
	PlayerComponentController pcc;

    public bool ifDesroyItself = false;
    public bool releaseAfterSub = false;
    protected int index;

    public virtual void Awake()
    {
        //If text asset is not assigned then find it atuomatically using file name generator
        if (textAsset == null) assignTextAsset();
        //subtitleTexts = AllSubtitles.Split('\n');

    }

    // Use this for initialization
    public virtual void Start () {

        //At start import texts from text asset
        if (textAsset != null) importFromTextFile();

        text = SubtitleFade.subtitles["CharacterSubtitle"];

        //If couldnt find subtitle with SubtitleFade get it with tag
        if (text == null)
        {
            text=getCharSubt();
            Debug.Log("Getting char subtitle with tag");
        }

        if (text == null)
        {
            Debug.Log("No text here");
         
        }
        index = -1;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player!=null)
        pcc = player.GetComponent<PlayerComponentController>();

        //Update funct'on shouldn't while subtitle is not active
        this.enabled = false;



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
    protected virtual void Update () {
        //  print("index: "+index+"array: "+subtitleTexts.Length);

        if (Time.timeScale == 0) return;

        if (index != -1)
        {
            
            if (index < subtitleTexts.Length)
            {
                
                if (Input.GetMouseButtonDown(0) || Input.GetAxis("InteractionKeyboard")==1)
                {
                    //Debug.Log("You pressed to mouse");
                    index++;
                                    
                    if (index < subtitleTexts.Length)
                        text.text = subtitleTexts[index];
                }
            }
            
            else
            {
                //Debug.Log("index is exceeds subtitle length");
                ClickTrigger.disabled = false;

                text.text = "";

                if(pcc!=null&&releaseAfterSub)
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
                this.enabled = false;
            }
        }
        else
        {
            Debug.Log("index is -1");
        }

	}


   public virtual void startSubtitle()
    {
        
        if (text == null) Debug.Log("No text");
        //Debug.Log(subtitleTexts.Length);

        if (text == null)
        {
            text = SubtitleFade.subtitles["CharacterSubtitle"];
        }

        text.fontStyle = FontStyle.Normal;

        //if (text == null) Debug.Log("No text again");


        //If subtitle text is not null dont start subtitle.
        if (text.text != "") return;

        //Debug.Log("Subtitle is started");

        //Debug.Log(subtitleTexts.Length);

        text.text = subtitleTexts[0];
        index = 0;
        if (pcc != null)
        //{
            pcc.StopToWalk();
        //}else
        //{
        //    Debug.Log("PCC is null");
        //}
        ClickTrigger.disabled = true;
        this.enabled = true;

        //Debug.Log("enabled subtitile");
    }


    public Text getCharSubt()
    {
        GameObject subObj = GameObject.FindGameObjectWithTag("CharacterSubtitle");
        if (subObj != null) return subObj.GetComponent<Text>();
        return null;

    }

    public Text getNarSubt ()
    {
        GameObject subObj = GameObject.FindGameObjectWithTag("NarratorSubtitle");
        if (subObj != null) return subObj.GetComponent<Text>();
        return null;


    }

    public void updatePCC()
    {
        pcc = CharGameController.getActiveCharacter().GetComponent<PlayerComponentController>();
    }

    public int getCurrentIndex()
    {
        return index;
    }


    public string[] textFileToStringArray(TextAsset ta)
    {
        string allText = ta.text;
        string[] lines=allText.Split('\n');

        //foreach (string line in lines) Debug.Log(line);

        return lines;
    }

    void createFileName()
    {
        SubtitleController[] allControllers = gameObject.GetComponents<SubtitleController>();
        int scriptIndex=-1;
        for (int i = 0; i < allControllers.Length; i++)
        {
            if (allControllers[i] == this) scriptIndex = i;
        }

        fileName = SceneManager.GetActiveScene().name + "_" + gameObject.name + "_" + scriptIndex/*+".txt"*/;
        //Debug.Log(fileName);
    }

    //This method exports current subtitletexts to file that is generated by fileName generator
    //While text asset based module developed after a point, it is written for backing up current subtitles which is assigned to subtitleTexts
    //by hand. 
    //It exports subtitles as TR while all subtitles that is assigned to subtitleTexts by hand is turkish.
    public void exportToTextFile()
    {
        string directory = Application.dataPath + "/Resources/" + folderName + "/" + fileName;
        //string directory = "Assets/" + folderName + "/" + fileName;
        var writer = File.CreateText(directory);
        string[] allLines = subtitleTexts;

        writer.Write("TR\n");
        foreach(string line in allLines)
        {

            writer.Write(line+'\n');
        }

        //For finish
        writer.Write(finishSignature);

        writer.Close();

        Debug.Log("Subtitle is exported as " + directory);

    }

    //Imports subtitle text according to language setting
    public void importFromTextFile() {


        string languageCode = GlobalController.Instance.getLangueSetting().ToString();
        
        //Debug.Log(languageCode);

        int startIndex = findLine(languageCode)+1;
        int finishIndex = findLine(startIndex,finishSignature)-1;


        //If text asset doesn't have text for languge settings that user set, then import turkish subtitles by default. Because 
        // I am sure there will be turkish subtitle.
        if (startIndex > finishIndex || startIndex==0)
        {
            //Debug.Log("Couldn't find subtite for " + languageCode + " in "+ fileName+" so importing turkish");
            startIndex = findLine("TR") + 1;
            finishIndex = findLine(startIndex, finishSignature) - 1;

            //If stille couldn't find any subtitle from text asset then return and don't do aything
            if (startIndex > finishIndex) return;
        }


        //Debug.Log("start index is " + startIndex + " finish index is" + finishIndex);

        subtitleTexts = readLinesFromTA(startIndex, finishIndex, textAsset);

    }

    //Read lines from text asset with starting fromt startIndex and finishing at finishIndex lines.
    public string[] readLinesFromTA(int startIndex, int finishIndex, TextAsset ta)
    {
        string[] allLines = textFileToStringArray(ta);
        string[] rangeLines = new string[finishIndex - startIndex + 1];

        for (int i = 0; i < rangeLines.Length; i++)
        {
            rangeLines[i] = allLines[startIndex + i];
            //Debug.Log(rangeLines[i]);
        }

        return rangeLines;

    }

    //Finds and assign text asset from folder name an fenerated file name.
    public void assignTextAsset()
    {

        //Debug.Log("Assigning text file");
        createFileName();



        //textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/" + folderName + "/" + fileName, typeof(TextAsset)) as TextAsset;
        textAsset = Resources.Load(folderName + "/" + fileName, typeof(TextAsset)) as TextAsset;
        

        //If text asset isn't found then export current text to file if it is not null 
        if (textAsset == null && subtitleTexts.Length > 0)
        {
            Debug.Log("Couldn't found text asset. So exporting current subtitleTets to text asset and assigning to again");
            exportToTextFile();
            //Try to assign again after creating it
            textAsset = Resources.Load(folderName + "/" + fileName, typeof(TextAsset)) as TextAsset;

            if (!textAsset) Debug.Log("FUUUUUUUUUUUUUUUCK" +  folderName + " / " + fileName);

        }
        else {
            //Debug.Log("Foun text");
        }

    }


    //Returns line of input string if it is exist
    int findLine(string line)
    {

        //StreamReader theReader = new StreamReader(Application.dataPath + "/Resources/" + folderName + "/" + fileName+".txt");

        string wholeFile =/* theReader.ReadToEnd();*/ textAsset.text;
        string[] lines = wholeFile.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            if (string.Compare(line, lines[i].TrimEnd()) == 0)
            {
                return i;
            }
        }

        //Debug.Log("Coduln2t fine line " + line + " in text asset");
        return -1;
    }

    //Returns line of input string if it is exist, starting to search after startIndex.
    int findLine(int startIndex,string line)
    {

        //StreamReader theReader = new StreamReader(Application.dataPath + "/Resources/" + folderName + "/" + fileName + ".txt");

        string wholeFile =/* theReader.ReadToEnd();*/ textAsset.text;
        string[] lines = wholeFile.Split('\n');
        string[] sublines = new string[lines.Length - startIndex];

        for (int i = 0; i < sublines.Length; i++)
        {

            sublines[i] = lines[startIndex + i];
            //Debug.Log(sublines[i].Length+ " line is "+line.Length);
            //Debug.Log(string.Compare(line, sublines[i].TrimEnd()));

            if (string.Compare(line,sublines[i].TrimEnd())==0)
            {
                //Debug.Log("Returned " + (startIndex+ i));
                return startIndex+i;
               
            }
        }       

        return -1;
    }



}
