using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using MovementEffects;

//This script is parent script of game controllers. 
//It initilizes main variables generally that is used by every game controller.
abstract public class GameController : MonoBehaviour {


    public bool isDisabledAtStart;

    ////Last scenes of sceneList must be same as this array
    //public GlobalController.Scenes[] activateCondition;

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


        updateCharacterVariables();

        assignSubtitles();
        sc = GetComponent<SubtitleCaller>();


        ////TODO test it
        //if (activateCondition.Length != 0)
        //{
        //   for(int i=activateCondition.Length-1;i>-1; i--)
        //    {
        //        if (i >= GlobalController.Instance.sceneList.Count || GlobalController.Instance.sceneList[i] != (int)activateCondition[i])
        //            deactivateController();
        //    }
        //}

    }

    public void assignSubtitles()
    {
        subtitle = SubtitleFade.subtitles["CharacterSubtitle"];
        narSubtitle = SubtitleFade.subtitles["NarratorSubtitle"];
    }

    protected Text getSubt()
    {
        if (subtitle == null) assignSubtitles();
        return subtitle;
    }

    protected Text getNarSubt()
    {
        if (narSubtitle == null) assignSubtitles();
        return narSubtitle;
    }

    //Gets new character and upğdates variables tıo that character object
    public void updateCharacterVariables()
    {
        player = CharGameController.getActiveCharacter();
        if (player != null)
        {
            playerNma = player.GetComponent<NavMeshAgent>();
            playerAnim = player.GetComponent<Animator>();
            pcc = player.GetComponent<PlayerComponentController>();


        }
    }
	
    public virtual void Awake()
    {

        if (isDisabledAtStart)
        {
            deactivateController();
        }
    }

    //These two function controles activate status of game controllers.
    //While every game property have different apparence (for example some of them just scripts, while some of them both gameobject and script)
    //They should decide how to be activated and deactivated themselfs.

    public virtual void activateController() {
        Debug.Log("Activated  "+ transform.name);
    }
    public virtual void deactivateController() {
        Debug.Log("Deactivated  " + transform.name);
    }


 
}
