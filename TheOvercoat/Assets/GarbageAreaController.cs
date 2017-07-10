using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

//Creates reflection of garbage area in front of camera
//User can move every object.
//If it finds treause and click it win conition is broadcasted.
public class GarbageAreaController : MonoBehaviour, IClickAction {

    public GameObject canvas;
    public float scaleSize=30;
    public Button closeButton;
    GameObject reflection;
    PlayerComponentController pcc;

    [HideInInspector]
    public ReyhanGameController rgc;

    private void Update()
    {
        
    }


    public void createReflectionOnCamera()
    {
        reflection = Instantiate(gameObject);

        //Prevent infinite loop
        Destroy(reflection.GetComponent<GarbageAreaController>());
        reflection.transform.parent = canvas.transform;
        canvas.SetActive(true);

        reflection.transform.localScale = Vector3.one * scaleSize;
        reflection.transform.localPosition = Vector3.zero;
        reflection.transform.LookAt(canvas.transform.position - canvas.transform.forward);

        closeButton.gameObject.SetActive(true);
        closeButton.onClick.AddListener(destroyReflection);
        
            
        //Add every child object move with mouse 
        for(int i = 0; i < reflection.transform.childCount; i++)
        {
            GameObject child = reflection.transform.GetChild(i).gameObject;
            child.name = child.name + "_reflection";
            //Debug.Log("Adding move with mouse");
            MoveWithMouseV2 mwm=child.AddComponent<MoveWithMouseV2>();
            mwm.canvas = canvas;
            //mwm.lerp = false;

            Debug.Log(gameObject.transform.GetChild(i).name);

            //Add broadcast script to treasure
            if (gameObject.transform.GetChild(i).name == rgc.treasure.name)
            {
                Debug.Log("Adding broadcast to treusere");
                BroadcastOnClick boc = child.AddComponent<BroadcastOnClick>();
                boc.reciever = rgc.gameObject;
                boc.message = "foundTreasure";
            }

        }

    }

    public void destroyReflection()
    {
        Destroy(reflection);
        pcc.ContinueToWalk();
        gameObject.tag = "ActiveObject";

        closeButton.gameObject.SetActive(false);
        closeButton.onClick.RemoveAllListeners();

        enabled = false;



    }

    public void Action()
    {
        enabled = true;
        createReflectionOnCamera();

        pcc = CharGameController.getActiveCharacter().GetComponent<PlayerComponentController>();
        pcc.StopToWalk();

        

        gameObject.tag = "Untagged";

    }
}
