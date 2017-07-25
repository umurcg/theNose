using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class TavernGameController : GameController {

    public GameObject tarkovksy, door, tarkovskyChair,chair, barBuilding, bears;

    public Material drunkTransparentMaterial;
    public Material drunkMaterial;
    public Material normalMaterial;
    public GameObject tavernBuilding;
    public GameObject[] drunkBuildings;

    characterComponents ccTarkovsky;
    WalkLookAnim tarkovskySitScript;

    bool disabled;

	// Use this for initialization
	public override void Start () {
        base.Start();
        ccTarkovsky = new characterComponents(tarkovksy);

        tarkovskySitScript = tarkovskyChair.GetComponent<WalkLookAnim>();
        //tarkovskySitScript.start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public  void TarkovskyStopsIvan()
    {
        if (disabled) return;
        Timing.RunCoroutine(_TarkovskyStopsIvan());
    }

    IEnumerator<float> _TarkovskyStopsIvan()
    {
        
  
        //Debug.Log("Tavern game");
        pcc.StopToWalk();
        Vector3 aim = door.transform.position + door.transform.forward * 3;

        //tarkovskySitScript.getUp();

        ////while (ccTarkovsky.navmashagent != enabled) yield return 0;
        //yield return Timing.WaitForSeconds(3f);

        ccTarkovsky.navmashagent.SetDestination(aim);

        Timing.RunCoroutine(Vckrs._lookTo(player, tarkovksy.transform.position - player.transform.position, 1f));

        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(tarkovksy));

        yield return Timing.WaitUntilDone(handlerHolder);

        //while (Vector3.Distance(tarkovksy.transform.position, aim) > 2)
        //{
        //    //Debug.Log(Vector3.Distance(tarkovksy.transform.position, aim)); //TODO look at here
        //    yield return 0;
        //}
       
        Debug.Log("Calling subtitle");
        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "")
        {
            
            yield return 0;
        }
  

        ccTarkovsky.navmashagent.SetDestination(tarkovskyChair.transform.position);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(tarkovksy,0));
        yield return Timing.WaitUntilDone(handlerHolder);
        //ccTarkovsky.animator.SetBool("SitPosition", true);
        tarkovskySitScript.start();

        pcc.ContinueToWalk(); 

        yield break;
    }


    public void ivanEntersBar()
    {
        if (disabled) return;

        ClickTriggerSingleton cts = player.GetComponent<ClickTriggerSingleton>();
        if(cts) cts.abort();
        sc.callSubtitleWithIndex(1);
    }

    public void ivanSitsBar()
    {
        //if (disabled) return;
        string charName = CharGameController.getActiveCharacter().gameObject.name;
        if (charName == "Ivan")
        {
            Timing.RunCoroutine(_ivanSitsBar());
        }
        //else if(charName!="Bird")
        //{
            Timing.RunCoroutine(_justDrink());
            //just drink
        //}
    }


    IEnumerator<float> _justDrink()
    {
        chair.GetComponent<WalkLookAnim>().lockSit = true;

        yield return Timing.WaitForSeconds(3);
        sc.callSubtitleWithIndex(4);
        if (player.name == "Bird")
        {
            subtitle.text = player.name + ": " + "tweet tweet cluck clap cock a doo";
        }
        else
        {
            subtitle.text = player.name + ": " + subtitle.text;
        }

        while (subtitle.text != "") yield return 0;
        yield return Timing.WaitForSeconds(2);
        handlerHolder = blackScreen.script.fadeOut();
        yield return Timing.WaitUntilDone(handlerHolder);

        makeAllBuildingsDrunk();
        Timing.RunCoroutine(Vckrs._fadeObject(tavernBuilding, 1f));

        yield return Timing.WaitForSeconds(5);
        bears.transform.GetChild(2).gameObject.SetActive(true);


        handlerHolder = blackScreen.script.fadeIn();
        yield return Timing.WaitUntilDone(handlerHolder);
        chair.GetComponent<WalkLookAnim>().lockSit = false;
        yield break;
        yield break;
    }

    IEnumerator<float> _ivanSitsBar()
    {
        chair.GetComponent<WalkLookAnim>().lockSit = true;

        yield return Timing.WaitForSeconds(3);
        sc.callSubtitleWithIndex(2);
        while (subtitle.text != "") yield return 0;
        yield return Timing.WaitForSeconds(2);
        handlerHolder= blackScreen.script.fadeOut();
        yield return Timing.WaitUntilDone(handlerHolder);
                
        makeAllBuildingsDrunk();
        Timing.RunCoroutine(Vckrs._fadeObject(tavernBuilding, 1f));

        yield return Timing.WaitForSeconds(5);

        bears.transform.GetChild(0).gameObject.SetActive(false);
        bears.transform.GetChild(1).gameObject.SetActive(true);
        bears.transform.GetChild(2).gameObject.SetActive(true);


        handlerHolder =blackScreen.script.fadeIn();
        yield return Timing.WaitUntilDone(handlerHolder);
        sc.callSubtitleWithIndex(3);
        while (subtitle.text != "") yield return 0;


        chair.GetComponent<WalkLookAnim>().lockSit = false;
        yield break;
    }

    public override void activateController()
    {
        disabled = false;
        tarkovksy.SetActive(true);
    }
    public override void deactivateController()
    {

        disabled = true;
        bears.transform.GetChild(0).gameObject.SetActive(false);
        tarkovksy.SetActive(false);

    }

    public void makeAllBuildingsDrunk()
    {
        foreach(GameObject obj in drunkBuildings)
        {
            Renderer[] rends=obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                rend.material = drunkMaterial;
            }
        }

        barBuilding.GetComponent<Renderer>().material = drunkTransparentMaterial;

    }

    public void recoverAllBuildings()
    {

        foreach (GameObject obj in drunkBuildings)
        {
            Renderer[] rends = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                rend.material = normalMaterial;
            }
        }
        barBuilding.GetComponent<Renderer>().material = normalMaterial;
    }
}
