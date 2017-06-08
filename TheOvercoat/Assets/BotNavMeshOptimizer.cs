using UnityEngine;
using System.Collections;
using System;

//This script makes nav mesh agent highly optimized when agent is out of screen
public class BotNavMeshOptimizer : MonoBehaviour, IVisibility {

    NavMeshAgent nma;

    void Awake()
    {


        nma=GetComponent<NavMeshAgent>();
    }
    
    public void onInvisible()
    {
        nma.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        nma.autoBraking = false;
        nma.autoRepath = false;
        nma.autoRepath = false;
    }

    public void onVisible()
    {
        nma.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        nma.autoBraking = true;
        nma.autoRepath = true;
        nma.autoRepath = true;
    }


}
