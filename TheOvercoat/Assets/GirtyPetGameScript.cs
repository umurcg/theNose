using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class GirtyPetGameScript : MonoBehaviour {

    Camera mainCamera;
    bool running = false;

    public GameObject pointBar;
    Image pointBarFill;
    Text pointText;

    public float speed = 5;
    public float rotationSpeed = 10f;

    public float approachSpeed = 1f;
    public float timeToApproach = 30f;
    float timer;
    IEnumerator<float> approachCor;
    bool canPet = false;

    public float pointSpeed;

    Vector3 petMp;
    public float point;


    void Awake()
    {
        pointBarFill = pointBar.GetComponent<Image>();
        pointText = pointBar.transform.GetChild(0).GetComponent<Text>();

    }

    // Use this for initialization
    void Start () {

        mainCamera = CharGameController.getCamera().GetComponent<Camera>();
        //Set position to middle of screen
        transform.position = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));

        //Look to cam
        transform.LookAt(mainCamera.gameObject.transform.position-mainCamera.gameObject.transform.forward);

        timer = timeToApproach;

        
    }
	
	// Update is called once per frame
	void Update () {
                
        checkObjInScreen(gameObject);

        if (timer>0)  timer -= Time.deltaTime;
        if (timer < 0) approachCor= Timing.RunCoroutine(approachToMouse());

        if (point >= 100) Debug.Log("win");

        if (canPet)
        {
            //Debug.Log("canpet mouse movement: " + Vector3.Distance(petMp, Input.mousePosition));
            if (Vector3.Distance(petMp, Input.mousePosition) > 10f)
            {
                canPet = false;
                point = 0;
            }

            Debug.Log("mouse obj dist: " + Vector3.Distance(transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition)));
            if (Input.GetMouseButton(0) && Vector3.Distance(transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition))<2f){
                point += Time.deltaTime*pointSpeed* Vector3.Distance(petMp, Input.mousePosition);
            }

            petMp = Input.mousePosition;

            pointText.text = "%"+ point.ToString();
            pointBarFill.fillAmount = point / 100;

        }
        
	}

    void OnMouseOver()
    {
        if (canPet) return;

        timer = timeToApproach;
        
        Vector3 mousePosition = Input.mousePosition;
        Vector3 runDir = transform.position - mainCamera.ScreenToWorldPoint(mousePosition);

        Timing.RunCoroutine(run(transform.position, Vector3.ProjectOnPlane(runDir, mainCamera.transform.forward) * 3));

    }

    IEnumerator<float> approachToMouse()
    {

        Vector3 mousePos = Input.mousePosition;
        timer = 0;
        
        while (Vector3.Distance(transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition))>GetComponent<SphereCollider>().radius*2.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition), approachSpeed*Time.deltaTime);

            if (Vector3.Distance(mousePos, Input.mousePosition) > 0.25f)
            {
                Vector3 runDir = transform.position - mainCamera.ScreenToWorldPoint(mousePos);
                Timing.RunCoroutine(run(transform.position, Vector3.ProjectOnPlane(runDir, mainCamera.transform.forward) * 3));
                yield break;

            }
            mousePos = Input.mousePosition;

            yield return 0;
        }

        petMp = Input.mousePosition;
        canPet = true;
    }


   IEnumerator<float> run(Vector3 from, Vector3 aim)
    {
        if (running) yield break;

        running = true;
  

        //Set look
        IEnumerator<float> rotationHandler=Timing.RunCoroutine(Vckrs._lookTo(gameObject, aim, rotationSpeed));

        Vector3 initialPosition =from;
        gameObject.transform.position = initialPosition;

        float ratio = 0;

        float time = speed / Vector3.Distance(from, from + aim) ;

        while (ratio < 1)
        {
            ratio += Time.deltaTime * time;


            gameObject.transform.position = Vector3.Lerp(initialPosition,initialPosition+ aim, ratio);
            yield return 0;

        }
        gameObject.transform.position = initialPosition + aim;

    

        running = false;

        Timing.KillCoroutines(rotationHandler);
        Timing.RunCoroutine(Vckrs._lookTo(gameObject, -mainCamera.transform.forward, rotationSpeed));

        yield break;
    }




    public void checkObjInScreen(GameObject obj)
    {
        float tol = 10;

        Vector2 objScreenPos = mainCamera.WorldToScreenPoint(obj.transform.position);

        if (objScreenPos.x > Screen.width+tol)
        {
            obj.transform.position = mainCamera.ScreenToWorldPoint(new Vector2(0, objScreenPos.y));
            Timing.RunCoroutine(run(transform.position, mainCamera.gameObject.transform.right*tol));
        }
        else if (objScreenPos.x < 0 - tol)
        {
            obj.transform.position = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, objScreenPos.y));
            Timing.RunCoroutine(run(transform.position, - mainCamera.gameObject.transform.right*tol));
        }

        objScreenPos = mainCamera.WorldToScreenPoint(obj.transform.position);

        if (objScreenPos.y > Screen.height + tol)
        {
            obj.transform.position = mainCamera.ScreenToWorldPoint(new Vector2(objScreenPos.x, 0));
            Timing.RunCoroutine(run(transform.position, mainCamera.gameObject.transform.up*tol));
        }
        else if (objScreenPos.y < 0 - tol)
        {

            obj.transform.position = mainCamera.ScreenToWorldPoint(new Vector2(objScreenPos.x, Screen.height));
            Timing.RunCoroutine(run(transform.position, - mainCamera.gameObject.transform.up*tol));
        }

    }
}
