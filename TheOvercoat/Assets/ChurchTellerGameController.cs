using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;


//This script controls curch teller bot. It goes to kovalev and tells about church that he should go.
//Todo set instantiate position and walk position of kovalev
public class ChurchTellerGameController : GameController {

    characterComponents ownerCC;

	// Use this for initialization
	public override void Start () {
        base.Start();
        ownerCC = new characterComponents(gameObject);

        Timing.RunCoroutine(_goTellChurch());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator<float> _goTellChurch()
    {
        playerAnim.SetBool("RightHandAtFace", true);
        pcc.StopToWalk();
        //Walk to player

        //Wait for intilzing subtitiles
        yield return Timing.WaitForSeconds(1f);

        //Call subtitle.
        sc.callSubtitleWithIndex(0);

        ownerCC.navmashagent.SetDestination(player.transform.position + 2*(transform.position-player.transform.position).normalized);
        
        //Kovalev looks churchteller
        Timing.RunCoroutine(Vckrs._lookTo(player, gameObject, 1f));

        //Wait for wfinish walking
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(gameObject));
        yield return Timing.WaitUntilDone(handlerHolder);
        

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

        ownerCC.navmashagent.SetDestination(player.transform.position - 50 * Vector3.forward);

        timer = 10;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return 0;
        }


        Destroy(gameObject);

        yield break;
    }
}
