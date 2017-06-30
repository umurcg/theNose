using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public class FisherStandController : GameController, IClickAction {
    
    public GameObject bucket;
    public GameObject uncollectPosition;
    public OpenDoorLoad odl;
    public DenizEfeGameController degc;

	// Use this for initialization
	public override void Action () {
        Timing.RunCoroutine(_startConversation());   
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator <float> _startConversation()
    {
        sc.callSubtitleWithIndex(0);

        while (subtitle.text != "") yield return 0;

        bucket.GetComponent<CollectableObject>().UnCollect(uncollectPosition.transform.position);
        bucket.GetComponent<SwapMaterials>().deactivate();
        bucket.transform.tag = "Untagged";

        odl.playerCanOpen = true;

        degc.gameObject.SetActive(false);
        
        transform.tag = "Untagged";


        yield break;
    }


}
