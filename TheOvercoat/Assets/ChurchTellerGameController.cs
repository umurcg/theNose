using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;


//This script controls curch teller bot. It goes to kovalev and tells about church that he should go.
//Todo set instantiate position and walk position of kovalev
public class ChurchTellerGameController : GameController {

    characterComponents ownerCC;

    public GameObject reyhan;
    bool waitingForReyhanDumptMe = false;


	// Use this for initialization
	public override void Start () {
        base.Start();
        ownerCC = new characterComponents(gameObject);
        //Debug.Log("helooooooooooooooooo");


        setInitialPos();

        Timing.RunCoroutine(_goTellChurch());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //TODO If you do not disable navmesh before changin position it makes an odd effect. Learn why it is. But for now it is working with disabling and enablign it.
    public void setInitialPos()
    {
        ownerCC.navmashagent.enabled = false;

        bool foundPosition = Vckrs.setPositionToOutsideOfCameraAndOnNavmesh(gameObject,player.transform.position ,100, CharGameController.getMainCameraComponent(),30,60);
        if (foundPosition == false)
        {
            Debug.Log("Couldnt generate position, so just setting object to forward of player by 20");
            Vector3 pos = CharGameController.getActiveCharacterPosition() + CharGameController.getActiveCharacter().transform.forward * 20;
            transform.position = pos;
        }

        ownerCC.navmashagent.enabled = true;
    }

    IEnumerator<float> _goTellChurch()
    {

        yield return 0;



        playerAnim.SetBool("RightHandAtFace", true);
        pcc.StopToWalk();
        //Walk to player

        //Wait for intilzing subtitiles
        yield return Timing.WaitForSeconds(1f);

        //Call subtitle.
        sc.callSubtitleWithIndex(0);
        

        //Vckrs.testPosition(player.transform.position + 2 * (transform.position - player.transform.position).normalized);
        ownerCC.navmashagent.SetDestination(player.transform.position + 5*(transform.position-player.transform.position).normalized);
        
        //Kovalev looks churchteller
        Timing.RunCoroutine(Vckrs._lookTo(player, gameObject, 1f));

        //Wait for wfinish walking
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(gameObject));
        yield return Timing.WaitUntilDone(handlerHolder);

        while (subtitle.text != "") yield return 0;

        //Call subtitle.
        sc.callSubtitleWithIndex(1);


        while (subtitle.text != "") yield return 0;

        //Show face
        playerAnim.SetBool("RightHandAtFace", false);
        float timer = 4;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return 0;
        }
        playerAnim.SetBool("RightHandAtFace", true);

        //Call subtitle.
        sc.callSubtitleWithIndex(2);

        while (subtitle.text != "") yield return 0;

        Vector3 randomPos = Vckrs.generateRandomPositionOnCircle(transform.position, 200f);

        registerAsUsed();

        Vckrs.findNearestPositionOnNavMesh(randomPos, ownerCC.navmashagent.areaMask, 50f, out randomPos);

        ownerCC.navmashagent.isStopped = false;
        ownerCC.navmashagent.SetDestination(randomPos);


        Renderer rend = ownerCC.player.GetComponentInChildren<Renderer>();

        timer = 30;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            Debug.Log(rend.isVisible);

            yield return 0;
        }


        yield return 0;

        if (rend.isVisible == true) yield return 0;

        Debug.Log("Albpran is destroyed");


        Destroy(gameObject);

        yield break;
    }

    public override void gameIsUsed()
    {
        base.gameIsUsed();
        Destroy(gameObject);
    }

    public override void activateController()
    {
        base.activateController();
        //Debug.Log("TAKE ME TO THE TO CHURCH");

        if (!reyhan.activeSelf) 
        {
            gameObject.SetActive(true);
        }
        else
        {
            waitingForReyhanDumptMe = true;
        }
    }

    public void tryToActivate()
    {
        if (waitingForReyhanDumptMe) gameObject.SetActive(true);
    }



    public override void deactivateController()
    {
        base.deactivateController();
        gameObject.SetActive(false);
    }
}
