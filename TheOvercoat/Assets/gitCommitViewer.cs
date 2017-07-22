using MovementEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gitCommitViewer : GameController, IClickAction {


    public GameObject paper; 
    public Text[] columns;
    public int linePerColumn = 70;
    public TextAsset textAsset;
    string[] lines;
    
    // Use this for initialization
    public override void Start () {

        base.Start();
        lines = textAsset.text.Split('\n');

        Debug.Log("Lenght of line " + lines.Length);

        for(int i = 0; i < columns.Length; i++)
        {
            if (i * linePerColumn > lines.Length) continue;

            string wholeColumn = "";

            int numberOfLine = (i+1) * linePerColumn;


            //Debug.Log("number of line "+numberOfLine);

            for (int j = i*linePerColumn; j < numberOfLine; j++)
            {
                if (j >= lines.Length) continue;
                wholeColumn += lines[j] + '\n';
            }

            columns[i].text = wholeColumn;

        }


	}

    IEnumerator<float> showPaper()
    {
        sc.callSubtitleWithIndex(0);

        while (subtitle.text != "") yield return 0;

        paper.SetActive(true);

        while (!Input.GetMouseButtonDown(0)) yield return 0;

        paper.SetActive(false);

        pcc.ContinueToWalk();

        yield break;
    }

    public override void Action()
    {
        base.Action();
        Timing.RunCoroutine(showPaper());
    }


}
