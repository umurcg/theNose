using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using MovementEffects;

public class JokeGroupGameController : GameController {
    public GameObject marco, zoria, trigger;

    AlwaysLookTo marcoALT;
    AlwaysLookTo zoriaALT;
    UnityEngine.AI.NavMeshAgent marcoNma;
    UnityEngine.AI.NavMeshAgent zoriaNma;




	// Use this for initialization
	public override void Start () {

        base.Start();
        
        if (player == null)
        {
            enabled = false;
            gameObject.SetActive(false);
            return;
        }

        //Debug.Log("Last scene is " + GlobalController.Instance.getLastSceneInList()); //TODO disable if is not coming from Ivan scene



        marcoALT = marco.GetComponent<AlwaysLookTo>();
        zoriaALT = zoria.GetComponent<AlwaysLookTo>();
        marcoNma=marco.GetComponent<UnityEngine.AI.NavMeshAgent>();
        zoriaNma=zoria.GetComponent<UnityEngine.AI.NavMeshAgent>();

    }
	
    public void joke()
    {
        Timing.RunCoroutine(_joke());
    }

	IEnumerator<float> _joke()
    {
        trigger.SetActive(false);
        print("joke");
        marco.SetActive(true);
        zoria.SetActive(true);

        subtitle.text = "Hey Ivan!";
        yield return Timing.WaitForSeconds(1.5f);
        pcc.StopToWalk();
        playerNma.Stop();

        marcoNma.SetDestination(player.transform.position + player.transform.forward * 2 + player.transform.right * 1);
        zoriaNma.SetDestination(player.transform.position + player.transform.forward * 2 - player.transform.right * 1);

        yield return Timing.WaitForSeconds(0.5f);
        IEnumerator<float> handler=Timing.RunCoroutine(Vckrs.waitUntilStop(zoria, 0.01f));
        yield return Timing.WaitUntilDone(handler);

        marcoALT.aim = player;
        zoriaALT.aim = player;
        marcoALT.enabled = true;
        zoriaALT.enabled = true;

        subtitle.text = "";
        sc.callSubtitle();
        while (subtitle.text != "")
        {
            yield return 0;
        }

        yield return Timing.WaitForSeconds(3);
        sc.callSubtitle();
        while (subtitle.text != "")
        {
            yield return 0;
        }


        marcoALT.enabled = false;
        zoriaALT.enabled = false;

        marcoNma.SetDestination (marco.transform.position-Vector3.right * 80);
        zoriaNma.SetDestination(zoria.transform.position - Vector3.right * 80);

        yield return Timing.WaitForSeconds(1.5f);

        sc.callSubtitleWithIndexTime(0);
        //subtitle.text = "-Marko: Bak şimdi anlatıyorum. ";
        //yield return Timing.WaitForSeconds(2);
        //subtitle.text = "-Marko: Zegin bir adam hastanede doğan yeni çocuğunun haberini bekliyormuş...";

        while (subtitle.text != "") yield return 0;

        yield return Timing.WaitUntilDone(Vckrs.waitUntilStop(marco));

        marco.SetActive(false);
        zoria.SetActive(false);
    }

    public override void activateController()
    {
        base.activateController();
        gameObject.SetActive(true);
    }
    public override void deactivateController()
    {
        base.deactivateController();
        gameObject.SetActive(false);

    }

}
