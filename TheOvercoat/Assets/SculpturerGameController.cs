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
    public Material freezeMaterial;
    PointBarScript healthBar;
    PointBarScript healthBarSculp;

    public Transform canvas;

    string kovalevBarName, sculptrurerBarName;
    

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

        Language lan = (Language)GlobalController.Instance.getLangueSetting();
        if (lan == Language.ENG)
        {
            kovalevBarName = "Kovalev Health";
            sculptrurerBarName = "Sculpturer Health";
        }
        else
        {
            kovalevBarName = "Kovalev'in Cani";
            sculptrurerBarName = "Heykeltirasin Cani";

        }


        Timing.RunCoroutine(innerSpeech());
        //win();



        //lockPlayer();
        //ropeGame.GetComponent<RopeGameController>().enabled = true;

        //startGame();

    }

    // Update is called once per frame
    void Update () {

        if (healthBar != null && health <= 0)
        {
            die();
            enabled = false;
        }

        if (healthBarSculp != null && sculpHealth <= 0)
        {
            win();
            enabled = false;
        }

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

        //Transform canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        healthBar=  ((GameObject)Instantiate(healthBarPrefab,canvas) as GameObject).GetComponent<PointBarScript>();
        healthBar.gameObject.SetActive(true);
        healthBar.setName(kovalevBarName);
        healthBar.setLimits(healthOfPlayer, 0);

        RectTransform rt = healthBar.GetComponent<RectTransform>();
        rt.position =new Vector2( Screen.width * 4 / 5,Screen.height*4/5);

        health = healthOfPlayer;
        healthBar.setPoint(healthOfPlayer);

        yield break;
    }

    void oneHeykelIsLeft()
    {
        heykels[0].shootMaster(true);
        sculpturer.GetComponent<SculpturerAI>().shootWithAlci(true);
        
        healthBarSculp = ((GameObject)Instantiate(healthBarPrefab, canvas) as GameObject).GetComponent<PointBarScript>();
        healthBarSculp.gameObject.SetActive(true);
        healthBarSculp.setName(sculptrurerBarName);
        healthBarSculp.setLimits(healthOfPlayer, 0);
        sculpHealth = healthOfPlayer;
        healthBarSculp.setPoint(sculpHealth);


        RectTransform rt = healthBarSculp.GetComponent<RectTransform>();
        rt.position = new Vector2(Screen.width * 1 / 5, Screen.height * 4 / 5);



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
        //Timing.RunCoroutine(lost());
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

    public void damage(float amount)
    {
        //Debug.Log("Damagin " + amount);
        health -= amount;
        healthBar.setPoint(health);
    }

    public void damageEnemy(int amount)
    {
        Debug.Log("Enemy damage");
        sculpHealth -= amount;
        healthBarSculp.setPoint(sculpHealth);

    }

    [ContextMenu("win")]
    void win()
    {
        Timing.RunCoroutine(_win());
    }

    IEnumerator<float> _win()
    {
        sculpturer.tag = "ActiveObject";
        sculpturer.GetComponent<SculpturerAI>().enabled = false;
        Debug.Log("Win");

        foreach(HeykelController H in heykels)
        {
            
            if (H) H.enabled = false;
        }


        Animator sculpAnim = sculpturer.GetComponent<Animator>();


        
        sculpAnim.SetTrigger("die");
        yield return 0;

        //yield return Timing.WaitUntilDone(Timing.RunCoroutine(Vckrs.waitUntilFractionOfAnimation(sculpAnim, 1, "dirPart1")));

        Timing.RunCoroutine(Vckrs._lookTo(player, sculpturer, 1f));

        yield return Timing.WaitForSeconds(1);

        //sculpAnim.speed = 0;


        Debug.Log("animation is finished");

        sc.callSubtitleWithIndex(3);
        while (subtitle.text!="") yield return 0;



        sculpAnim.SetTrigger("die");


        yield return Timing.WaitForSeconds(2);

        //Register before exiting scene so kovalev house will understand last game controller is this and arrange scen according to that
        registerAsUsed();


        LoadScene ls = GetComponent<LoadScene>();
        ls.Scene = GlobalController.Scenes.KovalevHouse;
        ls.Load();

        //sculpAnim.speed = 1;

        yield break;
    }

    IEnumerator<float> lost()
    {
        pcc.StopToWalk();
  


        Renderer playerRenderer=null;
        for(int i=0;i<player.transform.childCount;i++)
            if(player.transform.GetChild(i).gameObject.activeSelf && player.transform.GetChild(i).GetComponent<Renderer>()) playerRenderer =player.transform.GetChild(i).GetComponent<Renderer>();

        Material originalMat = playerRenderer.material;
        playerRenderer.material = freezeMaterial;


        player.GetComponent<BasicCharAnimations>().enabled = false;
        playerAnim.speed = 0;

        sculpturer.GetComponent<SculpturerAI>().enabled = false;
        Timing.RunCoroutine(Vckrs._lookTo(sculpturer, player, 1f));

        sc.callSubtitleWithIndex(2);
        while (subtitle.text != "") yield return 0;

        Debug.Log("Lost");
        LoadScene ls=GetComponent<LoadScene>();
        ls.Scene = GlobalController.Scenes.Atolye;
        handlerHolder=Timing.RunCoroutine( ls._Load());

        yield return Timing.WaitUntilDone(handlerHolder);
        playerAnim.speed = 1;
        player.GetComponent<BasicCharAnimations>().enabled = true;
        playerRenderer.material = originalMat;

    }

    void die()
    {
        //Die animation
        Timing.RunCoroutine(lost());
    }

    //public void freezeKovalev()
    //{
    //    Debug.Log("Freeze");
    //    pcc.StopToWalk();
    //    //Alciyla kapla
    //    Timing.RunCoroutine(lost());


    //}

    //public void outerSpeech()
    //{
    //    Timing.RunCoroutine(_outerSpeech());
    //}

    //IEnumerator<float> _outerSpeech()
    //{
        

    //    yield return 0;
    //    sc.callSubtitleWithIndex(3);

    //    while (subtitle.text != "") yield return 0;

    //    //Register before exiting scene so kovalev house will understand last game controller is this and arrange scen according to that
    //    registerAsUsed();


    //    LoadScene ls = GetComponent<LoadScene>();
    //    ls.Scene = GlobalController.Scenes.KovalevHouse;
    //    ls.Load();

    //}

    [ContextMenu ("full health")]
    void fullHealth()
    {
        health = healthOfPlayer;
        healthBar.setPoint(health);
    }

    
}

