using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllerPoliceStation : GameController {
    public GameObject trigger,  Gaurd,  Chair0;

    EnterTrigger et;
    public bool debug;
    bool comingFirstTime=false;



    public override void Awake () {
        base.Awake();
    
        et = trigger.GetComponent<EnterTrigger>();

        
        if (GlobalController.Instance == null)
        {
            comingFirstTime = true;
        }
        else
        {

            if (!GlobalController.Instance.sceneList.Contains(SceneManager.GetActiveScene().buildIndex))
                comingFirstTime = true;
         }

    }


    public override void Start()
    {
        base.Start();

        //If main character is kovalev force him to cover his face
        if (CharGameController.getActiveCharacter().name=="Kovalev")
            CharGameController.coverKovalevsFace();        
    }

    public void callGuardStopsKovalev() {
        Timing.RunCoroutine(_guardStopsKovalev());
    }

    IEnumerator<float> _guardStopsKovalev()
    {
        if (comingFirstTime == false)
            yield break;

        Gaurd.GetComponent<ClickExternalCallSubtitle>().Destroy();

        trigger.SetActive(false);
        pcc.StopToWalk();
        UnityEngine.AI.NavMeshAgent navKov = player.GetComponent<UnityEngine.AI.NavMeshAgent>();
        navKov.Stop();


        sc.callSubtitleWithIndex(0);

        yield return Timing.WaitForSeconds(2f);


        handlerHolder= Timing.RunCoroutine(Chair0.GetComponent<WalkLookAnim>()._getUp());
        yield return Timing.WaitUntilDone(handlerHolder);

        yield return Timing.WaitForSeconds(1f);

        UnityEngine.AI.NavMeshAgent guardnNma = Gaurd.GetComponent<UnityEngine.AI.NavMeshAgent>();
        guardnNma.Resume();
        guardnNma.SetDestination(Vector3.Lerp(Gaurd.transform.position, player.transform.position, 0.7f));
       
        Timing.RunCoroutine(Vckrs._lookTo(player, Gaurd.transform.position, 0.5f));

        yield return Timing.WaitForSeconds(0.5f);
        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs.waitUntilStop(Gaurd, 0f));
        yield return Timing.WaitUntilDone(handler);

        Timing.RunCoroutine(Vckrs._lookTo(Gaurd, player.transform.position-Gaurd.transform.position, 0.5f));

        while (subtitle.text != "")
        {
           
            yield return 0;
        }

        sc.callSubtitleWithIndex(1);
        //while(true)
        //yield return 0;

        while (subtitle.text != "") yield return 0;

        guardnNma.SetDestination(Chair0.transform.position + Chair0.transform.right * 2);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(guardnNma.gameObject));
        yield return Timing.WaitUntilDone(handlerHolder);

        Chair0.GetComponent<WalkLookAnim>().start();

        Gaurd.transform.tag = "Untagged";

        registerAsUsed();

        yield break;


    }

    public void DestroyItSelf()
    {
        Gaurd.transform.tag = "Untagged";

        registerAsUsed();

        Destroy(trigger);
    }

    public override void gameIsUsed()
    {
        base.gameIsUsed();
        DestroyItSelf();
    }



}
