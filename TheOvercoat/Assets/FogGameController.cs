using UnityEngine;
using System.Collections;

public class FogGameController : MonoBehaviour {

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

    private void Awake()
    {
        fogs = GetComponentsInChildren<FogController>();
        fogs[0].enabled = true;

        bar.enableBar("Breath");
        bar.setLimits(fullBreath,0.0f);
        bar.setPoint(fullBreath);
        windUI=Instantiate(windUI);
        windUI.transform.parent = Canvas3d.transform;
        windUI.transform.position = Canvas3d.transform.position + (Screen.width / 2) * 1 / 5 * windUI.transform.right + (Screen.height / 2) * 1 / 5 * windUI.transform.up;
    }

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

    void finisGame()
    {

    }

    void restartGame()
    {

    }



}
