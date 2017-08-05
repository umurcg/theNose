using UnityEngine;
using System.Collections;

public class OsmanBeyGameController : GameController {

    public GameObject efe;

	// Use this for initialization
	public override void Start () {
	    base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void talkWithEfe()
    {
        sc.callSubtitleWithIndex(0);
        registerAsUsed();
        deactivateController();
    }

    public override void gameIsUsed()
    {
        base.gameIsUsed();
        deactivateController();
    }

    public override void deactivateController()
    {
        base.deactivateController();
        efe.tag = "Untagged";
    }
}
