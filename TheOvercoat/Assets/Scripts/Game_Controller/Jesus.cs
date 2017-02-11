using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;


public class Jesus : GameController, IClickAction {

    public GameObject pontiff;
    public GameObject priest;
    public GameObject door;
    public GameObject basementTrigger,throne;


    NavMeshAgent priestNma;
    Animator priestAnim;

    characterComponents pontiffCC;


    // Use this for initialization
    public override void Start () {
        base.Start();
        priestNma = priest.GetComponent<NavMeshAgent>();
        priestAnim = priest.GetComponent<Animator>();

        pontiffCC = new characterComponents(pontiff);

    }
	
	// Update is called once per frame
	void Update () {
	    


	}

    public void Action()
    {
        print("hello");
        Timing.RunCoroutine(_pray());

    }

    IEnumerator<float> _pray()
    {
        playerNma.SetDestination(transform.position - transform.up * 2);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(player, 0));
        yield return Timing.WaitUntilDone(handlerHolder);

        Timing.RunCoroutine(Vckrs._lookTo(player, transform.position - player.transform.position, 1f));
        sc.callSubtitle();
        while (subtitle.text != "")
        {
            yield return 0;
        }


        priestNma.SetDestination(player.transform.position + player.transform.right * 3);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(priest, 0));
        yield return Timing.WaitUntilDone(handlerHolder);

        Timing.RunCoroutine(Vckrs._lookTo(priest, player.transform.position - priest.transform.position, 1f));

        sc.callSubtitle();

        yield return Timing.WaitForSeconds(1.5f);
        Timing.RunCoroutine(Vckrs._lookTo(player, priest.transform.position - player.transform.position, 1f));

        while (subtitle.text != "")
        {
            yield return 0;

        }

        playerAnim.SetBool("RightHandAtFace", false);

        yield return Timing.WaitForSeconds(1.5f);

        sc.callSubtitle();

        while (subtitle.text != "")
        {
            yield return 0;

        }

        priestNma.SetDestination(door.transform.position - door.transform.up);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(priest, 0));
        yield return Timing.WaitUntilDone(handlerHolder);

        handlerHolder = Timing.RunCoroutine(Vckrs._lookTo(priest, door.transform.position - priest.transform.position, 1f));
        yield return Timing.WaitUntilDone(handlerHolder);

        priestAnim.SetBool("Hands", true);
        yield return Timing.WaitForSeconds(3f);



        priestAnim.SetBool("Hands", false);
        yield return Timing.WaitForSeconds(1f);
        priestNma.SetDestination(door.transform.position - door.transform.up * 2+door.transform.right*2);

        while (door.transform.rotation.z < 0.4f)
        {
            print(door.transform.rotation.z);
            door.transform.Rotate(Vector3.forward, Timing.DeltaTime*30f);
            yield return 0;
        }


        sc.callSubtitle();

        while (subtitle.text != "")
        {
            yield return 0;

        }


    }

    public void basement()
    {
        Timing.RunCoroutine(_basement());
    }

    IEnumerator<float> _basement()
    {
        //First subt
        sc.callSubtitle();

        yield return Timing.WaitForSeconds(2f);

        //Pontiff get closer
        pontiffCC.navmashagent.SetDestination(player.transform.position + Vector3.forward*3);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(pontiff, 0));

        while (subtitle.text != "") yield return 0;

        yield return Timing.WaitUntilDone(handlerHolder);

        //Second sub
        sc.callSubtitle();
        while (subtitle.text != "") yield return 0;

        WalkLookAnim wla = throne.GetComponent<WalkLookAnim>();

        //Wait for the sit
        while (!wla.isSitting()) yield return 0;

        //Third sub
        sc.callSubtitle();
        while (subtitle.text != "") yield return 0;


    }

    public override void activateController()
    {
        gameObject.SetActive(true);
    }
    public override void deactivateController()
    {

        gameObject.SetActive(false);

    }

}
