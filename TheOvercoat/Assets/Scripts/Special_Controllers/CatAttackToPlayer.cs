using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class CatAttackToPlayer : GameController, IClickAction {
    public GameObject catNose, trigger;
    GameObject nose;

    //PlayerComponentController pcc;
    NavMeshAgent catNma;
    RandomWalkBot rwb;
    Text charSubt;
    Animator catAnim;
    //Animator playerAnim;
    CharacterController playerCC;
    public float JumpSpeed = 2;

    bool used = false;
    public bool debug;


	//public float jumpTolerance = 0.5f;
	//Vector3 initialPos;

   

	//Quaternion initialRot;
	//public float rotTolerance=0.1f;
	//float ratio=0;
	//Quaternion aimRot;



	// Use this for initialization
	public override void Start () {
        base.Start();

        if (player == null)
        {
            //Debug.Log("Hello i am citty. no player");
            enabled = false;
            return;
        }


        catNma = GetComponent<NavMeshAgent>();
        rwb = GetComponent<RandomWalkBot>();
        charSubt = SubtitleFade.subtitles["CharacterSubtitle"];
        catAnim = GetComponent<Animator>();
        playerCC = player.GetComponent<CharacterController>();
        playerAnim = player.GetComponent<Animator>();

        GameObject noseTransform = CharGameController.getObjectOfHand("nosePackage",CharGameController.hand.LeftHand);
        if (noseTransform != null) {
            nose =noseTransform.gameObject;
        }else
        {
            Debug.Log("Couldn't find nose package");
            enabled = false;
        }
        //initialPos = transform.position;
        //aimRot = Quaternion.LookRotation (player.transform.position,transform.up);

        //TODO uncommment when two object at seethrough buildings script bug is fixed.
        //Timing.RunCoroutine(_lateStart()); 

    }

    IEnumerator<float> _lateStart()
    {
        yield return Timing.WaitForSeconds(1f);

        //SeeCharacterThroughObjects scto = CharGameController.getCamera().GetComponent<SeeCharacterThroughObjects>();
        //if (scto)
        //{

        //    scto.registerToTargets(transform.gameObject);
        //}
    }
	
	// Update is called once per frame
	void Update () {

        if (debug)
        {
            Action();
            debug = false;
        }
	}



    public IEnumerator<float> _startAttack()
    {
        if (used) yield break;

        registerAsUsed();

        disableGame();

        print("startattack");
        pcc.StopToWalk();
        playerCC.enabled = false;
        Timing.RunCoroutine(Vckrs._lookTo(player, transform.position-player.transform.position, 1f));
        charSubt.text = "Ivan: Gel pisi pisi... Bak leziz bir şey var burada."; //TODO remove magic string


        rwb.enabled = false;
        catNma.Stop();

        IEnumerator<float> handler=Timing.RunCoroutine(Vckrs._lookTo(gameObject, player.transform.position-gameObject.transform.position, 1f));
        yield return Timing.WaitUntilDone(handler);
        yield return Timing.WaitForSeconds(2);
        
        catNose.SetActive(true);

        if(nose!=null)
            nose.SetActive(false);
        

        catAnim.SetTrigger("Smell");
        catNose.transform.parent = null;
        playerCC.enabled = true;
        
    }

    public override void Action()
    {
        base.Action();
        Timing.RunCoroutine(_startAttack());
    }

    public void playerFall()
    {
        Timing.RunCoroutine(_playerFall());
    }

    IEnumerator<float> _playerFall()
    {
        trigger.GetComponent<Collider>().enabled = false;
        playerCC.enabled = false;
        playerAnim.SetTrigger("FallBack");
        charSubt.text = "AAAAAA!";
        yield return Timing.WaitForSeconds(3f);

        //Getup
        playerAnim.SetTrigger("FallBack");
        yield return Timing.WaitForSeconds(2f);

        charSubt.text = "Lanet kedi!";

        catNose.SetActive(false);
        catNose.transform.parent = transform;
        catNose.transform.localPosition = new Vector3(0, 0, 1.15f);
        if (nose != null)
            nose.SetActive(true);
        catNma.enabled = true;
        rwb.enabled = true;

        Timing.WaitForSeconds(2);
        charSubt.text = "";

        pcc.ContinueToWalk();



    }


    public void disableGame()
    {
        //Destroy after finish everything
        used = true;
        gameObject.transform.tag = "Untagged";
    }

    public void Jump()
    {
        Timing.RunCoroutine(Vckrs._TweenSinHeight(gameObject,player.transform.position,JumpSpeed,1));
    }

    //    IEnumerator<float> _Jump(){

    //       //print("JUmpppppp");
    //	Vector3 initialPos = transform.position;
    //	float ratio = 0;

    //	while (ratio<1) {
    //		ratio += Time.deltaTime * JumpSpeed;
    //		transform.position = Vector3.Lerp (initialPos, player.transform.position,ratio);
    //		yield return 0;

    //	}


    //}
    public override void activateController()
    {
        gameObject.SetActive(true);
        //SeeCharacterThroughObjects scto= CharGameController.getCamera().GetComponent<SeeCharacterThroughObjects>();
        //if (scto)
        //{
            
        //    scto.registerToTargets(transform.gameObject);
        //}

    }
    public override void deactivateController()
    {
        gameObject.SetActive(false);
        //SeeCharacterThroughObjects scto = CharGameController.getCamera().GetComponent<SeeCharacterThroughObjects>();
        //if (scto)
        //{
        //    scto.removeFromTargets(transform.gameObject);
        //}
    }


}
