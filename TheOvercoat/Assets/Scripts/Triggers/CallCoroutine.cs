using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CallCoroutine : MonoBehaviour, IFinishedSwitching, IClickAction, IDirectClick, IWalkLookAnim
{
    public enum callType{ClickAction,DirectClick,Switch,/*EnterTrigger*/NoInterface,WalkLookAnim, OnEnter};
    public callType CallType;


    public GameObject owner;
    public Object passParameter;
    public string methodName;
    public bool destroySelf = false;

    //Check for subtitle. Click action shouldn't work while subtitle is not empty
    public bool preventClickActionWithSubtitle = true;
    Text subtitle;

    private void Awake()
    {
        subtitle = SubtitleFade.subtitles["CharacterSubtitle"];
    }

    public void call()
    {
        //Debug.Log("calling "+methodName);
        if (passParameter == null)
        {
            owner.SendMessage(methodName);
        }else
        {
            owner.SendMessage(methodName, passParameter);
        }

        if (destroySelf) Destroy(this);
    }

    public void finishedSwitching()
    {
        if (CallType == callType.Switch)
            call();
    }

    

    public void Action()
    {
        //print("action");
        if(CallType==callType.ClickAction && (!preventClickActionWithSubtitle || subtitle==null || (subtitle.text=="")))
        call();
    }
    
    //public  void TriggerAction()
    //{
    //    if (InterfaceType == interFaceTpyes.EnterTrigger)
    //        call();
    //}

    public void directClick()
    {
        //Debug.Log("Direct click");
        if (CallType == callType.DirectClick)
            call();
    }

    public void finishedIWLA()
    {
        if (CallType == callType.WalkLookAnim)
            call();
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("entered");
        if(CallType==callType.OnEnter && col.transform.tag=="Player")   call();
    }


}