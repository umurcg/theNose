using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using MovementEffects;

public class FogGameController : GameController {

    public GameObject Canvas3d;
    
    
    public float breath = 100;
    public float fullBreath = 100;
    public float suffocationSpeed = 5f;

    //[HideInInspector]
    //public bool inFog = false;
    public int numberOfEnteredFogCollider = 0;

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
        currentFogIndex = 0;
        fogs[currentFogIndex].enabled = true;

        bar.enableBar("Breath");
        bar.setLimits(fullBreath,0.0f);
        bar.setPoint(fullBreath);
        windUI=Instantiate(windUI);
        windUI.transform.parent = Canvas3d.transform;
        windUI.transform.position = Canvas3d.transform.position + (Screen.width / 2) * 1 / 5 * windUI.transform.right + (Screen.height / 2) * 1 / 5 * windUI.transform.up;
    }

    public override void Start()
    {
        base.Start();
        //finisGame();
        numberOfEnteredFogCollider = 0;
    }

    // Update is called once per frame
    void Update () {

        if (breath <= 0)
        {
            die();
            enabled = false;
            return;
        }

        //if (inFog)
        if(numberOfEnteredFogCollider>0)
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
    void die()
    {
        Timing.RunCoroutine(_die());
       
    }

    IEnumerator<float> _die()
    {

        

        BirdController bc =player.GetComponent<BirdController>();

        yield return Timing.WaitUntilDone( Timing.RunCoroutine(bc._fall()));

        yield return Timing.WaitForSeconds(2f);

        bc.termianteFall();

        yield return 0;

        GetComponent<LoadScene>().Load();

        

        yield break;
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

    //We should track fogs that bird is entered 
    //It shouldnt be boolean because colldiers of fog can be in intersection
    public void birdIsEnteredInAfog()
    {
        numberOfEnteredFogCollider++;

    }

    public void birdIsExitedFog()
    {

        if(numberOfEnteredFogCollider>0)
            numberOfEnteredFogCollider--;

    }

    public FogController getActiveFogController()
    {
        if (fogs == null) return null;

        return fogs[currentFogIndex];

    }

}
