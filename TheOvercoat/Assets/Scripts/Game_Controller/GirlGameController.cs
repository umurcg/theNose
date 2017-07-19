using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class GirlGameController : MonoBehaviour {

    public int scoreValue=0;
    public int level;
    public float aimObjectTime = 5f;
    public float obstacleTime = 10f;
    float aimObjectTimer=5f;
    float obstacleTimer=10f;
    public GameObject obstacle;
    public GameObject aimObject;
    public GameObject scoreObj;


    public string message;
    public GameObject recieverObj;

    public float obstacleDelay=5f;
    RectTransform rt;
    Text score;
    public float aimScalePersp;
    public float obsScalePersp;
    public float aimScaleOrth;
    public float obsScaleOrth;
    float aimScale, obsScale;

    bool isPerspective = false;

    public float distanceToCamera = 10f;
    public GameObject kovalev;

    Camera mainCam;


    private void OnEnable()
    {
        CameraFollower cf = CharGameController.getCamera().GetComponent<CameraFollower>();
        if (cf) cf.lockCameraRotation(true);

        CameraController.disableCameraSettings();

    }

    private void OnDisable()
    {

        CameraFollower cf = CharGameController.getCamera().GetComponent<CameraFollower>();
        if (cf) cf.lockCameraRotation(false);

        CameraController.enableCameraSettings();
    }



    // Use this for initialization
    void Start () {

        mainCam = CharGameController.getMainCameraComponent();

        if (!mainCam) Debug.Log("Main cam is null");

        score = scoreObj.GetComponent<Text>();
        rt = GetComponent<RectTransform>();

        MovementWithKeyboard2D mwk = kovalev.GetComponent<MovementWithKeyboard2D>();
        mwk.distanceToCam = distanceToCamera;

        if (CharGameController.getCameraType() == CharGameController.cameraType.Ortographic)
        {
            isPerspective = false;
            aimScale = aimScaleOrth;
            obsScale = obsScaleOrth;
        }
        else
        {
            isPerspective = true;

            aimScale = aimScalePersp;
            obsScale = obsScalePersp;
        }

        setKovalevPositionToInitialPosition();

        

        CameraRotator bew = mainCam.GetComponent<CameraRotator>();
        if (bew) bew.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (obstacleDelay > 0)
        {
            obstacleDelay -= Time.deltaTime;
        }
        else
        {
            obstacleTimer -= Time.deltaTime * level;
          }
        aimObjectTimer -= Time.deltaTime*level;

        
        
        if (aimObjectTimer <= 0)
        {
     

            GameObject obj=Instantiate(aimObject);
            obj.transform.localScale= new Vector3(aimScale, aimScale, aimScale);
            obj.transform.position = transform.position;

            HeartGameObject hgo = obj.GetComponentInChildren<HeartGameObject>();
            hgo.setGirlGameController(this);

            Vector3 screenPosition = mainCam.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width),  Screen.height,distanceToCamera));

            //obj.transform.rotation = Quaternion.LookRotation(-1*transform.up);
            obj.transform.rotation = transform.rotation;
            obj.transform.position = screenPosition;
            obj.transform.parent = transform;
            aimObjectTimer = aimObjectTime;
        }


        if (obstacleTimer <= 0)
        {
      

            GameObject obj = Instantiate(obstacle);
            obj.transform.localScale = new Vector3(obsScale, obsScale, obsScale);
            obj.transform.position = transform.position;

            HeartGameObject hgo = obj.GetComponent<HeartGameObject>();
            hgo.setGirlGameController(this);

            Vector3 screenPosition = mainCam.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height*0.05f, distanceToCamera));

            obj.transform.rotation = Quaternion.LookRotation(transform.up);
            obj.transform.position = screenPosition;
            obj.transform.parent = transform;
            obstacleTimer = aimObjectTime;
        }

        //if (scoreValue < 0)
        //{
        //    finish();
        //}
        checkObjInScreen(kovalev);


    }

    public void checkObjInScreen(GameObject obj)
    {
        Vector3 rightLimit= mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0, distanceToCamera));
        Vector3 leftLimit= mainCam.ScreenToWorldPoint(new Vector3(0, 0, distanceToCamera));

        float width = Vector3.Distance(rightLimit, leftLimit);

        //print((Vector3.Distance(leftLimit, obj.transform.position) +" "+ width));

        if (Vector3.Distance(leftLimit, obj.transform.position) > width)
        {
            obj.transform.position = leftLimit;
        }


        if (Vector3.Distance(rightLimit, obj.transform.position) > width)
        {
            obj.transform.position = rightLimit;
        }

    }

    public void updateScore()
    {
      
        score.text = scoreValue.ToString();
    }

    public IEnumerator<float> _finish(float seconds)
    {
        while (seconds > 0)
        {
            seconds -= Time.deltaTime;
            yield return 0;
        }

        recieverObj.SendMessage(message);

        CameraRotator bew = Camera.main.GetComponent<CameraRotator>();
        if (bew) bew.enabled = true;

        transform.parent.gameObject.SetActive(false);
    }


    public void setKovalevPositionToInitialPosition()
    {
        if(!mainCam) mainCam = CharGameController.getMainCameraComponent();
        kovalev.transform.position =mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0, distanceToCamera));
        //Time.timeScale = 0;
    }



}
