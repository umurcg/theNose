using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

//This script provides functionality of girt pet game script

public class GirtyPetGameScript : MonoBehaviour {

    //Game point variables
    public GameObject pointBar;
    PointBarScript pointBarScript;
    Text pointText;
    float point;

    public float speed = 5;
    public float rotationSpeed = 10f;
    public float approachSpeed = 1f;
    public float runAwayMagnitude = 3f;

    public string[] pointBarNameENG_TR;

    //How much time to wait for approach dog to mouse
    public float timeToApproach = 30f;
    public float pointSpeed=5f;

    //Minimum mouse movement while petting
    public float minimumMouseMovement = 10f;

    //Minimum distance between object and mouse for petting
    public float minimumDistanceForPetting = 2f;

    //Cursor texture when player can pet girty
    public Texture2D petTexture;

    Camera mainCamera;

    //Is running from player
    bool running = false;
    
    //This timer is for triggereng approaching function of girty
    float timer;

    //Can player pet girty now?
    bool canPet = false;

    Animator anim;

    Vector3 previousMousePosition;
    Vector3 previousPosition;

    //Message recieve and message to send when win condition is triggered
    public GameObject messageReciever;
    public string message;


    IEnumerator<float> runHandler;

  

    // Use this for initialization
    void Start () {

        pointBarScript = pointBar.GetComponent<PointBarScript>();
        pointBarScript.setLimits(100, 0);
        pointBarScript.setPoint(0);
        pointBar.SetActive(false);

        if (GlobalController.Instance.getLangueSetting() == GlobalController.Language.ENG)
        {
            pointBarScript.setName(pointBarNameENG_TR[0]);
        }
        else
        {
            pointBarScript.setName(pointBarNameENG_TR[1]);
        }


        anim = GetComponent<Animator>();

        mainCamera = CharGameController.getCamera().GetComponent<Camera>();
      
        //Set position to middle of screen
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2,100));

        //Look to cam
        transform.LookAt(mainCamera.gameObject.transform.position-mainCamera.gameObject.transform.forward);

        //Set timer
        timer = timeToApproach;

        //Set prev positions
        previousPosition = transform.position;
        previousMousePosition = Input.mousePosition;

        
        
    }
	
	// Update is called once per frame
	void Update () {

        //Handles rotation of girty
        rotateTowardsMovementDir();

        //Prevents girty to go outside of screen. Like snake game
        checkObjInScreen(gameObject);

        //Timer counter
        if (timer>0)  timer -= Time.deltaTime;

        //If timer is finished then start approachin to mouse
        if (timer < 0) Timing.RunCoroutine(approachToMouse());

        //If point is above 100 win, and disable update funciton of script
        if (point >= 100)
        {
            enabled = false;
            win();
            return;
        }
    
        if (canPet) petting();

        previousMousePosition = Input.mousePosition;
        previousPosition = transform.position;
    }

    //This function enables player to pet girty and collect points
    void petting()
    {

        CursorImageScript cis = CharGameController.getOwner().GetComponent<CursorImageScript>();

        if (!pointBar.activeSelf)    pointBar.SetActive(true);

        //Mouse movement 
        float deltaMouse=Vector3.Distance(previousMousePosition, Input.mousePosition);

        //If mouse moves too much it scares girty and he runs away
        if (deltaMouse > minimumMouseMovement)
        {
            //Reset point and execute petting
            canPet = false;

            //Set to smell animation false
            anim.SetBool("Smell", false);

            point = 0;

            //Reset external texture
            if (cis.externalTexture == petTexture) cis.externalTexture = null;

            //Deactivate point bar
            pointBar.SetActive(false);

            //Reset timer
            timer = timeToApproach;
            
            return;
        }


        //Can pet if distance smaller than minimumDistanceForPetting
        if (Vector3.Distance(transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition+Vector3.forward*100)) < minimumDistanceForPetting)
        {
            //Set cursor to canpet texture
            if (petTexture != null && cis.externalTexture == null) cis.externalTexture = petTexture;

            if (Input.GetMouseButton(0))
            {
                point += Time.deltaTime * pointSpeed * deltaMouse;
            }
        }
        //Reset external texture
        else if (cis.externalTexture == petTexture) cis.externalTexture = null;

        //Update point text and bar
        pointBarScript.setPoint(point);
    }

    [ContextMenu ("win")]
    void win()
    {
        point = 100;
        //Update point text and bar
        //pointText.text = "%" + ((int)point).ToString();
        pointBarScript.setPoint(point);
        //Reset cursor texture
        CharGameController.getOwner().GetComponent<CursorImageScript>().resetExternalCursor();
        enabled = false;

        messageReciever.SendMessage(message);
    }

    void rotateTowardsMovementDir()
    {
        //Rotate to movement position
        Vector3 movementVector = transform.position - previousPosition;
        if (movementVector.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(
                 transform.rotation,
                 Quaternion.LookRotation(movementVector),
                 Time.deltaTime * rotationSpeed
                );

        }
        else
        {
            //If player can pet then don't look to camera, stay to look mouse
            if (canPet) return;

            //If not rotating then look to camera
            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(-mainCamera.transform.forward),
            Time.deltaTime * rotationSpeed
           );
        }

    }

    void OnMouseOver()
    {
        //If player can pet girty onMouseOver should return immediatly
        if (canPet) return;

        //Timer is reset if player disturb girty
        timer = timeToApproach;
        
        //Run boy run
        run(transform.position, findOppositeDirectionOfMouse() * runAwayMagnitude);

    }

    IEnumerator<float> approachToMouse()
    {
        //Local mouse position for this corouitne
        Vector3 localMousePosition = Input.mousePosition;

        //Set to 0 for disabling update function call again this function
        timer = 0;
        
        //While distance between girty and mouse position is bigger tan sphere collider's 2*r, walk towards mouse 
        while (Vector3.Distance(transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 100))>GetComponent<SphereCollider>().radius*2.5f)
        {
            //Walk towards
            transform.position = Vector3.MoveTowards(transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 100), approachSpeed*Time.deltaTime);

            //Check for mouse delta. If player moves mouse too much while girty is tryinh to approach run away.
            //Magic number is used for that. Sorryyyyyy. But script is already too complicted
            float deltaMouse = Vector3.Distance(localMousePosition, Input.mousePosition);
            if (deltaMouse > 0.25f)
            {                   
                run(transform.position, findOppositeDirectionOfMouse() * runAwayMagnitude);

                //Execure coroutine
                yield break;

            }

            //Update mouse position
            localMousePosition = Input.mousePosition;
            yield return 0;
        }

        //If coroutine reaches this line player wait enough for girt, Now he can pet him
        canPet = true;

        //Set to smell animation
        anim.SetBool("Smell", true);

        yield break;
    }

    //Calculates opposite direction for running away from mouse
    Vector3 findOppositeDirectionOfMouse()
    {
        Vector3 dirIn3d= transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 100);    
        Vector3 dirInPlane = Vector3.ProjectOnPlane(dirIn3d, mainCamera.transform.forward);
        return dirInPlane;
    }


    void run(Vector3 from, Vector3 aim)
    {
        //If already running exit coroutine
        if (running) return;

        runHandler = Timing.RunCoroutine(_run(from, aim));
    }

   IEnumerator<float> _run(Vector3 from, Vector3 aim)
    {               
        //Set running boolean
        running = true;
 
        //Set gameObject to intial position just in case
        Vector3 initialPosition =from;
        gameObject.transform.position = initialPosition;


        float ratio = 0;
        //Calculate time to walk from one point to another point according to distance and speed
        float time = speed / Vector3.Distance(from, from + aim) ;

        //Lerp until complete
        while (ratio < 1)
        {
            ratio += Time.deltaTime * time;
            gameObject.transform.position = Vector3.Lerp(initialPosition,initialPosition+ aim, ratio);
            yield return 0;
        }

        //Set object position to final destination
        gameObject.transform.position = initialPosition + aim;
           
        //Set running boolean false before finishing coroutine
        running = false;
        yield break;
    }

    //Stops run coroutine
    void stopRun()
    {
        Timing.KillCoroutines(runHandler);
        running = false;
    }

    //Checks girty in screen, if not moves it to other side of screen and make it walk to in to screen a little bit
    public void checkObjInScreen(GameObject obj)
    {
        float tol = 10;

        Vector2 objScreenPos = mainCamera.WorldToScreenPoint(obj.transform.position);

        if (objScreenPos.x > Screen.width+tol)
        {
            obj.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(0, objScreenPos.y,100));
            stopRun();
            run(transform.position, mainCamera.gameObject.transform.right*tol);
        }
        else if (objScreenPos.x < 0 - tol)
        {
            obj.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, objScreenPos.y, 100));
            stopRun();
            run(transform.position, - mainCamera.gameObject.transform.right*tol);
        }

        objScreenPos = mainCamera.WorldToScreenPoint(obj.transform.position);
        
        if (objScreenPos.y > Screen.height + tol)
        {
            obj.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(objScreenPos.x, 0, 100));
            stopRun();
            run(transform.position, mainCamera.gameObject.transform.up*tol);
        }
        else if (objScreenPos.y < 0 - tol)
        {

            obj.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(objScreenPos.x, Screen.height, 100));
            stopRun();
            run(transform.position, - mainCamera.gameObject.transform.up*tol);
        }

    }
}
