using MovementEffects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//This script make object talkable if assigned mesh has special subtitiles.


public class BotTalks : MonoBehaviour, IClickAction {

    SubtitleCaller sc;
    

    [System.Serializable]
    public struct meshAndsubtitle
    {
        public Mesh mesh;
        public TextAsset subtitle;
    }

    public meshAndsubtitle[] MeshAndSubtitles;

    private void Start()
    {
        lookForSubtitle();
    }

    public void lookForSubtitle()
    {
        SkinnedMeshRenderer rend = GetComponentInChildren<SkinnedMeshRenderer>();

        meshAndsubtitle mas = getStructWithMesh(rend.sharedMesh);

        if (mas.mesh != null)
        {
            sc=gameObject.AddComponent<SubtitleCaller>();
            SubtitleController controller = gameObject.AddComponent<SubtitleController>();
            controller.textAsset = mas.subtitle;
            //controller.enabled = false;
            controller.importFromTextFile();
            controller.releaseAfterSub = true;
            
            //controller.importFromTextFile();

            //make bot active object
            transform.tag = "ActiveObject";
            GetComponent<MaterialController>().enabled = true;
            GetComponent<SphereCollider>().isTrigger = true;   
        }
    }

    meshAndsubtitle getStructWithMesh(Mesh mesh)
    {
        foreach(meshAndsubtitle mas in MeshAndSubtitles)
        {
            if (mas.mesh == mesh) return mas;
        }

        return new meshAndsubtitle();
    }

    public void Action()
    {
        Timing.RunCoroutine(talkWithMe());

    }

    IEnumerator<float> talkWithMe()
    {
        if (sc.characterSubt.text != "") yield break;

        MoveRandomlyOnNavMesh mronm = GetComponent<MoveRandomlyOnNavMesh>();
        NavMeshAgent nma = GetComponent<NavMeshAgent>();

        mronm.enabled = false;
        nma.isStopped = true;

        GameObject player = CharGameController.getActiveCharacter();
        PlayerComponentController pcc = player.GetComponent<PlayerComponentController>();

        Timing.WaitUntilDone(Timing.RunCoroutine(Vckrs._lookTo(gameObject, player, 1f)));

        pcc.StopToWalk();

        sc.callSubtitleWithIndex(0);

        while (sc.characterSubt.text != "") yield return 0;

        mronm.enabled = true;
        nma.isStopped = false;

        yield break;    
    }

}
