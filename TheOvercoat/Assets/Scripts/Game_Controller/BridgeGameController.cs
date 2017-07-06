using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class BridgeGameController : GameController {

    public GameObject police,trigger,/*blackScreen,*/ shallNotPass, singerCafe, nehir;

    //bool onBridge = false;
    GameObject nose;
    BoxCollider bc;
    Rigidbody noseRB;
    NavMeshAgent policeNma;

    bool disabled;
    //public bool activate;

    CameraController cc;

	// Use this for initialization
	public override void Start () {
        base.Start();


        if (player == null)
        {
            this.enabled = false;
            return;
        }


        if (player.name == null)
        {
            this.enabled = false;
            return;
        }

        if (player.name != "Ivan")
        {
            this.enabled = false;
            return;
        }



        bc =GetComponent<BoxCollider>();

        policeNma = police.GetComponent<NavMeshAgent>();

        //Vckrs.testPosition(new Vector3(bc.bounds.center.x, player.transform.position.y, player.transform.position.z + 5));


    }
	
	// Update is called once per frame
	void Update () {
	  
	}

    public void throwNose()
    {
        //if (activate)
        //{
        //    activateController();
        //    activate = false;
        //}

        //Debug.Log("ThrowNose");
        if (disabled) return;
        if (player == null)
        {
            Debug.Log("No player");
            return;
        }

        if (player.name != "Ivan")
        {
            Debug.Log("Player name is not Ivan");
            this.enabled = false;
            return;
        }

        //if (onBridge)
        //{
            //nose = player.transform.Find("ivan/Armature/Torso/Chest/Arm_L/Hand_L/nosePackage").gameObject;
            nose = CharGameController.getObjectOfHand("nosePackage",CharGameController.hand.LeftHand);
            if(nose == null)
            {
                Debug.Log("Couldnt find nose");
                return;
            }
            noseRB = nose.GetComponent<Rigidbody>();

            Timing.RunCoroutine(_throwNose());
        //}else
        //{
        //    Debug.Log("Not on bridge");
        //}
        //if (nose == null)
        //    print("Couldn't find nose");
        //if (!onBridge)
        //    print("is not on bridge");


    }



    IEnumerator<float> _throwNose()
    {
        yield return 0;


        trigger.GetComponent<DirectClickScript>().enabled = false;
        //print("throwiiiiiiiiiiiing");
        IEnumerator<float> handler;


        pcc.StopToWalk();
        playerNma.enabled = true;
        //Finding edge point
        Vector3 edge = new Vector3(bc.bounds.max.x, player.transform.position.y, player.transform.position.z);
        playerNma.Resume();
        playerNma.SetDestination(edge);
        handler = Timing.RunCoroutine(Vckrs.waitUntilStop(player));
        yield return Timing.WaitUntilDone(handler);

       
        handler = Timing.RunCoroutine(Vckrs._lookTo(player, Vector3.right, 1f));
        yield return Timing.WaitUntilDone(handler);


        playerAnim.SetTrigger("Throw");
   
        while (playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f)
        {
             yield return 0;
        }

        nose.transform.parent = null;
        noseRB.isKinematic = false;
        noseRB.AddForce(player.transform.forward*40+transform.up*20, ForceMode.Impulse);

        yield return Timing.WaitForSeconds(1.5f);
        sc.callSubtitleWithIndex(0);
        while (subtitle.text !="")
        {
            yield return 0;
        }


        yield return Timing.WaitForSeconds(1f);
        
              
        policeNma.SetDestination(new Vector3(bc.bounds.center.x, player.transform.position.y, player.transform.position.z +  5));
        subtitle.text = "-Polis: Hey!";

        handler = Timing.RunCoroutine(Vckrs.waitUntilStop(police));
        yield return Timing.WaitUntilDone(handler);
        Timing.RunCoroutine(Vckrs._lookTo(police, player.transform.position-police.transform.position, 1));
        Timing.RunCoroutine(Vckrs._lookTo(player, police.transform.position-player.transform.position, 1));

        subtitle.text = "";
        sc.callSubtitleWithIndex(1);

        while (subtitle.text != "")
        {
            yield return 0;
        }

        playerNma.SetDestination(police.transform.position + police.transform.forward * 2);
        handler = Timing.RunCoroutine(Vckrs.waitUntilStop(player));

        Debug.Log("I am here");

        yield return Timing.WaitUntilDone(handler);

        

        while (subtitle.text != "")
        {
            yield return 0;
        }

        sc.callSubtitleWithIndex(2);

        while (subtitle.text != "")
        {
            yield return 0;
        }

        policeNma.SetDestination(police.transform.position + Vector3.forward * 50);
        yield return Timing.WaitForSeconds(0.5f);
        Timing.RunCoroutine(Vckrs.followObject(playerNma, police));
        yield return Timing.WaitForSeconds(1f);

        GameObject cam= Camera.main.gameObject;
        CameraFollower cf = cam.GetComponent<CameraFollower>();
        cf.enabled = false;

        cc = cam.GetComponent<CameraController>();

        //Timing.RunCoroutine(Vckrs._cameraSize(Camera.main, 40, 0.5f));
        cc.smoothZoomOut(40, 0.5f);
        sc.callSubtitleWithIndexTime(0);

                 
        yield return Timing.WaitForSeconds(5f);

        while (narSubtitle.text != "") yield return 0;

        //Dont load new scene instead fade out and activate singer cafe
        //GetComponent<LoadScene>().Load();

        Timing.RunCoroutine(changeScene());

        yield break;


    }

    public IEnumerator<float> changeScene()
    {
        handlerHolder = blackScreen.script.fadeOut();
        yield return Timing.WaitUntilDone(handlerHolder);

        CharGameController.getCamera().GetComponent<CameraFollower>().enabled = true;
        singerCafe.GetComponent<CafeSingerGameController>().activateController();

        handlerHolder = blackScreen.script.fadeIn();
        yield return Timing.WaitUntilDone(handlerHolder);

        Destroy(this);
    }


    void OnTriggerEnter()
    {
        //onBridge = true;
        trigger.tag = "ActiveObjectOnlyCursor";
        trigger.GetComponent<DirectClickScript>().enabled = true;
    }

    void OnTriggerExit()
    {
        //onBridge = false;
        trigger.tag = "Untagged";
        trigger.GetComponent<DirectClickScript>().enabled = false;
    }

    public override void activateController()
    {
        base.activateController();
        disabled = false;
        police.SetActive(true);
        trigger.SetActive(true);
        shallNotPass.SetActive(true);


    }
    public override void deactivateController()
    {
        base.deactivateController();
        disabled = true;
        police.SetActive(false);
        trigger.SetActive(false);
        shallNotPass.SetActive(false);

    }
}
