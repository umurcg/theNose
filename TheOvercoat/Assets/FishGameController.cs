﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MovementEffects;

public class FishGameController : GameController {


    public GameObject finishButton;
    Button finishButtonComp;
    public GameObject UI;
    public Camera cam;
    enum stage {outsideOfWater, serachingForFish, foundFish };
    stage Stage = stage.outsideOfWater;

    public float minSpeed=1000;
    public float maxSpeed=2000;

    public float minLength = 350;

    public float maxCurvature = 15;
    public float minCuravture = 10;

    //Minimum distance for including point to curvature calculation.
    public float curvatureDelta = 0.3f;

    public float fishProbability = 30;
    public int minimumTry=5;


    public GameObject fish;

    Vector2 prevMousePos;

    //List<Vector2> lines;
    List<float> speeds;
    List<Vector2> positions;
    float totalLenght;

    public FishingRotController frc;

    float timer = 0;

    public DenizEfeGameController degc;


    MoveObjectToFocus motf;

    CursorImageScript cis;

    public Material lineMaterial;
    public float lineRadius;

    List<GameObject> linesObject;
    Canvas canv;

    // Use this for initialization
    public override void Start () {

        base.Start();

        cam = CharGameController.getCamera().GetComponent<Camera>();

        speeds = new List<float>();
        //lines = new List<Vector2>();
        positions = new List<Vector2>();


        finishButton = Instantiate(finishButton);
        finishButtonComp = finishButton.GetComponentInChildren<Button>();
        finishButton.transform.parent = UI.transform;
        finishButton.transform.position = new Vector2(Screen.width * 4 / 5, Screen.height * 1 / 5);
        finishButton.GetComponentInChildren<Text>().text = "Finish Fishing";
        finishButtonComp.onClick.AddListener(finishGame);
        finishButtonComp.interactable = false;

        motf = GetComponent<MoveObjectToFocus>();

        linesObject = new List<GameObject>();

        canv = GetComponent<Canvas>();

#if UNITY_EDITOR

        finishButtonComp.interactable = true;

#endif

    }

    private void OnEnable()
    {
        cis = CharGameController.getOwner().GetComponent<CursorImageScript>();
        cis.externalTexture = cis.activeObject;
    }

    private void OnDisable()
    {
        cis.resetExternalCursor();
    }

    // Update is called once per frame
    void Update () {



        if (Stage == stage.outsideOfWater && Input.GetMouseButtonUp(0))
        {
            fish.SetActive(false);
            frc.throwHook();
            Stage = stage.serachingForFish;

        }
        else if (Stage == stage.serachingForFish)
        {
            if (findFish())
            {
                Stage = stage.foundFish;
                frc.fishFound();

            }
        }
        else if(Stage==stage.foundFish)
        {

            if (Input.GetMouseButton(0))
            {
                recordDif();

            }

            if (Input.GetMouseButtonUp(0))
            {

                eraseLine();

                float speed = getMeanofList(speeds);
                float curvature = calculateCurvature();

                Debug.Log("Mouse speed is " + speed + " Curvature is " + curvature + " Length is " + totalLenght);
                Debug.Log("Speed is " + (speed > minSpeed && speed < maxSpeed) + " Curvature is " + (curvature < maxCurvature && curvature > minCuravture) + " length is " + (totalLenght > minLength));


                frc.catchFish();
                Stage = stage.outsideOfWater;


                bool speedConition = (speed > minSpeed && speed < maxSpeed);
                bool curvatureCondition = (curvature < maxCurvature && curvature > minCuravture);
                bool lengthConition = (totalLenght > minLength);


                if ((speed < minSpeed)) {
                    sc.callSubtitleWithIndexTime(1);
                 
                }else if((speed > maxSpeed))
                {
                    sc.callSubtitleWithIndexTime(0);
                }else if ((curvature > maxCurvature))
                {
                    sc.callSubtitleWithIndexTime(4);
                }
                else if (curvature < minCuravture)
                {
                    sc.callSubtitleWithIndexTime(3);
                    
                }
                else if (!lengthConition) {
                    sc.callSubtitleWithIndexTime(2);
                    
                }


                if (speedConition && curvatureCondition && lengthConition)
                {
                    catchedFish();
                }

                speeds.Clear();
                //lines.Clear();
                positions.Clear();
                totalLenght = 0;
                prevMousePos = Vector3.zero;

                if (minimumTry <= 0)
                {
                    if (!finishButtonComp.IsInteractable()) finishButtonComp.interactable = true;
                }
                else
                {
                    minimumTry--;
                }



            }
        }


    }

    void recordDif()
    {
        

        if (prevMousePos == Vector2.zero)
        {
            prevMousePos = Input.mousePosition;
            positions.Add(prevMousePos);
            return;
        }else
        {

            positions.Add(Input.mousePosition);
            float dif = Vector2.Distance((Vector2)Input.mousePosition, prevMousePos);
            totalLenght += dif;
            speeds.Add(dif/Time.deltaTime);


            GameObject line = new GameObject();
            LineRendererCylinder lrc = line.AddComponent<LineRendererCylinder>();

            lrc.setStartAndPos(Input.mousePosition, prevMousePos);
            lrc.setStartAndPos(cam.ScreenToWorldPoint(Input.mousePosition), cam.ScreenToWorldPoint(prevMousePos));



            lrc.radiusOfCylinder = lineRadius;
            lrc.changeRadius(lineRadius);

            if (lineMaterial!=null) lrc.setMaterial(lineMaterial);

            FadeAndDie  fad=line.AddComponent<FadeAndDie>();
            fad.speed = 1f;
            fad.fadeAndDie();

            linesObject.Add(line);

            line.transform.parent = canv.transform;

            //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = cam.ScreenToWorldPoint(Input.mousePosition);

            prevMousePos = Input.mousePosition;
        }
        
    }

    void catchedFish()
    {
        fish.SetActive(true);
        if (!finishButtonComp.IsInteractable())
            finishButtonComp.interactable = true;
    }


    float calculateCurvature()
    {

        //Choose points according to curvatureDelta
        List<Vector2> choosenPoints = new List<Vector2>();

        choosenPoints.Add(positions[0]);

        //Positions list contains each position that is recorded in update function
        for (int i = 1; i < positions.Count; i++)
        {
            //Curvature delta is minimum distance from previous point for acounting next point from positions list. With this condition if user doesn't move his mouse
            //those points are not accounted and doesn't affect mean.
            if (Vector2.Distance(positions[i], choosenPoints[choosenPoints.Count-1]) >= curvatureDelta) choosenPoints.Add(positions[i]);
        }


        //Create lines
        List<Vector2> lines = new List<Vector2>();
        
        for(int i = 1; i < choosenPoints.Count; i++)
        {
            Vector2 line = choosenPoints[i] - choosenPoints[i - 1];
            lines.Add(line);
            //Debug.DrawLine(choosenPoints[i], choosenPoints[i - 1], Color.red, 1000000);
        }


        //Calculate angles
        List<float> angles = new List<float>();

        for (int i = 1; i < lines.Count; i++)
        {
            float angle = 180 - Vector2.Angle(-lines[i - 1], lines[i]);
            angles.Add(angle);
            //Debug.Log(angle);

        }


        return getMeanofList(angles);

    }
    

    float sumList(List<float> list)
    {
        float sum = 0;
        for (var i = 0; i < list.Count; i++)
        {
            sum += list[i];
        }
        return sum;
    }

    float getMeanofList(List<float> list)
    {
        float sum = 0;
        for (var i = 0; i < list.Count; i++)
        {
            sum += list[i];
        }
        return sum / list.Count;
    }

 
    bool findFish()
    {
        bool result = false;
        if (timer < 30 && timer%5>0.5)
        {
            timer += Time.deltaTime;
        } else if (timer > 30)
        {
            result = true;
            timer = 0;
        } else
        {

            int number = (int)Random.Range(0, fishProbability);
            result = (number == 7);

            timer = result ? 0 : timer+Time.deltaTime;        

        }

        return result;
    }
       
    public void finishGame()
    {
        if (degc) degc.fishingGameIsFinished();
        gameObject.SetActive(false);

        finishButton.SetActive(false);

    }

    void eraseLine()
    {
        //foreach(GameObject obj in linesObject)
        //{
        //    Destroy(obj);
        //}

        linesObject.Clear();
    }

}
