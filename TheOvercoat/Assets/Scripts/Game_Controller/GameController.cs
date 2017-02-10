using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using MovementEffects;

//This script is parent script of game controllers. 
//It initilizes main variables generally that is used by every game controller.
public class GameController : MonoBehaviour {

    protected GameObject player;
    protected NavMeshAgent playerNma;
    protected Animator playerAnim;
    protected SubtitleCaller sc;
    protected Text subtitle;
    protected PlayerComponentController pcc;
    protected Text narSubtitle;
    protected IEnumerator<float> handlerHolder;


    // Initilize variables of game controller.
    public virtual  void Start () {

        
        player = CharGameController.getActiveCharacter();
        if (player != null)
        {
            playerNma = player.GetComponent<NavMeshAgent>();
            playerAnim = player.GetComponent<Animator>();
            pcc = player.GetComponent<PlayerComponentController>();
        

        }
        subtitle = SubtitleFade.subtitles["CharacterSubtitle"];
        narSubtitle = SubtitleFade.subtitles["NarratorSubtitle"];
        sc = GetComponent<SubtitleCaller>();
    }
	
    public virtual void Awake()
    {

    }

   
 
}
