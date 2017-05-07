using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class IvanHouseGameController : GameController {
    public GameObject Praskovaya,  BigBread, BreadPH, Nose,  door, bread, aim1;
    public Vector3 ivanFirstPosition;

    Animator praskovayaAnim;
    NavMeshAgent praskovayaNma;


    public override void Awake()
    {   
        base.Awake();

        if (GlobalController.Instance == null)
        {
            Debug.Log("No global controller instance");
            return;
        }

        if (GlobalController.Instance.sceneList.Count != 1 || GlobalController.Instance.sceneList[GlobalController.Instance.sceneList.Count - 1] != (int)GlobalController.Scenes.City)
        {
            Debug.Log("Deactivating controller because you are not coming from city first time");
            deactivateController();
            return;
        }



    }

    // Use this for initialization
    public override void Start () {


        //Set ivan as a character and put it on true positon
        CharGameController.setCharacter("Ivan");
        CharGameController.movePlayer(ivanFirstPosition);
        CharGameController.getCamera().GetComponent<CameraFollower>().updateTarget();
        CharGameController.getCamera().GetComponent<CameraFollower>().fixRelativeToDefault();
        CharGameController.getCamera().SetActive(true);
        base.Start();
        player.transform.LookAt(Vector3.forward);


        Debug.Log("Start");
                
        playerNma.enabled = false;
        praskovayaAnim = Praskovaya.GetComponent<Animator>();
        praskovayaNma = Praskovaya.GetComponent<NavMeshAgent>();

        Timing.RunCoroutine(_wakeUpScene());

        //Timing.RunCoroutine(_startBreadGame());
        //Timing.RunCoroutine(_noseDrop());
    }
	
 
	// Update is called once per frame
	void Update () {
	   
	}

    IEnumerator<float> _wakeUpScene()
    {

        //GameObject canvas=GameObject.FindGameObjectWithTag("TutorialCanvas");
        //if (canvas != null)
        //{
        //    canvas.GetComponent<TutorailCanvas>().startFullTutorial(10f);
        //}

        //Debug.Log("Wakeupscene");
        //fading in;
        //handlerHolder = Timing.RunCoroutine(Vckrs._fadeInfadeOut(blackScreen,0.5f));

        //Setting animations and stop player
        praskovayaAnim.SetBool("Hands", true);
        playerAnim.SetBool("GetOffBed", true);
        pcc.StopToWalk();
        //yield return Timing.WaitUntilDone(handlerHolder);

        yield return Timing.WaitForSeconds(6);


        //Praskovaya walks to ivan
        praskovayaAnim.SetBool("Hands", false);
        Vector3 praskovayaFirstPos = Praskovaya.transform.position;
        praskovayaNma.SetDestination(player.transform.position - Vector3.right *2);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Praskovaya));
        yield return Timing.WaitUntilDone(handlerHolder);

        //Talk
        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "")
        {
            yield return 0;
        }

        //praskovaya returns to hr poistion
        praskovayaNma.SetDestination(praskovayaFirstPos);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Praskovaya));

        yield return Timing.WaitForSeconds(1);

        //Ivan get up
        playerAnim.SetTrigger("GetOffContinue");
      

        yield return Timing.WaitUntilDone(handlerHolder);

        handlerHolder=Timing.RunCoroutine(Vckrs._lookTo(Praskovaya, -Praskovaya.transform.right, 1f));
        yield return Timing.WaitUntilDone(handlerHolder);
        praskovayaAnim.SetBool("Hands", true);

        sc.callSubtitleWithIndex(1);
        while (subtitle.text != "")
        {
            yield return 0;
        }
        playerAnim.SetBool("GetOffBed", false);
        yield return Timing.WaitForSeconds(2);
        
        Timing.RunCoroutine(Vckrs._Tween(player, aim1.transform.position, 1f));
        yield return Timing.WaitForSeconds(4);
       
        playerNma.enabled = true;
        pcc.ContinueToWalk();

    }

    public void startBreadGame()
    {
        //Debug.Log("I am in collected arrau  "+CollectableObject.collected[0].transform.name+ " "+ CollectableObject.collected[0].transform.position);
        //Debug.Log("I am bread reference  " + BigBread.transform.name + " " + BigBread.transform.position);


        if (CollectableObject.collected.Contains(bread))
        {
            
            Timing.RunCoroutine(_startBreadGame());

        }
    }

    IEnumerator<float> _startBreadGame()
    {
        
      
        pcc.StopToWalk();

        while (WalkLookAnim.activeScript == null) yield return 0;
        WalkLookAnim.activeScript.lockSit = true;

        GameObject smallBread = CollectableObject.collected[0];
        CollectableObject co = smallBread.GetComponent<CollectableObject>();
        //co.UnCollect(player.transform.position + player.transform.forward * 1+player.transform.up*2);
        co.UnCollect(BreadPH.transform.position);
        sc.callSubtitleWithIndexTime(0);
        
        while (narSubtitle.text[0] != ' ')
        {
            yield return 0;

        }
        pcc.StopToWalk();


        BigBread.transform.position = Camera.main.gameObject.transform.position+ Camera.main.gameObject.transform.forward;
        BigBread.SetActive(true);
    

    }

    public void noseDrop()
    {
        Timing.RunCoroutine(_noseDrop());
    }

    IEnumerator<float> _noseDrop()
    {
        

        //pcc.ContinueToWalk();
        Nose.SetActive(true);
        Rigidbody rb = Nose.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.right*2-Vector3.forward, ForceMode.Impulse);

        yield return Timing.WaitForSeconds(3);

        praskovayaAnim.SetBool("Hands", false);

        sc.callSubtitleWithIndex(3);
        while (subtitle.text != "")
        {
            yield return 0;
        }


        praskovayaNma.SetDestination(Nose.transform.position - Vector3.forward);
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(Praskovaya));
              
        yield return Timing.WaitUntilDone(handlerHolder);

        Timing.RunCoroutine(Vckrs._lookTo(Praskovaya,Nose, 1f));

        if (WalkLookAnim.activeScript != null)
        {
            WalkLookAnim wla = WalkLookAnim.activeScript;
            handlerHolder = Timing.RunCoroutine(WalkLookAnim.activeScript._getUp());
            yield return Timing.WaitUntilDone(handlerHolder);
            wla.lockSit = false;
            pcc.StopToWalk();
        }
        
       

        playerNma.enabled = true;
        playerNma.SetDestination(Nose.transform.position - Vector3.right);

        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(player));
        yield return Timing.WaitUntilDone(handlerHolder);

        Timing.RunCoroutine(Vckrs._lookTo(player, Nose, 1f));

        rb.isKinematic = true;



        sc.callSubtitleWithIndex(4);
        while (subtitle.text != "")
        {
            yield return 0;
        }

        pcc.ContinueToWalk();
    }

    public void swapNose(Object caller)
    {
        Timing.RunCoroutine(_swapNose(caller));
    }

    IEnumerator<float> _swapNose(Object caller)
    {
        pcc.StopToWalk();

        GameObject drawer = (GameObject)caller;

        playerAnim.SetBool("Hands",true);
        yield return Timing.WaitForSeconds(4.0f);

        Nose.SetActive(false);
        GameObject nosePackage=CharGameController.getObjectOfHand("nosePackage", CharGameController.hand.LeftHand);
        nosePackage.SetActive(true);


        playerAnim.SetBool("Hands", false);

        ActivateAnotherObject.Disable(drawer);
        ActivateAnotherObject.Activate(door);
        door.GetComponent<OpenDoorLoad>().playerCanOpen = true;

        pcc.ContinueToWalk();
        yield break;
    }
    public override void activateController()
    {
        base.activateController();
    }
    public override void deactivateController()
    {
        base.deactivateController();
        bread.transform.position = BreadPH.transform.position;
        //bread.GetComponent<CollectableObject>().enabled = false;
        ActivateAnotherObject.Disable(bread);
        door.GetComponent<OpenDoorLoad>().playerCanOpen = true;
        ActivateAnotherObject.Activate(door);
        Destroy(this);

    }

}
