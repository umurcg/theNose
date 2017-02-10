using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class TavernGameController : GameController {

    public GameObject tarkovksy, door, tarkovskyChair;

    characterComponents ccTarkovsky;

	// Use this for initialization
	public override void Start () {
        base.Start();
        ccTarkovsky = new characterComponents(tarkovksy);


	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public  void TarkovskyStopsIvan()
    {
        if(enabled)
        Timing.RunCoroutine(_TarkovskyStopsIvan());
    }

    IEnumerator<float> _TarkovskyStopsIvan()
    {
  
        Debug.Log("Tavern game");
        pcc.StopToWalk();
        Vector3 aim = door.transform.position + door.transform.forward * 3;
        ccTarkovsky.navmashagent.SetDestination(aim);

        Timing.RunCoroutine(Vckrs._lookTo(player, tarkovksy.transform.position - player.transform.position, 1f));

        while (Vector3.Distance(tarkovksy.transform.position, aim) > 2)
        {
            //Debug.Log(Vector3.Distance(tarkovksy.transform.position, aim)); //TODO look at here
            yield return 0;
        }
       
        //Debug.Log("Calling");
        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "")
        {
            
            yield return 0;
        }
  

        ccTarkovsky.navmashagent.SetDestination(tarkovskyChair.transform.position);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(tarkovksy,0));
        yield return Timing.WaitUntilDone(handlerHolder);
        ccTarkovsky.animator.SetBool("SitPosition", true);
        

        pcc.ContinueToWalk(); 

        yield break;
    }


    public void ivanEntersBar()
    {

        sc.callSubtitleWithIndex(1);
    }

    public void ivanSitsBar()
    {
        Timing.RunCoroutine(_ivanSitsBar());
    }
    IEnumerator<float> _ivanSitsBar()
    {
        yield return Timing.WaitForSeconds(3);
        sc.callSubtitleWithIndex(2);
        while (subtitle.text != "") yield return 0;
        yield return Timing.WaitForSeconds(2);
        blackScreen.script.fadeOut();
        yield return Timing.WaitForSeconds(5);
        blackScreen.script.fadeIn();
    }
   
}
