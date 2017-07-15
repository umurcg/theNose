using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadCastOnDestroy : Broadcast {

    private void OnDestroy()
    {
        sendMessage();
    }
}
