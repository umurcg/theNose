using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    protected GameObject player;
    protected NavMeshAgent playerNma;
    protected Animator playerAnim;
    protected SubtitleCaller sc;
    protected Text subtitle;
    protected PlayerComponentController pcc;


    // Use this for initialization
    void Start () {

        player = CharGameController.getActiveCharacter();
        playerNma = player.GetComponent<NavMeshAgent>();
        playerAnim = player.GetComponent<Animator>();
        sc = GetComponent<SubtitleCaller>();
        subtitle = SubtitleFade.subtitles["CharacterSubtitle"];
        pcc = player.GetComponent<PlayerComponentController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
