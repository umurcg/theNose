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
    public float aimScale;
    public float obsScale;
    GameObject playerObject;


    // Use this for initialization
	void Start () {
        //float aimObjectTimer = aimObjectTime;
        //float obstacleTimer = obstacleTime;
        score = scoreObj.GetComponent<Text>();
        rt = GetComponent<RectTransform>();
        playerObject = transform.GetChild(0).gameObject;


        setKovalevPositionToInitialPosition();


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

            Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width),  Screen.height,0));
            
            obj.transform.rotation = Quaternion.LookRotation(transform.up);
            obj.transform.position = screenPosition;
            obj.transform.parent = transform;
            aimObjectTimer = aimObjectTime;
        }


        if (obstacleTimer <= 0)
        {
      

            GameObject obj = Instantiate(obstacle);
            obj.transform.localScale = new Vector3(obsScale, obsScale, obsScale);
            obj.transform.position = transform.position;

            Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), 30, 0));

            obj.transform.rotation = Quaternion.LookRotation(transform.up);
            obj.transform.position = screenPosition;
            obj.transform.parent = transform;
            obstacleTimer = aimObjectTime;
        }

        //if (scoreValue < 0)
        //{
        //    finish();
        //}
        checkObjInScreen(playerObject);


    }

    public void checkObjInScreen(GameObject obj)
    {
        Vector3 rightLimit= Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        Vector3 leftLimit= Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));

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

        transform.parent.gameObject.SetActive(false);
    }


    public void setKovalevPositionToInitialPosition()
    {
        playerObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
    }

}
