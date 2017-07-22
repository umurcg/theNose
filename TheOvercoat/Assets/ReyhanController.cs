using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.SceneManagement;

public class ReyhanController : GameController {

    FollowSmoothly fs;
    Animator anim;
    BasicCharAnimations bca;
    NewsGameController ngc;

    //// Update is called once per frame
    //void Update () {

    //}


    public override void Start()
    {
        base.Start();
        fs = GetComponent<FollowSmoothly>();
        anim = GetComponent<Animator>();
        bca = GetComponent<BasicCharAnimations>();
        anim.SetBool("cry",true);
    }

    IEnumerator<float> startConv()
    {

        anim.SetBool("cry", false);
        bca.enabled = true;

        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "") yield return 0;

        fs.target = CharGameController.getActiveCharacter();
        fs.enabled = true;

        ngc.deactivateController();

        registerAsUsed();

        ////Register to be persistent
        //transform.parent = CharGameController.getOwner().transform;

        //SceneManager.sceneLoaded += teleportToDoor;


    }


    //void teleportToDoor(Scene scene, LoadSceneMode mode)
    //{
    //    Debug.Log("Teleporting");


    //    GameObject door = CharGameController.cgc.getLastVisitedDoor().gameObject;
    //    transform.position = door.transform.position + door.transform.forward * 3;

    //    Time.timeScale = 0;

    //    SceneManager.sceneLoaded -= teleportToDoor;
    //}

    public override void Action()
    {
        //base.Action();
        Timing.RunCoroutine(startConv());
    }

    public override void gameIsUsed()
    {
        base.gameIsUsed();
        gameObject.SetActive(false);

        //Two condition
        //If reythan game is compleetly finished than dont disable edito
        //But if reyhan waiting at the door disable editor.
    }

    public override void activateController()
    {
        base.activateController();
        transform.tag = "ActiveObject";
        
    }

    public override void deactivateController()
    {
        base.deactivateController();
        transform.tag = "Untagged";
    }

}
