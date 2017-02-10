﻿using UnityEngine;
using System.Collections;


//This interface is used for click actions. When an active object is clicked Action method will be called.
public interface IClickAction{

	void Action();

}

public interface IClickActionDifferentPos
{
    Vector3 giveMePosition();
}

public interface IDirectClick
{
    void directClick();
}

public interface IFinishGirlGame
{
    void finished();

}

public interface IWalkLookAnim
{
    void finishedIWLA();
}

public interface IFinishedSwitching
{
    void finishedSwitching();
}

public interface ICollectableObjectAction
{
    void startingToCollecting();
    void startingToUncollecting();
    void finishedToUncollecting();
}

public interface IEnterTrigger{

	void TriggerAction(Collider  col);
    void exitTriggerAction(Collider col);

}

//This interface is used to detect is player trying to move.
public interface ITryingTomove
{
    void trying();

}

public interface INearObjectAciton
{
    void noAction();
}

public interface ISubtitleTrigger
{

    void callSubtitleWithIndex(int index);
    void callSubtitle();

    //FUCKKKKKK DESIGN FAULT
    bool isAutomatic();

}



public interface ISubtitleFinishFunction
{

    //This method called when subtitle finishes its job.
    void finishFunction();

}
