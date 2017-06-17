using UnityEngine;
using System.Collections;

public class SaveTest : GameController, IClickAction{


	// Update is called once per frame
	void Update () {
	
	}

    public override void Action()
    {
        if (!isUsed())
        {
            sc.callSubtitleWithIndex(0);
            registerAsUsed();

        }else
        {
            sc.callSubtitleWithIndex(1);
            
        }
    }

}
