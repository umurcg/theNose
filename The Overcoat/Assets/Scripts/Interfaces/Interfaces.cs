using UnityEngine;
using System.Collections;


//This interface is used for click actions. When an active object is clicked Action method will be called.
public interface IClickAction{

	void Action();

}

public interface IEnterTrigger{

	void TriggerAction(Collider  col);

}

public interface ISubtitleTrigger
{

    void callSubtitleWithIndex(int index);
    void callSubtitle();

}



public interface ISubtitleFinishFunction
{

    //This method called when subtitle finishes its job.
    void finishFunction();

}