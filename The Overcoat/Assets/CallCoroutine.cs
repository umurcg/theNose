using UnityEngine;
using System.Collections;

public class CallCoroutine : MonoBehaviour, IFinishedSwitching, IClickAction
{
    public enum interFaceTpyes{ClickAction,Switch,NoInterface};
    public interFaceTpyes InterfaceType;


    public GameObject owner;
    public string methodName;

    public void call()
    {
        owner.SendMessage(methodName);
    }

    public void finishedSwitching()
    {
        if (InterfaceType == interFaceTpyes.Switch)
            call();
    }

    public void Action()
    {
        if(InterfaceType==interFaceTpyes.ClickAction)
        call();
    }
}