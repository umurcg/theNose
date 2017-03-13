using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//Girty handles its own activation and deactivation while it is complecated.
public class GirtController : GameController {

    public GameObject girtyPetGameCanvas;

    RandomWalkAndAnimate rwaa;
    Animator animDog;
    NavMeshAgent girtyNma;
    bool girtyGame = false;
	// Use this for initialization
	public override void Awake () {
        base.Awake();
        rwaa = GetComponent<RandomWalkAndAnimate>();
        animDog=GetComponent<Animator>();
        GameObject player = CharGameController.getActiveCharacter();
        girtyNma = GetComponent<NavMeshAgent>();


        //if (player == null)
        //{
        //    dltpta.enabled = false;  
        //}else if (player.transform.name != "Kovalev")
        //{
        //    dltpta.enabled = false;
        //}
    }

    public override void Start()
    {
        base.Start();
        activation();


    }

    // Update is called once per frame
    void Update () {
	
	}


    void activation()
    {
        List<int> sceneList= GlobalController.Instance.sceneList;


        rwaa = GetComponent<RandomWalkAndAnimate>();
        animDog = GetComponent<Animator>();

        if (player.name == "Ivan")
        {  
            rwaa.enabled = false;
            animDog.SetBool("Bark", true);
        }else if(player.name=="Kovalev" && GlobalController.countSceneInList(GlobalController.Scenes.Newspaper)==1 )
        {
            
            rwaa.enabled = true;
            girtyGame = true;

        } else if(GlobalController.countSceneInList(GlobalController.Scenes.Newspaper) == 2)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            //Debug.Log("casual");
            rwaa.enabled = true;
        }


    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player" && girtyGame)
        {
            Timing.RunCoroutine(triggerGirtyPetGame());

        }
    }

    public void win()
    {
        sc.callSubtitleWithIndex(1);
        GetComponent<girtyBeFriendsScript>().enabled = true;
        girtyPetGameCanvas.transform.GetChild(0).gameObject.SetActive(false);
        Destroy(GetComponent<SphereCollider>());
    }

    IEnumerator<float> triggerGirtyPetGame()
    {
        //Set camera of canvas
        girtyPetGameCanvas.GetComponent<Canvas>().worldCamera = CharGameController.getCamera().GetComponent<Camera>();
        rwaa.enabled = false;
        girtyNma.Stop();
        Timing.RunCoroutine(Vckrs._lookTo(gameObject, player, 1f));
                
        GetComponent<Animator>().SetBool("Bark",true);
        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "") yield return 0;

        
        girtyPetGameCanvas.transform.GetChild(0).gameObject.SetActive(true);

        yield return 0;
    }

    //public void barkToChairDance()
    //{
         
    //}

    //public void allGamesActive()
    //{

    //}

    //public void randomMovementAndBark()
    //{

    //}

}
