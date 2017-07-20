using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditScript : MonoBehaviour {

    public TextAsset credits;
    public string[] lines;
    public GameObject canvas;
    public GameObject textPrefab;
    public float movementSpeed = 1;
    public float instantiateFrequency = 1f;
    public float destroyAfterSeconds = 5f;

    public Camera cam;

    Timer timer;

    int lineIndex;

    // Use this for initialization
    void Awake () {
        string allText = credits.text;
        lines = allText.Split('\n');

        timer = new Timer(instantiateFrequency);
        instantiateLine(getInstantiatePos(), lines[0]);

        if(lines.Length>1)
            lineIndex = 1;
    }
	
	// Update is called once per frame
	void Update () {

        if (timer.ticTac(Time.deltaTime))
        {
            
        }


	}

    Vector2 getInstantiatePos()
    {
        return cam.ScreenToWorldPoint(new Vector2(Screen.width / 2, -10));
    }

    void instantiateLine(Vector3 pos, string text)
    {
        GameObject spawned = Instantiate(textPrefab);
        textPrefab.transform.position = pos;
        textPrefab.transform.parent = canvas.transform;

        Text textComp = spawned.GetComponentInChildren<Text>();
        textComp.text = text;

        MoveWithDirection mwd= spawned.AddComponent<MoveWithDirection>();
        mwd.moveSpeed = movementSpeed;
        mwd.Direction = MoveWithDirection.direction.up;
        mwd.space = Space.Self;

        Destroy(spawned, destroyAfterSeconds);
        

    }

}
