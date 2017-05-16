using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script is controller for merge nose head game
public class NoseHeadMergeGame : MonoBehaviour {

    int numberOfDrawedEdges;
    int numberOfPairs;
    public GameObject messageReciever;
    public string message;

    public bool debugSkipGame = false;



	// Use this for initialization
	void Start () {
        numberOfPairs = GetComponentsInChildren<VertexPair>().Length;
        if (debugSkipGame) drawedAllEdges();

	}
	
	// Update is called once per frame
	void Update () {
        if (numberOfDrawedEdges == numberOfPairs)
        {
            drawedAllEdges();
            enabled = false;
            return;

        }

	}

    void drawedAllEdges()
    {
        Debug.Log("Drawed all edges");
        if (messageReciever == null) return;

        messageReciever.SendMessage(message);

        //Clear edge objects
        LineRendererCylinder[] lrcs = transform.parent.GetComponentsInChildren<LineRendererCylinder>();
        foreach (LineRendererCylinder lrc in lrcs) Destroy(lrc.gameObject);
    }

    public void increaseDrawedEdges()
    {
        numberOfDrawedEdges++;
    }

    


}
