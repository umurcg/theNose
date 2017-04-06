using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;


public class NewsGameController : GameController {

    int numberOfRegister;
    Conv convType;

	// Use this for initialization
	public override void Start () {
        base.Start();

        numberOfRegister = GlobalController.Instance.countGameController(generateID());

        Debug.Log(numberOfRegister);

        //Check for girty
        GirtController gc= CharGameController.getOwner().GetComponentInChildren<GirtController>();
        if (gc != null)
        {
            setConversation(Conv.FoundGirt);

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
            //Edit: Also player can come back after give girty and go outside. In that case number of registered game cotroller should be 2 and in that case 
            //you shouldn't remove last scenes while player completed whole newspaper story.
            if (GlobalController.countSceneInList(GlobalController.Scenes.Newspaper) == 2 && numberOfRegister<2)
            {
                Debug.Log("Player still couldnt found girty");
                //If this condition is met then there is no girty but player comes to scene second time. So come on lets erase last two scenes
                GlobalController.Instance.sceneList.RemoveAt(GlobalController.Instance.sceneList.Count-1);
                GlobalController.Instance.sceneList.RemoveAt(GlobalController.Instance.sceneList.Count - 1);
                Debug.Log("I removed last scene. Now last scene is " + GlobalController.Instance.sceneList[GlobalController.Instance.sceneList.Count-1]);

                                
            }

            if (numberOfRegister == 1)
            {
                setConversation(Conv.CouldntFoundGirt);
            }
            else if (numberOfRegister == 0)
            {
                setConversation(Conv.ComignFirstTime);
            }
            else if (numberOfRegister == 2)
            {
                setConversation(Conv.NoConv);
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void callSubtitle()
    {

        if (convType == Conv.ComignFirstTime)
        {
            registerAsUsed();
        }else if (convType == Conv.FoundGirt)
        {
            GlobalController.Instance.registerGameControllerCanBeDuplicated(generateID());
        }

        sc.callSubtitle();
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

            //Set parent of girty null
            gc.gameObject.transform.parent = null;
        }
    }


    //Arrange conversation according to preference
    enum Conv {
        ComignFirstTime,
        CouldntFoundGirt,
        FoundGirt,
        NoConv

    }

    void setConversation(Conv conv)
    {
        convType = conv;
        Debug.Log("Convype is "+convType);
        switch (conv) {

            //Kovalev talks editor first time
            case Conv.ComignFirstTime:
                Destroy(GetComponents<SubtitleController>()[2]);
                Destroy(GetComponents<SubtitleController>()[3]);
                break;

            //Kovalev talks editor second time but still couldnt found girty
            case Conv.CouldntFoundGirt:
                Destroy(GetComponents<SubtitleController>()[0]);
                Destroy(GetComponents<SubtitleController>()[2]);
                Destroy(GetComponents<SubtitleController>()[3]);
                break;

            //Founrd girty
            case Conv.FoundGirt:
                Destroy(GetComponents<SubtitleController>()[0]);
                Destroy(GetComponents<SubtitleController>()[1]);
                break;



            case Conv.NoConv:
                Destroy(GetComponents<SubtitleController>()[0]);
                Destroy(GetComponents<SubtitleController>()[1]);
                Destroy(GetComponents<SubtitleController>()[2]);

                break;

                    }

    }

}
