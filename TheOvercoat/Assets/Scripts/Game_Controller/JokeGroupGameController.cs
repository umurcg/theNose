using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using MovementEffects;

public class JokeGroupGameController : MonoBehaviour {
    public GameObject marco, zoria, trigger;

    AlwaysLookTo marcoALT;
    AlwaysLookTo zoriaALT;
    NavMeshAgent marcoNma;
    NavMeshAgent zoriaNma;
    GameObject player;
    Text subt;
    PlayerComponentController pcc;
    NavMeshAgent nma;
    SubtitleCaller sc;

	// Use this for initialization
	void Start () {
        
        
        player = CharGameController.getActiveCharacter();
        if (player == null)
        {
            enabled = false;
            gameObject.SetActive(false);
            return;
        }

        //Debug.Log("Last scene is " + GlobalController.Instance.getLastSceneInList()); //TODO disable if is not coming from Ivan scene


        subt = SubtitleFade.subtitles["CharacterSubtitle"].GetComponent<Text>();
        marcoALT = marco.GetComponent<AlwaysLookTo>();
        zoriaALT = zoria.GetComponent<AlwaysLookTo>();
        pcc = player.GetComponent<PlayerComponentController>();
        nma = player.GetComponent<NavMeshAgent>();
        marcoNma=marco.GetComponent<NavMeshAgent>();
        zoriaNma=zoria.GetComponent<NavMeshAgent>();
        sc = GetComponent<SubtitleCaller>();
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

        subt.text = "Hey Ivan!";
        yield return Timing.WaitForSeconds(1.5f);
        pcc.StopToWalk();
        nma.Stop();

        marcoNma.SetDestination(player.transform.position + player.transform.forward * 2 + player.transform.right * 1);
        zoriaNma.SetDestination(player.transform.position + player.transform.forward * 2 - player.transform.right * 1);

        yield return Timing.WaitForSeconds(0.5f);
        IEnumerator<float> handler=Timing.RunCoroutine(Vckrs.waitUntilStop(zoria, 0.01f));
        yield return Timing.WaitUntilDone(handler);

        marcoALT.aim = player;
        zoriaALT.aim = player;
        marcoALT.enabled = true;
        zoriaALT.enabled = true;

        sc.callSubtitle();
        while (subt.text != "")
        {
            yield return 0;
        }

        yield return Timing.WaitForSeconds(3);
        sc.callSubtitle();
        while (subt.text != "")
        {
            yield return 0;
        }


        marcoALT.enabled = false;
        zoriaALT.enabled = false;

        marcoNma.SetDestination (marco.transform.position-Vector3.right * 80);
        zoriaNma.SetDestination(zoria.transform.position - Vector3.right * 80);

        yield return Timing.WaitForSeconds(1.5f);

        subt.text = "-Marko: Bak şimdi anlatıyorum. ";
        yield return Timing.WaitForSeconds(2);
        subt.text="-Marko: Zegin bir adam hastanede doğan yeni çocuğunun haberini bekliyormuş...";
        yield return Timing.WaitForSeconds(5);
        subt.text = "";
        yield return Timing.WaitForSeconds(5);

        marco.SetActive(false);
        zoria.SetActive(false);
    }

}
