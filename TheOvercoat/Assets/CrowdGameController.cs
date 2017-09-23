using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class CrowdGameController : GameController, IClickAction {

    public    GameObject stage;
    
	// Use this for initialization
	public override void Start () {

        base.Start();
        //if (CharGameController.getActiveCharacter() == null) {
        //    Destroy(gameObject);
        //    return;
        //}
        //if (CharGameController.getActiveCharacter().transform.name != "Ivan") Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Action()
    {
        Debug.Log("Action");
        askToCrowd();
    }

    public void askToCrowd()
    {
        Timing.RunCoroutine(_askToCrowd());
    }

    IEnumerator<float> _askToCrowd()
    {
        GameObject nearestObject=getNearestPersonToPlayer();
        
            
        Vector3 originalRotationLook = nearestObject.transform.forward;
        handlerHolder =    Timing.RunCoroutine(Vckrs._lookTo(nearestObject, player, 1f));
        yield return Timing.WaitUntilDone(handlerHolder);

        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "") yield return 0;

        Timing.RunCoroutine(Vckrs._lookTo(nearestObject,originalRotationLook, 1f));

        transform.tag = "Untagged";

        enabled = false;

        yield break;
    }

    //Gets nearest object to player between children objects
    GameObject getNearestPersonToPlayer()
    {
        float minDistance = Mathf.Infinity;
        GameObject nearestObject=null;
        int childCount = transform.childCount;

        
        for (int i = 0; i < childCount; i++)
        {
            GameObject childObject = transform.GetChild(i).gameObject;
            float distance = Vector3.Distance(player.transform.position,childObject.transform.position);
            if (distance < minDistance){
                nearestObject = childObject;
                minDistance = distance;
            }
        }
        return nearestObject;
    }

    public override void activateController()
    {
        gameObject.SetActive(true);
        Vckrs.enableAllChildren(stage.transform);

    }
    public override void deactivateController()
    {
        gameObject.SetActive(false);
        Vckrs.disableAllChildren(stage.transform,new List<Transform>());
    }


}
