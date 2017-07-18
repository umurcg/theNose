using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UntagAfterAction : MonoBehaviour, IClickAction {

    public GameObject objectToUntag;
    public bool destroyAfterUsed = true;

    public void Action()
    {
        objectToUntag.tag = "Untagged";
        if (destroyAfterUsed) Destroy(this);
    }
}
