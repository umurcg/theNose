using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class RainOverMe : MonoBehaviour {

    public GameObject[] prefabs;
    public GameObject cameraObj;
    public int numberOfSpawnAtAwake = 20;

    public int dropPerPeriod = 1;
    public float period = 1f;
    float timer = 1f;

    GameObject mainChar;

    public float maxLinerForce = 0.3f;
    public float minLinearForce = 0.1f;
    public float maxTorque = 0.3f;
    public float minTorque = 0.1f;

	// Use this for initialization
	void Start () {

        if (prefabs.Length == 0) enabled=false;

        mainChar = CharGameController.getActiveCharacter();

        for(int i = 0; i < numberOfSpawnAtAwake; i++)
        {
            spawnNewDropAtScreen();
        }
        

	}
	
	// Update is called once per frame
	void Update () {

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = period;
            for (int i = 0; i < dropPerPeriod; i++)
                spawnNewDropAtTop();
            
        }

	}

    void spawnNewDropAtScreen()
    {

        GameObject spawnedObject = Instantiate(prefabs[Random.Range(0, prefabs.Length)]) as GameObject;

        float upY = cameraObj.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0, Random.Range(0, Screen.height), 0)).y;
        float maxX = cameraObj.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float minX = cameraObj.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0, 0, 0)).x;

        spawnedObject.transform.position = new Vector3(Random.Range(minX, maxX), upY + 5, mainChar.transform.position.z);

        spawnedObject.SetActive(true);

        ConstantForce cf = spawnedObject.AddComponent<ConstantForce>();
        cf.force = new Vector3(0, -Random.Range(minLinearForce, maxLinerForce), 0);
        cf.torque = new Vector3(Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque));


        //Disable gravity
        spawnedObject.GetComponent<Rigidbody>().useGravity = false;

        //make parent player
        spawnedObject.transform.parent = cameraObj.transform;

        Timing.RunCoroutine(destroyAfterSeconds(spawnedObject, 10f));
    }
    
    void spawnNewDropAtTop()
    {
        GameObject spawnedObject = Instantiate(prefabs[Random.Range(0, prefabs.Length)]) as GameObject;

        float upY = cameraObj.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0, Random.Range( Screen.height, Screen.height+50), 0)).y;
        float maxX = cameraObj.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float minX = cameraObj.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0, 0, 0)).x;

        spawnedObject.SetActive(true);

        spawnedObject.transform.position = new Vector3(Random.Range(minX, maxX), upY + 5, mainChar.transform.position.z);

        ConstantForce cf = spawnedObject.AddComponent<ConstantForce>();
        cf.force = new Vector3(0, -Random.Range(minLinearForce, maxLinerForce), 0);
        cf.torque = new Vector3(Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque));

        //Disable gravity
        spawnedObject.GetComponent<Rigidbody>().useGravity = false;

        //make parent player
        spawnedObject.transform.parent = cameraObj.transform;

        Timing.RunCoroutine(destroyAfterSeconds(spawnedObject, 10f));
    }

    IEnumerator<float> destroyAfterSeconds(GameObject obj,float delay)
    {
        yield return Timing.WaitForSeconds(delay);
        Destroy(obj);
        yield break;
    }
}
