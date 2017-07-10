using UnityEngine;
using System.Collections;


//Use this for characters that only talks. This script saves that player talk to that character.
//If player talked to that character, when he comes to that character again after changing scene, this scripts destroyes the subtitle controllers that should be destroy
//
public class SubtitleGameController : GameController, IClickAction {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public bool removeSubtitles=true;
    public bool makeInactive=false;
   
    public override void gameIsUsed()
    {
        base.gameIsUsed();
        
        

        if(removeSubtitles)
            destroyAllOneUsedSubtitleControllers();

        if (makeInactive)
            transform.tag = "Untagged";

    }

    void destroyAllOneUsedSubtitleControllers()
    {
        SubtitleController[] scs = GetComponents<SubtitleController>();
        
        foreach( SubtitleController sc in scs)
        {
            if (sc.ifDesroyItself) Destroy(sc);
        }
    }


    //public override void Action()
    //{
    //    base.Action();
    //    registerAsUsed();
    //}

}
