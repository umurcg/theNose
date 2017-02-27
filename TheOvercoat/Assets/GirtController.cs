using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Girty handles its own activation and deactivation while it is complecated.
public class GirtController : GameController {

    DontLetPlayerToApproach dltpta;
    RandomWalkAndAnimate rwaa;
    Animator animDog;
	// Use this for initialization
	public override void Awake () {
        base.Awake();
        dltpta = GetComponent<DontLetPlayerToApproach>();
        rwaa = GetComponent<RandomWalkAndAnimate>();
        animDog=GetComponent<Animator>();
        GameObject player = CharGameController.getActiveCharacter();

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

        dltpta = GetComponent<DontLetPlayerToApproach>();
        rwaa = GetComponent<RandomWalkAndAnimate>();
        animDog = GetComponent<Animator>();

        if (player.name == "Ivan")
        {  

            dltpta.enabled = false;
            rwaa.enabled = false;
            animDog.SetBool("Bark", true);
        }else if(player.name=="Kovalev" && GlobalController.countSceneInList(GlobalController.Scenes.Newspaper)==1 )
        {
            dltpta.enabled = true;
            rwaa.enabled = true;


        } else if(GlobalController.countSceneInList(GlobalController.Scenes.Newspaper) == 2)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            dltpta.enabled = false;
        }

    }


}
