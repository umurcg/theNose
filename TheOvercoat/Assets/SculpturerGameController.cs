using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class SculpturerGameController : GameController {

    public GameObject playerInitialPos;
    public GameObject sculpturer;
    public GameObject ropeGame;
    public float healthOfPlayer = 100;
    float health;
    float sculpHealth;
    List<HeykelController> heykels;
    public GameObject healthBarPrefab;
    Image healthBar;
    Image healthBarSculp;

    //public GameObject target;
    //public GameObject source;
    //public GameObject master;
    //float AA;
    //float BB;


	// Use this for initialization
	public override void Awake () {
        base.Awake();
        heykels = new List<HeykelController>();
        
       
	}

    public override void Start()
    {
        base.Start();
        CharGameController.movePlayer(playerInitialPos.transform.position);
        playerAnim.SetBool("HandsAreTied", true);
        enableHeykels(false);
        base.Start();

        //Timing.RunCoroutine(innerSpeech());
        startGame();


    }

    // Update is called once per frame
    void Update () {

        if (healthBar != null && health <= 0) die();

        if (healthBarSculp != null && sculpHealth <= 0) win();

    }


    public void startGame()
    {
        Timing.RunCoroutine(_startGame());
    }

    IEnumerator<float> _startGame()
    {
        
        playerAnim.SetBool("HandsAreTied", false);

        //Subt
        sc.callSubtitleWithIndex(1);
        while (subtitle.text != "") yield return 0;

        unlockPlayer();

        sculpturer.GetComponent<SculpturerAI>().enabled = true;
        enableHeykels(true);
        Transform canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        healthBar=  ((GameObject)Instantiate(healthBarPrefab,canvas) as GameObject).GetComponent<Image>();
        RectTransform rt = healthBar.GetComponent<RectTransform>();
        rt.position =new Vector2( Screen.width * 4 / 5,Screen.height*4/5);

        health = healthOfPlayer;
        healthBar.fillAmount = health/healthOfPlayer;

        yield break;
    }

    void oneHeykelIsLeft()
    {
        heykels[0].shootMaster(true);
        sculpturer.GetComponent<SculpturerAI>().shootWithAlci(true);

        Transform canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        healthBarSculp = ((GameObject)Instantiate(healthBarPrefab, canvas) as GameObject).GetComponent<Image>();
        healthBarSculp.transform.GetChild(0).GetComponent<Text>().text = "Enemy Health";
        RectTransform rt = healthBar.GetComponent<RectTransform>();
        rt.position = new Vector2(Screen.width * 1 / 5, Screen.height * 4 / 5);

        sculpHealth = healthOfPlayer;
        healthBar.fillAmount = sculpHealth / healthOfPlayer;


    }

    IEnumerator<float> innerSpeech()
    {


        //Look each other
        player.transform.LookAt(new Vector3(sculpturer.transform.position.x, player.transform.position.y, sculpturer.transform.position.z));
        sculpturer.transform.LookAt(new Vector3(player.transform.position.x, sculpturer.transform.position.y, player.transform.position.z));

        lockPlayer();

      
        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "")
        {
            //if(GetComponent<SubtitleController>().getCurrentIndex()==4 && handlerHolder ==null)
            //    //Make pace sculpturer
            //    handlerHolder = Timing.RunCoroutine(Vckrs._pace(sculpturer, sculpturer.transform.position + sculpturer.transform.right * 4, sculpturer.transform.position - sculpturer.transform.right * 4));

            yield return 0;
        }

        //Timing.KillCoroutines(handlerHolder);

        ropeGame.GetComponent<RopeGameController>().enabled = true;

        yield break;
    }
    
    //Locks player while it is captured by sculpturer
    //TODO modelling and animations
    void lockPlayer()
    {
        pcc.StopToWalk();
    }

    void unlockPlayer()
    {
        pcc.ContinueToWalk();
    }


    public void registerHeykel(HeykelController heykel)
    {
        heykels.Add(heykel);
    }

    public void removeHeykel(HeykelController heykel)
    {
        heykels.Remove(heykel);
        if (heykels.Count == 1) oneHeykelIsLeft();
    }

    void enableHeykels(bool enable)
    {
        foreach (HeykelController heykel in heykels)
        {
            heykel.enabled = enable;
        }
    }

    public void damage(int amount)
    {
        health -= amount;
        healthBar.fillAmount = health/healthOfPlayer;
    }

    public void damageEnemy(int amount)
    {
        Debug.Log("Enemy damage");
        sculpHealth -= amount;
        healthBarSculp.fillAmount = sculpHealth/healthOfPlayer;

    }

    void win()
    {
        sculpturer.tag = "ActiveObject";
        sculpturer.GetComponent<SculpturerAI>().enabled = false;
        Debug.Log("Win");
        

    }

    IEnumerator<float> lost()
    {
        sc.callSubtitleWithIndex(2);
        while (subtitle.text != "") yield return 0;

        Debug.Log("Lost");
        LoadScene ls=GetComponent<LoadScene>();
        ls.Scene = GlobalController.Scenes.Atolye;
        ls.Load();

    }

    void die()
    {
        //Die animation
        Timing.RunCoroutine(lost());
    }

    public void freezeKovalev()
    {
        Debug.Log("Freeze");
        pcc.StopToWalk();
        //Alciyla kapla
        Timing.RunCoroutine(lost());


    }

    public void outerSpeech()
    {
        Timing.RunCoroutine(_outerSpeech());
    }

    IEnumerator<float> _outerSpeech()
    {
        

        yield return 0;
        sc.callSubtitleWithIndex(3);

        while (subtitle.text != "") yield return 0;

        //Register before exiting scene so kovalev house will understand last game controller is this and arrange scen according to that
        registerAsUsed();


        LoadScene ls = GetComponent<LoadScene>();
        ls.Scene = GlobalController.Scenes.KovalevHouse;
        ls.Load();

    }

    
}

