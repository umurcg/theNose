using UnityEngine;
using System.Collections;

public class CallCoroutine : MonoBehaviour, IFinishedSwitching, IClickAction, IDirectClick, IWalkLookAnim
{
    public enum interFaceTpyes{ClickAction,DirectClick,Switch,/*EnterTrigger*/NoInterface,WalkLookAnim};
    public interFaceTpyes InterfaceType;


    public GameObject owner;
    public Object passParameter;
    public string methodName;

    public void call()
    {
        //print("call");
        if (passParameter == null)
        {
            owner.SendMessage(methodName);
        }else
        {
            owner.SendMessage(methodName, passParameter);
        }
    }

    public void finishedSwitching()
    {
        if (InterfaceType == interFaceTpyes.Switch)
            call();
    }

    

    public void Action()
    {
        print("action");
        if(InterfaceType==interFaceTpyes.ClickAction)
        call();
    }
    
    //public  void TriggerAction()
    //{
    //    if (InterfaceType == interFaceTpyes.EnterTrigger)
    //        call();
    //}

    public void directClick()
    {
        if (InterfaceType == interFaceTpyes.DirectClick)
            call();
    }

    public void finishedIWLA()
    {
        if (InterfaceType == interFaceTpyes.WalkLookAnim)
            call();
    }


}