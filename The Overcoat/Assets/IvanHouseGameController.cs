using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class IvanHouseGameController : GameController {
    public GameObject Praskovaya, blackScreen, Bread, Nose;

    Animator praskovayaAnim;
    NavMeshAgent praskovayaNma;


	// Use this for initialization
	public override void Start () {
        base.Start();

        praskovayaAnim = Praskovaya.GetComponent<Animator>();
        praskovayaNma = Praskovaya.GetComponent<NavMeshAgent>();

        //Timing.RunCoroutine(_wakeUpScene());
        //Timing.RunCoroutine(_startBreadGame());
        Timing.RunCoroutine(_noseDrop());
    }
	
	// Update is called once per frame
	void Update () {
	   
	}

    IEnumerator<float> _wakeUpScene()
    {
        //fading in;
        handlerHolder = Timing.RunCoroutine(Vckrs._fadeInfadeOut(blackScreen,0.5f));

        //Setting animations and stop player
        praskovayaAnim.SetBool("Hands", true);
        playerAnim.SetBool("GetOffBed", true);
        pcc.StopToWalk();
        yield return Timing.WaitUntilDone(handlerHolder);

        yield return Timing.WaitForSeconds(6);


        //Praskovaya walks to ivan
        praskovayaAnim.SetBool("Hands", false);
        Vector3 praskovayaFirstPos = Praskovaya.transform.position;
        praskovayaNma.SetDestination(player.transform.position + player.transform.forward *2);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Praskovaya,0f));
        yield return Timing.WaitUntilDone(handlerHolder);

        //Talk
        sc.callSubtitle();
        while (subtitle.text != "")
        {
            yield return 0;
        }

        //praskovaya returns to hr poistion
        praskovayaNma.SetDestination(praskovayaFirstPos);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Praskovaya, 0f));

        yield return Timing.WaitForSeconds(1);

        //Ivan get up
        playerAnim.SetTrigger("GetOffContinue");
        playerAnim.SetBool("GetOffBed", false);

        yield return Timing.WaitUntilDone(handlerHolder);

        handlerHolder=Timing.RunCoroutine(Vckrs._lookTo(Praskovaya, -Praskovaya.transform.right, 1f));
        yield return Timing.WaitUntilDone(handlerHolder);
        praskovayaAnim.SetBool("Hands", true);

        yield return Timing.WaitForSeconds(2);
        Timing.RunCoroutine(Vckrs._Tween(player, player.transform.position+player.transform.forward*1.5f-player.transform.up*0.3f, 1f));
        yield return Timing.WaitForSeconds(4);
       
        playerNma.enabled = true;
        pcc.ContinueToWalk();

    }

    public void startBreadGame()
    {
        Timing.RunCoroutine(_startBreadGame());
    }

    IEnumerator<float> _startBreadGame()
    {
        if (CollectableObject.collected.Count == 0)
        {
            yield break;
        }

        GameObject smallBread = CollectableObject.collected[0];
        CollectableObject co = smallBread.GetComponent<CollectableObject>();
        co.UnCollect(player.transform.position + player.transform.forward * 1+player.transform.up*2);

        sc.callSubtitleTime();
        
        while (narSubtitle.text[0] != ' ')
        {
            yield return 0;

        }

        Bread.SetActive(true);
    

    }


    IEnumerator<float> _noseDrop()
    {
        Nose.SetActive(true);
        Rigidbody rb = Nose.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.right*2, ForceMode.Impulse);

        yield return Timing.WaitForSeconds(3);

        praskovayaAnim.SetBool("Hands", false);

        sc.callSubtitle();
        while (subtitle.text != "")
        {
            yield return 0;
        }



        praskovayaNma.SetDestination(Nose.transform.position - Vector3.forward);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Praskovaya,0));
              
        yield return Timing.WaitUntilDone(handlerHolder);

        Timing.RunCoroutine(Vckrs._lookTo(Praskovaya,Nose, 1f));

        playerNma.enabled = true;
        playerAnim.SetTrigger("SitTrigger");
        playerNma.SetDestination(Nose.transform.position - Vector3.right);

        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(player, 0));
        yield return Timing.WaitUntilDone(handlerHolder);

        Timing.RunCoroutine(Vckrs._lookTo(player, Nose, 1f));

        rb.isKinematic = true;


        sc.callSubtitle();
        while (subtitle.text != "")
        {
            yield return 0;
        }

        
    }


}
