using UnityEngine;
using System.Collections;

public class CallCoroutine : MonoBehaviour, IFinishedSwitching, IClickAction, IDirectClick
{
    public enum interFaceTpyes{ClickAction,DirectClick,Switch,NoInterface};
    public interFaceTpyes InterfaceType;


    public GameObject owner;
    public string methodName;

    public void call()
    {
        print("call");
        owner.SendMessage(methodName);
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
    
    public void directClick()
    {
        if (InterfaceType == interFaceTpyes.DirectClick)
            call();
    }


}