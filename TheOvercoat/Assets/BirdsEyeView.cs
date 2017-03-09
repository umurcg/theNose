using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using System.Linq;
using UnityEngine.UI;

//This script deals with camera movement for map view of city
//User can send location to reciever via this script
//Also it gets doors names for creating ui elements on canvas like building names
public class BirdsEyeView : MonoBehaviour {

    //For sending selected destination
    public GameObject messageReciever;
    public string message;

    public GameObject mainCanvas;
    public GameObject uiTextPrefab;
    public GameObject[] streetAreas;
    List<GameObject> areaList;
    public float maxSize = 50f;
    public float speed = 0.1f;
    public LayerMask mask;
    public float maxZ = 600f;
    public float minZ = -100f;
    public float scrollSpeed = 2f;
    Vector3 initialPosition;
    Quaternion initialRotation;
    float initialSize;

    Dictionary<GameObject, Vector3> doorNames;

    bool isBirdEye = false;

    void Awake()
    {
        areaList = streetAreas.ToList();
        doorNames = new Dictionary<GameObject, Vector3>();
    }

    // Use this for initialization
    void Start () {

        //Timing.RunCoroutine(_goToBirdEye());

    }
	
	// Update is called once per frame

        
	void Update () {

       
        if (isBirdEye)
        {
            // scroll with mouse
            if (Input.mousePosition.y < (Screen.height / 3))
            {
                if (transform.position.z > minZ)
                {
                    transform.position= new Vector3(transform.position.x, transform.position.y, transform.position.z-Time.deltaTime* scrollSpeed*50);
                }
                //Debug.Log("Alttasin");
            }else if (Input.mousePosition.y > (2*Screen.height / 3))
            {
                if (transform.position.z < maxZ)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Time.deltaTime * scrollSpeed*50);
                }
                //Debug.Log("Usttesin");
            }
            else
            {
                //Debug.Log("Ortadasin");
            }


                //TODO add cancel button

                //Update door names positions
                updateDoorNamesPositions();

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit ,Mathf.Infinity,~mask))
            {
              if(areaList.Contains(hit.transform.gameObject)){

                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("You are goint to " + hit.point);
                        getBackToOriginal(hit.point);
                        
                    }

                }else
                {
                    if (Input.GetMouseButtonDown(0))
                    
                        Debug.Log("Unavaible road");
                }
            }
        }
	}

   
    public void goToBirdEye()
    {
        Timing.RunCoroutine(_goToBirdEye());
    }

    IEnumerator<float> _goToBirdEye()
    {
        setStreetsActive(true);
        
        disableEverythingExceptThis(false);

        Camera cam = GetComponent<Camera>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialSize = cam.orthographicSize;

        Vector3 aimPosition = new Vector3(50,0,0);
        Quaternion aimRotation = Quaternion.Euler(90, 0, 0);


        float ratio = 0;
        while (ratio < 1)
        {
            //Debug.Log(ratio);

            transform.position= Vector3.Lerp(initialPosition, aimPosition, ratio);
            transform.rotation = Quaternion.Slerp(initialRotation, aimRotation, ratio);
            cam.orthographicSize = Mathf.Lerp(initialSize, maxSize,ratio);

            ratio += Time.deltaTime * speed;
            yield return 0;
        }

        createMapUI();

        isBirdEye = true;

        //yield return Timing.WaitForSeconds(5f);
        //Timing.RunCoroutine(getBackToOriginal());
        yield break;

    }

    void createMapUI()
    {
        foreach (KeyValuePair<int, OpenDoorLoad> attachStat in OpenDoorLoad.doors)
        {
            if (!attachStat.Value.isLocked())
            {
                string doorName = attachStat.Value.doorName;
                Vector3 doorPos = attachStat.Value.transform.position;
                GameObject UIDoorName = Instantiate(uiTextPrefab, mainCanvas.transform, false) as GameObject;
                UIDoorName.GetComponent<Text>().text = doorName;
                Debug.Log(doorName);
                //UIDoorName.transform.position = GetComponent<Camera>().WorldToScreenPoint(doorPos);
                doorNames.Add(UIDoorName, doorPos);

                Debug.Log(doorName + " is not locked");
            }
        }

        updateDoorNamesPositions();
    }

    void updateDoorNamesPositions()
    {
    
        foreach (KeyValuePair<GameObject,Vector3> doorName in doorNames)
        {
            doorName.Key.transform.position = GetComponent<Camera>().WorldToScreenPoint(doorName.Value);
        }
    }

    void clearUI()
    {
        foreach (KeyValuePair<GameObject, Vector3> doorName in doorNames)
        {
            Destroy(doorName.Key);
        }
        doorNames.Clear();
    }

    void setStreetsActive(bool b)
    {
        //for (int i = 0; i < streetAreas.transform.childCount; i++)
        //{
        //    streetAreas.transform.GetChild(i).gameObject.SetActive(b);
        //}

        foreach (GameObject area in streetAreas)
            area.SetActive(b);
    }

    public void getBackToOriginal(Vector3 resultPos)
    {
        if (isBirdEye)
            Timing.RunCoroutine(_getBackToOriginal(resultPos));
    }

    IEnumerator<float> _getBackToOriginal(Vector3 resultPos)
    {
        clearUI();

        isBirdEye = false;
        Camera cam = GetComponent<Camera>();

        Vector3 curPos = transform.position;
        Quaternion curRot = transform.rotation;


        float ratio = 0;
        while (ratio < 1)
        {
            //Debug.Log(ratio);

            transform.position = Vector3.Lerp(curPos,initialPosition, ratio);
            transform.rotation = Quaternion.Slerp(curRot, initialRotation, ratio);
            cam.orthographicSize = Mathf.Lerp(maxSize, initialSize,  ratio);

            ratio += Time.deltaTime * speed;
            yield return 0;
        }

        disableEverythingExceptThis(true);

        if (resultPos != Vector3.zero)
            messageReciever.SendMessage(message, resultPos);

        setStreetsActive(false);

        yield break;

    }

    //Disable or enable every script except this one.
    void disableEverythingExceptThis(bool b)
    {
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
            {
                script.enabled = b;
            }
        }
    }
}
