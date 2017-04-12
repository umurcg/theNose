using UnityEngine;
using System.Collections;

public class garsonContoller : MonoBehaviour {

    public GameObject[] tables;
    public GameObject garsonPoint;
    public GameObject garson;
    public float timeBetweenWalks = 10;
    characterComponents garsoncc;

    float timer = 0;

	// Use this for initialization
	void Start () {
        garsoncc = new characterComponents(garson);
	}
	
	// Update is called once per frame
	void Update () {

        if (timer < 0)
        {
            int i = Random.Range(0, 2);
            if (i == 0)
            {
                garsoncc.navmashagent.SetDestination(garsonPoint.transform.position);
            }else
            {
                garsoncc.navmashagent.SetDestination(tables[Random.Range(0, tables.Length)].transform.position);
            }
            timer = timeBetweenWalks;
        }else{
            timer -= Time.deltaTime;
        }
	
	}
}
