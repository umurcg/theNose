using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;


public class NewsGameController : GameController {

    

	// Use this for initialization
	public override void Start () {
        base.Start();
        //Check for girty
        GirtController gc= CharGameController.getOwner().GetComponentInChildren<GirtController>();
        if (gc != null)
        {
            //Debug.Log("Found girt");
            Destroy(GetComponents < SubtitleController >()[0]);
            Destroy(GetComponents<SubtitleController>()[1]);

            //Enable girty. By default charcontroller disables every character excepts main at awake
            gc.gameObject.SetActive(true);
            //Set girtys position
            gc.transform.position = player.transform.position - Vector3.right + Vector3.up;

        }
        else
        {
            //There is no girty, but player maybe come back second time for cruisity. In this case global controller will think
            //player is going right direction. But he is not. So we should erase last save scene to sceneList manually from here if 
            //Player coming from second time. Fuck it was hard to notice.
            if (GlobalController.countSceneInList(GlobalController.Scenes.Newspaper) == 2)
            {
                Debug.Log("Player still couldnt found girty");
                //If this condition is met then there is no girty but player comes to scene second time. So come on lets erase last two scenes
                GlobalController.Instance.sceneList.RemoveAt(GlobalController.Instance.sceneList.Count-1);
                GlobalController.Instance.sceneList.RemoveAt(GlobalController.Instance.sceneList.Count - 1);
                Debug.Log("I removed last scene. Now last scene is " + GlobalController.Instance.sceneList[GlobalController.Instance.sceneList.Count-1]);
                //Also lets assume player triggered first subtitle. Maybe he didnt but for now i dont know how to handle that
                //TODO track subtitle that are triggered.
                Destroy(GetComponents<SubtitleController>()[0]);
            }

            Destroy(GetComponents<SubtitleController>()[2]);
            Destroy(GetComponents<SubtitleController>()[3]);
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Kovalev handles girty to newspaper
    public void takeGirty()
    {
        //If girty exists
        GirtController gc = CharGameController.getOwner().GetComponentInChildren<GirtController>();
        if (gc != null)
        {
            girtyBeFriendsScript gbfs = gc.gameObject.GetComponent<girtyBeFriendsScript>();
            if (gbfs != null) gbfs.enabled = false;
        }
    }

}
