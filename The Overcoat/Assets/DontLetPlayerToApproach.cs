using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

//This scripts stops player when he is trying to aproach owner object.
//While he is trying to approach him if animation is exist it triggers animation
//If subtitle exists also it shows subtitle while preventing player appraoch
//It is written for dog.

public class DontLetPlayerToApproach : MonoBehaviour {

    
    public string animName;
    public string[] subtitles;

    //Delay before and after showing subtitle.
    public float delay = 1f;

    characterComponents owner;
    characterComponents player;
    Text subt;
    PlayerComponentController pcc;

    IEnumerator<float> handler;

    RandomWalkAndAnimate rwaa;



	// Use this for initialization
	void Start () {
        GameObject playerObject = CharGameController.getActiveCharacter();
        if (!playerObject) enabled = false;
        player = new characterComponents(playerObject);
        pcc = player.player.GetComponent<PlayerComponentController>();

        owner = new characterComponents(gameObject);

        subt = SubtitleFade.subtitles["CharacterSubtitle"];

        if (!subt) enabled = false;

        //This part is dog. It has an ai script and it should be disabled before prevent function is called.
         rwaa = GetComponent<RandomWalkAndAnimate>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator<float> _prevent()
    {
        //print("prevent");

        enabled = false;
     
        if (rwaa) rwaa.enabled = false;

        pcc.StopToWalk();
        player.navmashagent.Stop();
        owner.navmashagent.Stop();

        handler = Timing.RunCoroutine(Vckrs._lookTo(gameObject, player.player.transform.position - transform.position, 1f));
        yield return Timing.WaitUntilDone(handler);

        owner.animator.SetBool(animName, true);
        yield return Timing.WaitForSeconds(delay);

        if (subtitles.Length > 0)
        {
            for (int i=0;i<subtitles.Length;i++)
            {
                
                subt.text = subtitles[i];
                //Wait for mouse click
                while (!Input.GetMouseButtonUp(0)) yield return 0;
                //Delay for getting inputs
                yield return 0;
            }
        }

        subt.text = "";

        player.navmashagent.Resume();
        player.navmashagent.SetDestination(player.player.transform.position+transform.forward*5);
        


        handler = Timing.RunCoroutine(Vckrs.waitUntilStop(player.player, 0));
        yield return Timing.WaitUntilDone(handler);

        owner.animator.SetBool(animName, false);
        pcc.ContinueToWalk();

        if (rwaa) rwaa.enabled = true;


        enabled = true;

        yield break;

        
    } 

    void OnTriggerEnter(Collider col)
    {

        Timing.RunCoroutine(_prevent());
    }

}
