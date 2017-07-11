using UnityEngine;
using System.Collections;

public class RotateObjectWithMouse : MonoBehaviour {

    public enum rotateButton {Nothing, Right, Left, Middle };
    public rotateButton RotateButton = rotateButton.Right;

    public bool rotateOnlyWhenOver=false;
    bool mouseIsOver = false;

    public float rotateSpeed = 150;
    public bool invertXY = false;


    public GameObject rotateAround;

    bool rotate = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        //bool rotate = false;

        switch (RotateButton)
        {

            case rotateButton.Left:
                if (Input.GetMouseButtonDown(0) && (!rotateOnlyWhenOver || mouseIsOver)) rotate = true;
                if (Input.GetMouseButtonUp(0)) rotate = false;
                break;

            case rotateButton.Right:
                if (Input.GetMouseButtonDown(1) && (!rotateOnlyWhenOver || mouseIsOver))
                {
                    rotate = true;
                    //Debug.Log("Mouse is down you can rotate");
                }
                if (Input.GetMouseButtonUp(1))
                {
                    //Debug.Log("Mouse is up you cant rotate");
                    rotate = false;
                }
                break;

            case rotateButton.Middle:
      
                if (Input.GetMouseButtonDown(2) && (!rotateOnlyWhenOver || mouseIsOver)) rotate = true;
                if (Input.GetMouseButtonUp(2)) rotate = false;
                break;

            case rotateButton.Nothing:
                rotate = true;
                break;
        }

        if (rotate) rotateFunc();
    }

    void rotateFunc()
    {
        if (invertXY)
        {
            if (rotateAround != null)
            {
                transform.RotateAround(rotateAround.transform.position, rotateAround.transform.up, Input.GetAxis("Mouse Y"));
                transform.RotateAround(rotateAround.transform.position, rotateAround.transform.right, Input.GetAxis("Mouse X"));
            }
            else
            {
                transform.Rotate(new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * Time.deltaTime * rotateSpeed);
            }
        }
        else
        {
            if (rotateAround != null)
            {
                transform.RotateAround(rotateAround.transform.position, rotateAround.transform.up, Input.GetAxis("Mouse X"));
                transform.RotateAround(rotateAround.transform.position, rotateAround.transform.right, Input.GetAxis("Mouse Y"));
            }
            else
            {
                transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotateSpeed);
            }
        }
    }

    void OnMouseEnter()
    {
        mouseIsOver = true;
    }

    private void OnMouseExit()
    {
        mouseIsOver = false;
    }
}
