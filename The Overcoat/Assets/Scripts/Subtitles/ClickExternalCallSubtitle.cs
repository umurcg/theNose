using UnityEngine;
using System.Collections;

//This script calls subtitle when iclikaction is called.
//You can specify owner object of subtitleController and subtitleCaller
//If you not specify index it calls first subtitle.

public class ClickExternalCallSubtitle : MonoBehaviour, IClickAction,IClickActionDifferentPos {

    public GameObject owner;
    public int index=-1;
    public bool destroyItself;
    bool destroyed=false;
    public Vector3 walkOffset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    

	}

    public Vector3 giveMePosition()
    {
        return transform.position+ walkOffset;
    }

    public void Destroy()
    {
        destroyed = true;
        this.enabled = false;
    }

    public void Action()
    {
        if (destroyed)
            return;

        if (owner == null)
        {
            owner = gameObject;
        }
        SubtitleCaller sc = owner.GetComponent<SubtitleCaller>();
        if (index == -1)
        {
            sc.callSubtitle();
        }else
        {
            sc.callSubtitleWithIndex(index);
        }

        if (destroyItself)
        {
            destroyed = true;
            this.enabled = false;
        }
    }
}
