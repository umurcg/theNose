﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FogGameController : GameController {

    public GameObject Canvas3d;
    
    
    public float breath = 100;
    public float fullBreath = 100;
    public float suffocationSpeed = 5f;

    [HideInInspector]
    public bool inFog = false;

    public PointBarScript bar;
    public GameObject windUI;

    FogController[] fogs;
    int currentFogIndex=0;

    GameObject birdInitialPos;

    public GameObject finishBirdPosition;

    public override void Awake()
    {
        base.Awake();
        fogs = GetComponentsInChildren<FogController>();
        fogs[0].enabled = true;

        bar.enableBar("Breath");
        bar.setLimits(fullBreath,0.0f);
        bar.setPoint(fullBreath);
        windUI=Instantiate(windUI);
        windUI.transform.parent = Canvas3d.transform;
        windUI.transform.position = Canvas3d.transform.position + (Screen.width / 2) * 1 / 5 * windUI.transform.right + (Screen.height / 2) * 1 / 5 * windUI.transform.up;
    }

    //public override void Start()
    //{
    //    base.Start();
    //    finisGame();
    //}

    // Update is called once per frame
    void Update () {

        if (breath <= 0)
        {
            restartGame();
            enabled = false;
            return;
        }

        if (inFog)
        {
            breath -= Time.deltaTime * suffocationSpeed;
            bar.setPoint(breath);
        }else if (breath < fullBreath)
        {
            breath += Time.deltaTime * suffocationSpeed;
            bar.setPoint(breath);
        }
    }

    public void fogIsDestroyed(FogController fc)
    {
        fc.enabled = false;

        if (currentFogIndex + 1 >= fogs.Length)
        {
            finisGame();
            return;
        }else
        {
            currentFogIndex++;
            fogs[currentFogIndex].enabled = true;
        }

        
    }

    [ContextMenu ("Finish")]
    void finisGame()
    {
        CharGameController.movePlayer(finishBirdPosition.transform.position);
        gameObject.SetActive(false);
    }

    [ContextMenu ("Restart")]
    void restartGame()
    {
        GetComponent<LoadScene>().Load();
    }


    public override void activateController()
    {
        base.activateController();
        gameObject.SetActive(true);
    }

    public override void deactivateController()
    {
        base.deactivateController();
        gameObject.SetActive(false);
    }


}
