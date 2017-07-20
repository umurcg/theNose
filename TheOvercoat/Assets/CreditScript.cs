using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditScript : MonoBehaviour {

    public TextAsset credits;
    string[] lines;
    public GameObject canvas;
    public GameObject textPrefab;
    public float movementSpeed = 1;
    public float instantiateFrequency = 1f;
    public float destroyAfterSeconds = 5f;

    public float initialDelat = 5f;

    public Camera cam;

    public float loadDelay = 10f;

    Timer timer;

    int lineIndex;

    // Use this for initialization
    void Awake () {
        string allText = credits.text;
        lines = allText.Split('\n');



    }
	
	// Update is called once per frame
	void Update () {

        if (initialDelat > 0)
        {
            initialDelat -= Time.deltaTime;
            return;
        }

        if (timer == null)
        {
            timer = new Timer(instantiateFrequency);
            instantiateLine(getInstantiatePos(), lines[0]);

        }
        else if (timer.ticTac(Time.deltaTime))
        {
            if (lineIndex + 1 >= lines.Length)
            {
                enabled = false;
                GetComponent<LoadScene>().Load(loadDelay);
                return;
            }

            lineIndex++;
            instantiateLine(getInstantiatePos(), lines[lineIndex]);

        }


	}

    Vector3 getInstantiatePos()
    {
        var pos= cam.ScreenToWorldPoint(new Vector3(Screen.width / 2-textPrefab.GetComponent<RectTransform>().rect.width/2, 0, 0));
        Debug.Log(pos);
        return pos;
    }

    void instantiateLine(Vector3 pos, string text)
    {
        GameObject spawned = Instantiate(textPrefab);
       
        spawned.transform.SetParent(canvas.transform);

        spawned.transform.localPosition = pos;

        Destroy(spawned.GetComponent<DynamicLanguageTexts>());

        Text textComp = spawned.GetComponentInChildren<Text>();
        Debug.Log("Setting text as " + text);
        textComp.text = text;

        

        MoveWithDirection mwd= spawned.AddComponent<MoveWithDirection>();
        mwd.moveSpeed = movementSpeed;
        mwd.Direction = MoveWithDirection.direction.up;
        mwd.space = Space.Self;

        Destroy(spawned, destroyAfterSeconds);
        

    }

}
