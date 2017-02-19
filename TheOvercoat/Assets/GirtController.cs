using UnityEngine;
using System.Collections;

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
	
	// Update is called once per frame
	void Update () {
	
	}


    public override void activateController()
    {
        base.activateController();
        dltpta.enabled = true;
        rwaa.enabled = true;
    }
    public override void deactivateController()
    {
        base.deactivateController();

        dltpta = GetComponent<DontLetPlayerToApproach>();
        rwaa = GetComponent<RandomWalkAndAnimate>();
        animDog = GetComponent<Animator>();

        //Debug.Log("hi");
        dltpta.enabled = false;
        rwaa.enabled = false;
        animDog.SetBool("Bark", true);

    }
}
