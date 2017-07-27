using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

//Creates reflection of garbage area in front of camera
//User can move every object.
//If it finds treause and click it win conition is broadcasted.
public class GarbageAreaController : MonoBehaviour, IClickAction {

    public GameObject canvas;
    //public float scaleSize = 30;
    public GameObject closeButton;
    GameObject reflection;
    PlayerComponentController pcc;
    
    Camera mainCam;

    CursorImageScript cis;

    [HideInInspector]
    public ReyhanGameController rgc;

    private void Start()
    {
        mainCam = CharGameController.getCamera().GetComponent<Camera>();
    }


    public void createReflectionOnCamera()
    {
        if(!mainCam) mainCam = CharGameController.getCamera().GetComponent<Camera>();

        reflection = Instantiate(gameObject);

        //Prevent infinite loop
        Destroy(reflection.GetComponent<GarbageAreaController>());
        reflection.transform.parent = canvas.transform;
        canvas.transform.GetChild(0).gameObject.SetActive(true);

        reflection.transform.localScale = Vector3.one * rgc.reflectionScaleSize;
        reflection.transform.localPosition = Vector3.zero;/* mainCam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2,GlobalController.cameraForwardDistance));*/
        reflection.transform.LookAt(canvas.transform.position - canvas.transform.forward);

        Destroy(reflection.gameObject.GetComponent<MaterialController>());
        Destroy(reflection.gameObject.GetComponent<Collider>());

        if (closeButton == null) closeButton = rgc.spawnedButton;

        Debug.Log("Setting button active "+closeButton.name);
        closeButton.SetActive(true);
        closeButton.GetComponent<Button>().onClick.AddListener(destroyReflection);

        cis = CharGameController.getOwner().GetComponent<CursorImageScript>();


        //Add every child object move with mouse 
        for (int i = 0; i < reflection.transform.childCount; i++)
        {
            GameObject child = reflection.transform.GetChild(i).gameObject;
            child.name = child.name + "_reflection";
            //Debug.Log("Adding move with mouse");
            MoveWithMouseV2 mwm=child.AddComponent<MoveWithMouseV2>();
            mwm.canvas = canvas;
            //mwm.lerp = false;

            //Cursor script
            child.AddComponent<ChangeCursorWhenMouseOver>().text = cis.frontierObject;

            Debug.Log(gameObject.transform.GetChild(i).name);

            //Add broadcast script to treasure
            if (gameObject.transform.GetChild(i).name == rgc.treasure.name)
            {
                //Debug.Log("Adding broadcast to treusere");
                //BroadcastOnClick boc = child.AddComponent<BroadcastOnClick>();
                //boc.reciever = rgc.gameObject;
                //boc.message = "foundTreasure";

                BroadcastOnClick broad = child.AddComponent<BroadcastOnClick>();
                broad.reciever = rgc.gameObject;
                broad.message = "foundTreasure";
                broad.destroyAfterBC = true;
                broad.sendMessageWithObject = true;
            }

        }

    }

    public void destroyReflection()
    {
        if (reflection == null) return;

        Destroy(reflection);
        pcc.ContinueToWalk();
        gameObject.tag = "ActiveObject";

        Debug.Log("Setting button deactive " + closeButton.name);
        closeButton.gameObject.SetActive(false);
        closeButton.GetComponent<Button>().onClick.RemoveAllListeners();

        canvas.transform.GetChild(0).gameObject.SetActive(false);

        reflection = null;

        enabled = false;



    }

    public void Action()
    {
        enabled = true;

        gameObject.tag = "Untagged";

        createReflectionOnCamera();

        pcc = CharGameController.getActiveCharacter().GetComponent<PlayerComponentController>();
        pcc.StopToWalk();

        


    }

    private void OnDisable()
    {
        destroyReflection();
    }
}
