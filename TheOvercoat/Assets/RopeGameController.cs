using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//This script is for rope game.
//Player tries to untie every node of rope with matching same size of I and Os.
public class RopeGameController : MonoBehaviour {

    public int numberOfNode = 4;
    public GameObject ropeObject;
    public GameObject O;
    public GameObject I;
    public GameObject _3DCanvas;
    public float minScalesOI = 30f;
    public float scaleDeltaOI = 5f;
    public float minDistanceBetweenOI=10f;
    public GameObject sculpturerGameContr;
    //public Texture2D stickTexture;

    List<GameObject> ropes;
    List<GameObject> torus;

    public bool reset = false;

    GameObject mainCam;
    Camera cam;
    

	// Use this for initialization
	void Start () {

        mainCam = CharGameController.getCamera();
        cam = mainCam.GetComponent<Camera>();


        ropes = new List<GameObject>();
        torus = new List<GameObject>();

        //minDistanceBetweenOI = O.GetComponent<SphereCollider>().radius + I.GetComponent<SphereCollider>().radius + 1;
        Debug.Log("min dist" + minDistanceBetweenOI);

        Timing.RunCoroutine(createNodes());


    }
	
	// Update is called once per frame
	void Update () {

        if (reset)
        {
            reset = false;
            resetGame();
        }

	}




    IEnumerator<float> createNodes()
    {

        if ((ropes != null && torus != null) && (ropes.Count != 0 || torus.Count != 0)) yield break;

        List<Vector3> usedPositions = new List<Vector3>();

        for (int i = 0; i < numberOfNode; i++)
        {
            GameObject spawnObject = Instantiate(ropeObject) as GameObject;
            spawnObject.transform.parent = _3DCanvas.transform;

            spawnObject.transform.position = cam.ScreenToWorldPoint(new Vector3(Screen.width * 1 / 10 + Screen.width * 1 / 10 * i, Screen.height * 9/10,GlobalController.cameraForwardDistance));

            ropes.Add(spawnObject);


            GameObject spawnedO = Instantiate(O) as GameObject;
            spawnedO.transform.parent = _3DCanvas.transform;
            spawnedO.transform.localScale = Vector3.one * (i * scaleDeltaOI + minScalesOI);
            spawnedO.transform.position = randomPosOnScree(3);
            spawnedO.SetActive(true);
            RopeGameTorusController rgtc = spawnedO.GetComponent<RopeGameTorusController>();
            rgtc.RGC = this;

            torus.Add(spawnedO);
            usedPositions.Add(spawnedO.transform.position);
            
        }

        for (int i = 0; i < numberOfNode; i++)
        {

            //Debug.Log("hEY");
            RopeGameTorusController rgtc = torus[i].GetComponent<RopeGameTorusController>();
            GameObject spawnedI=null;


            //Find suitable position
            Vector3 foundPos = getAvaibleRandomPosition(usedPositions, Screen.height * 1 / 4);

            if (foundPos == Vector3.zero)
            {
                Debug.Log("Couyldnt found proper position");
            }

            spawnedI = Instantiate(I) as GameObject;
            spawnedI.transform.parent = _3DCanvas.transform;
            spawnedI.transform.localScale = Vector3.one * (i * scaleDeltaOI + minScalesOI);
            spawnedI.transform.position = foundPos;
            spawnedI.SetActive(true);

            rgtc.setKey(spawnedI);

            

        }

        //foreach (GameObject t in torus) t.GetComponent<RopeGameTorusController>().enabled = true;


        yield break;
    }


    Vector3 getAvaibleRandomPosition(List<Vector3> usedPositions, float offsetFromScreen, float maximumTry=100)
    {


        float maxX = Screen.width - offsetFromScreen;
        float minX = offsetFromScreen;

        float maxY = Screen.height - offsetFromScreen;
        float minY = offsetFromScreen;

        float foundX = 0;
        float foundY = 0;

        while (maximumTry > 0)
        {
            float randomX = Random.Range(minX, maxX);
            bool tooClose = false;

            foreach(Vector3 pos in usedPositions)
            {
                if (Mathf.Abs(randomX - cam.WorldToScreenPoint(pos).x) < minDistanceBetweenOI)
                {
                    tooClose = true;
                }
            }

            if (!tooClose)
            {
                foundX = randomX;
                break;
            }

            maximumTry--;
        }

        while (maximumTry > 0)
        {
            float randomY = Random.Range(minY, maxY);
            bool tooClose = false;

            foreach (Vector3 pos in usedPositions)
            {
                if (Mathf.Abs(randomY - cam.WorldToScreenPoint(pos).y) < minDistanceBetweenOI)
                {
                    tooClose = true;
                }
            }

            if (!tooClose)
            {
                foundY = randomY;
                break;
            }

            maximumTry--;
        }

        if (maximumTry > 0)
        {
            //Debug.Log("lefft number of try is " + maximumTry);
            return cam.ScreenToWorldPoint(new Vector3(foundX, foundY, GlobalController.cameraForwardDistance));
        }

        return Vector3.zero;


    }

    Vector3 randomPosOnScree(float offset)
    {
        float height = Random.Range(Screen.height/offset, Screen.height- Screen.height / offset);
        float width = Random.Range(Screen.width / offset, Screen.width - Screen.width / offset);

        return cam.ScreenToWorldPoint(new Vector3(width, height,GlobalController.cameraForwardDistance));
    }



    public void untie(object script)
    {
        RopeGameTorusController rgtc = (RopeGameTorusController)script;
        if (!rgtc) return;

        torus.Remove(rgtc.gameObject);
        rgtc.destroyTorusAndKey();

        GameObject lastRope=ropes[ropes.Count - 1];
        ropes.Remove(lastRope);
        Destroy(lastRope);

        if (ropes.Count == 0) win();
        
        
    }

    [ContextMenu ("win")]
    void win()
    {
        sculpturerGameContr.GetComponent<SculpturerGameController>().SendMessage("startGame");
        clearGame();
        gameObject.SetActive(false);
        enabled = false;

    }

    void resetGame()
    {
        clearGame();
        Timing.RunCoroutine(createNodes());
    }

    void clearGame()
    {
        foreach(GameObject r in ropes)
        {
            Destroy(r);
        }

        foreach (GameObject t in torus)
        {
            //Debug.Log(t.GetComponent<RopeGameTorusController>());
            t.GetComponent<RopeGameTorusController>().destroyTorusAndKey();
            
        }

        ropes.Clear();
        torus.Clear();
    }

}
