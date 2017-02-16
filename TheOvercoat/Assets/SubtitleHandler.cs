using UnityEngine;
using System.Collections;

public class SubtitleHandler : MonoBehaviour, IClickAction{

    //Use this script when you need just put subtitile to active object.
    //Use this if you only have one subtitile controller.
    //No need subtitle caller


    public void Action()
    {
        Debug.Log("Action");
        SubtitleController  sc = GetComponent<SubtitleController>();
        if (sc == null) return;
        sc.startSubtitle();       
        
        
    }
}
