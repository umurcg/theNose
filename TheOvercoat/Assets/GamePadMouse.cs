using UnityEngine;
using System.Collections;



public class GamePadMouse : MonoBehaviour {

    public string axisX;
    public string axisY;
    public bool invertX,invertY;

    public GameObject gamePadCursor;
    public float speed = 1f;



	// Use this for initialization
	void Start () {
	
	}

    private void OnEnable()
    {
        gamePadCursor.SetActive(true);
    }

    private void OnDisable()
    {
        gamePadCursor.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

        //if(Input.GetAxis("Mouse X") || Input.GetAxis("Mouse Y"))


        float x = Input.GetAxis(axisX);
        float y = Input.GetAxis(axisY);

        if (invertX) x = x * -1;
        if (invertY) y = y * -1;

        if (gamePadCursor.transform.position.x +  x * speed > Screen.width) x = 0;
        if (gamePadCursor.transform.position.y + y * speed > Screen.height) y = 0;

        gamePadCursor.transform.position += (Vector3)((Vector2.right * x + Vector2.up * y)*speed);

	}

    

}
