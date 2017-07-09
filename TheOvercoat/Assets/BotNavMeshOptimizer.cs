using UnityEngine;
using System.Collections;
using System;

//This script makes nav mesh agent highly optimized when agent is out of screen
public class BotNavMeshOptimizer : MonoBehaviour, IVisibility {

    UnityEngine.AI.NavMeshAgent nma;

    void Awake()
    {


        nma=GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    
    public void onInvisible()
    {
        nma.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
        nma.autoBraking = false;
        nma.autoRepath = false;
        nma.autoRepath = false;
    }

    public void onVisible()
    {
        nma.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        nma.autoBraking = true;
        nma.autoRepath = true;
        nma.autoRepath = true;
    }


}
