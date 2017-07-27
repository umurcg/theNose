using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using MovementEffects;

public class DrunkManGameController : GameController {

    public GameObject canvas;
    public GameObject barPrefab;
    public GameObject sliderPrefab;
    public GameObject emptyUI;
    public DrunkManGameSceneController dmgsc;
    public DrunkManAI dmai;
    public ShootWithBottle swb;
    

    public Vector2 UIPosRatio;

    GameObject ui;
    GameObject kovalevHealthBar , drunkManHelathBar, slider;

    PointBarScript kovalevPS, drunkPS;

    float drunkManHealth = 100;
    float kovalevHealth = 100;

    public float UIScale;

    public Vector2 cellsize;
    public Vector2 spacing;

    Scrollbar sb;

    characterComponents drunkCC;

    public GameObject bigNose;

    //// Use this for initialization
    public override void Start()
    {
        base.Start();




    }


    private void OnEnable()
    {
        drunkCC = new characterComponents(dmai.gameObject);

        
        

        ui = Instantiate(emptyUI, canvas.transform) as GameObject;
        ui.transform.position = Vckrs.screenRatioToPosition(UIPosRatio.x, UIPosRatio.y);
        GridLayoutGroup glg = ui.AddComponent<GridLayoutGroup>();
        glg.cellSize = cellsize;
        glg.spacing = spacing;

        kovalevHealthBar = Instantiate(barPrefab/*, ui.transform*/) as GameObject;
        kovalevHealthBar.transform.parent = ui.transform;
        kovalevHealthBar.SetActive(true);
        kovalevPS = kovalevHealthBar.GetComponent<PointBarScript>();
        kovalevPS.setLimits(kovalevHealth, 0);
        kovalevPS.setName("Kovalev Health: ");
        kovalevPS.setPoint(kovalevHealth);


        drunkManHelathBar = Instantiate(barPrefab/*, ui.transform*/) as GameObject;
        drunkManHelathBar.SetActive(true);
        drunkManHelathBar.transform.parent = ui.transform;
        drunkPS = drunkManHelathBar.GetComponent<PointBarScript>();
        drunkPS.setLimits(drunkManHealth, 0);
        drunkPS.setName("Drunk Man Health: ");
        drunkPS.setPoint(drunkManHealth);

        slider = Instantiate(sliderPrefab/*, ui.transform*/) as GameObject;
        slider.transform.parent = ui.transform;
        slider.GetComponentInChildren<Text>().text = "Shoot Velocity";
        sb = slider.GetComponent<Scrollbar>();

        ui.transform.localScale = Vector3.one * UIScale;
    }


    private void OnDisable()
    {
        //Destroy(kovalevHealthBar);
        //Destroy(drunkManHelathBar);
        //Destroy(slider);
        if(ui!=null) Destroy(ui);
        dmai.enabled = false;
        swb.enabled = false;
        


    }

    // Update is called once per frame
    void Update () {
	
	}


    public void damage(float damageValue)
    {
        //Debug.Log("Player get damage: "+damageValue);

        if (enabled == false) return;

        kovalevHealth -= damageValue;
        if (kovalevHealth <= 0) lost();

        kovalevPS.setPoint(kovalevHealth);
  
    }

    public void damageEnemy(float damage)
    {
        //Debug.Log("Enemy get damage: " + damage);

        if (enabled == false) return;

        drunkManHealth -= damage;
        if (drunkManHealth <= 0) win();

        drunkPS.setPoint(drunkManHealth);

    }

    [ContextMenu("Lost")]
    public void lost()
    {
        Timing.RunCoroutine(_lost());
    }

    IEnumerator<float> _lost()
    {
        pcc.StopToWalk();
        playerAnim.SetTrigger("die");

        yield return Timing.WaitForSeconds(3f);

        yield return Timing.WaitUntilDone(Timing.RunCoroutine(blackScreen.script.fadeOut()));

        OnDisable();

        player.transform.position = dmgsc.kovalevEncPos;
        drunkCC.player.transform.position = dmgsc.drunkManEncPos;

        yield return Timing.WaitForSeconds(2f);

        playerAnim.SetTrigger("ForceIdle");

        yield return Timing.WaitForSeconds(1f);

        yield return Timing.WaitUntilDone(Timing.RunCoroutine(blackScreen.script.fadeIn()));

        dmgsc.encounter();

        enabled = false;

        yield break;
    }

    IEnumerator<float> _win()
    {
        CallCoroutine bc = bigNose.AddComponent<CallCoroutine>();
        bc.CallType = CallCoroutine.callType.ClickAction;
        bc.owner = gameObject;
        bc.methodName = "takeNose";

        bigNose.transform.tag = "ActiveObject";

        drunkCC.animator.SetTrigger("Die");
        dmai.enabled = false;
        drunkCC.navmashagent.isStopped = true;

        sc.callSubtitleWithIndex(3);

        while (subtitle.text != "") yield return 0;

        drunkCC.animator.SetTrigger("Die");


        swb.enabled = false;

        ObjectSpawnerContinously osc = GetComponent<ObjectSpawnerContinously>();
        osc.enabled = false;

        OnDisable();

        List<GameObject> bottles = osc.getSpawnedObjects();
        foreach (GameObject b in bottles)
        {
            b.transform.tag = "Untagged";
        }

        yield break;
    }

    [ContextMenu ("Win")]
    public void win() {

        Timing.RunCoroutine(_win());
    }
    
    public void takeNose()
    {
        bigNose.SetActive(false);
        GameObject noseAtHand=CharGameController.getHand(CharGameController.hand.RightHand).transform.GetChild(1).gameObject;
        noseAtHand.SetActive(true);
        OpenDoorLoad.getDoorSciptWithScene(GlobalController.Scenes.Doctor).Unlock();
        OpenDoorLoad.getDoorSciptWithScene(GlobalController.Scenes.KovalevHouse).Unlock();

        //Recover ligth
        DayAndNightCycle danc = CharGameController.getSun().GetComponent<DayAndNightCycle>();
        danc.minIntensity = 0.1f;

        registerAsUsed();


    }

    public void setVelocityUI(float value)
    {
        sb.value = Mathf.Clamp(value, 0, 1);
    }


    public override void gameIsUsed()
    {
        base.gameIsUsed();
        dmgsc.deactivateController();
    }


}
