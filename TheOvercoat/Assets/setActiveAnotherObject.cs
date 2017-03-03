using UnityEngine;
using System.Collections;
using System;


//This script set actives another object 
//It can be called by click action, enter trigger
public class setActiveAnotherObject : MonoBehaviour, IClickAction, IDirectClick {

    public enum callType {Click,DirectClick,OnEnter };
    public callType CallType;
    public GameObject objectToSetActive;

    public bool oneTimeUse=false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (CallType==callType.OnEnter&& col.transform.tag == "Player")
        {
            setActive();
        }
    }

    public void directClick()
    {
        if (CallType == callType.DirectClick) setActive();
    }

    public void Action()
    {
        if (CallType == callType.Click) setActive();
    }

    void setActive()
    {
        objectToSetActive.SetActive(true);

        if (oneTimeUse) Destroy(this);
        ActivateAnotherObject.Disable(gameObject);
    }
}
