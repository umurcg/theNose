using UnityEngine;
using System.Collections;


//Use this for characters that only talks. This script saves that player talk to that character.
//If player talked to that character, when he comes to that character again after changing scene, this scripts destroyes the subtitle controllers that should be destroy
//
public class SubtitleGameController : GameController {

	//// Use this for initialization
	//void Start () {
	
	//}
	
	//// Update is called once per frame
	//void Update () {
	
	//}

    public override void gameIsUsed()
    {
        base.gameIsUsed();
        destroyAllOneUsedSubtitleControllers();

    }

    void destroyAllOneUsedSubtitleControllers()
    {
        SubtitleController[] scs = GetComponents<SubtitleController>();
        
        foreach( SubtitleController sc in scs)
        {
            if (sc.ifDesroyItself) Destroy(sc);
        }
    }

}
