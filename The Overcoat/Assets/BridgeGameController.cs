using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class BridgeGameController : MonoBehaviour {

    public GameObject police,trigger,blackScreen;

    bool onBridge = false;
    GameObject player;
    GameObject nose;
    BoxCollider bc;
    NavMeshAgent playerNma;
    Rigidbody noseRB;
    Animator anim;
    SubtitleCaller sc;
    Text subtitle;
    PlayerComponentController pcc;
    NavMeshAgent policeNma;

	// Use this for initialization
	void Start () {
        player = CharGameController.getActiveCharacter();
        if (player.name != "Ivan")
        {
            this.enabled = false;
            return;
        }


        bc =GetComponent<BoxCollider>();
        playerNma = player.GetComponent<NavMeshAgent>();
        anim = player.GetComponent<Animator>();
        sc = GetComponent<SubtitleCaller>();
        subtitle = SubtitleFade.subtitles["CharacterSubtitle"];
        pcc = player.GetComponent<PlayerComponentController>();
        policeNma = police.GetComponent<NavMeshAgent>();

        //Vckrs.testPosition(new Vector3(bc.bounds.center.x, player.transform.position.y, player.transform.position.z + 5));


    }
	
	// Update is called once per frame
	void Update () {
	  
	}

    public void throwNose()
    {

        if (player.name != "Ivan")
        {
            this.enabled = false;
            return;
        }

        if (onBridge && nose != null)
        {
            nose = player.transform.Find("ivan/Armature/Torso/Chest/Arm_L/Hand_L/nosePackage").gameObject;
            noseRB = nose.GetComponent<Rigidbody>();

            Timing.RunCoroutine(_throwNose());
        }
        //if (nose == null)
        //    print("Couldn't find nose");
        //if (!onBridge)
        //    print("is not on bridge");

    }



    IEnumerator<float> _throwNose()
    {



        trigger.GetComponent<DirectClickScript>().enabled = false;
        //print("throwiiiiiiiiiiiing");
        IEnumerator<float> handler;


        pcc.StopToWalk();
        playerNma.enabled = true;
        //Finding edge point
        Vector3 edge = new Vector3(bc.bounds.max.x, player.transform.position.y, player.transform.position.z);
        playerNma.Resume();
        playerNma.SetDestination(edge);
        handler = Timing.RunCoroutine(Vckrs.waitUntilStop(player,0));
        yield return Timing.WaitUntilDone(handler);

       
        handler = Timing.RunCoroutine(Vckrs._lookTo(player, Vector3.right, 1f));
        yield return Timing.WaitUntilDone(handler);


        anim.SetTrigger("Throw");
   
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f)
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

        handler = Timing.RunCoroutine(Vckrs.waitUntilStop(police, 0));
        yield return Timing.WaitUntilDone(handler);
        Timing.RunCoroutine(Vckrs._lookTo(police, player.transform.position-police.transform.position, 1));
        Timing.RunCoroutine(Vckrs._lookTo(player, police.transform.position-player.transform.position, 1));
        sc.callSubtitleWithIndex(1);

        while (subtitle.text != "")
        {
            yield return 0;
        }

        playerNma.SetDestination(police.transform.position + police.transform.forward * 2);
        handler = Timing.RunCoroutine(Vckrs.waitUntilStop(player, 0));
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

        Timing.RunCoroutine(Vckrs._cameraSize(Camera.main, 40, 0.5f));
        sc.callSubtitleWithIndexTime(0);

        Timing.WaitForSeconds(1f);


        Timing.RunCoroutine(Vckrs._fadeInfadeOut(blackScreen, 0.1f));

    }



    void OnTriggerEnter()
    {
        onBridge = true;
    }

    void OnTriggerExit()
    {
        onBridge = false;
    }
}
