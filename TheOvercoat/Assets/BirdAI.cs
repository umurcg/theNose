using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

public class BirdAI : MonoBehaviour, IVisibility
{

    public float distanceFromCam;
    public float speed;
    public Camera cam;
    CharacterController cc;

    Timer timer;
    public float minimumTime, maximumTime;

    public float heightOffset, widthOffset;

    public bool visible = false;
    bool waitingForBecomeInvisible = false;


    // Use this for initialization
    void Start () {
        cam=CharGameController.getCamera().GetComponent<Camera>();
        cc = GetComponent<CharacterController>();

        timer = new Timer(UnityEngine.Random.Range(minimumTime, maximumTime));


	}


	
	// Update is called once per frame
	void Update () {

        if(timer==null) timer = new Timer(UnityEngine.Random.Range(minimumTime, maximumTime));

        cc.Move(transform.forward * speed * Time.deltaTime);


        if(waitingForBecomeInvisible && !visible)
        {
            
            waitingForBecomeInvisible = false;
            passInFrontOfCamera();
        }
        else if(waitingForBecomeInvisible)
        {
            Debug.Log("Waitin");
        }

        if (timer.ticTac(Time.deltaTime))
        {
            passInFrontOfCamera();
        }
	}

    public void passInFrontOfCamera()
    {
        if (visible)
        {
            waitingForBecomeInvisible = true;
            return;
        }


        Vector3 pos, lookPos;

        generatePositionAndLookPos(out pos, out lookPos);

        //pos.y = distanceFromCam;
        transform.position = pos;
        
        transform.LookAt(lookPos);

        timer = new Timer(UnityEngine.Random.Range(minimumTime, maximumTime));


    }

    //Generates a position in front of camera

    //TODO fix it
    void generatePositionAndLookPos(out Vector3 pos, out Vector3 lookPos)
    {
        //distanceFromCam = cam.nearClipPlane;

        var upperEdge = cam.ScreenToWorldPoint(new Vector3(UnityEngine.Random.Range(0-widthOffset, Screen.width+ widthOffset), Screen.height+ heightOffset, distanceFromCam));

        var rightEdge= cam.ScreenToWorldPoint(new Vector3(Screen.width+ widthOffset, UnityEngine.Random.Range(0- heightOffset, Screen.height+ heightOffset), distanceFromCam));

        var bottomEdge= cam.ScreenToWorldPoint(new Vector3(UnityEngine.Random.Range(-widthOffset, Screen.width+ widthOffset), 0- heightOffset, distanceFromCam));

        var leftEdge = cam.ScreenToWorldPoint(new Vector3(-widthOffset, UnityEngine.Random.Range(0- heightOffset, Screen.height+ heightOffset), distanceFromCam));

        List<Vector3> edges = new List<Vector3>() {upperEdge,rightEdge,bottomEdge,leftEdge };

        int randomInt = UnityEngine.Random.Range(0, edges.Count);
        pos = edges[randomInt];
        edges.RemoveAt(randomInt);

        randomInt = UnityEngine.Random.Range(0, edges.Count);
        lookPos = edges[randomInt];
        lookPos.y = pos.y;

        //Vckrs.testPosition(pos);
        //Vckrs.testPosition(lookPos);
        

    }


    public void onVisible()
    {
        visible = true;
    }

    public void onInvisible()
    {
        visible = false;
    }
}

[CustomEditor(typeof(BirdAI), true)]
public class BirdAIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BirdAI script = (BirdAI)target;
        if (GUILayout.Button("TestDefault zoom "))
        {
            script.passInFrontOfCamera();

        }

    }
}