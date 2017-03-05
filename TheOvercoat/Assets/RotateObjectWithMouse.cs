using UnityEngine;
using System.Collections;

public class RotateObjectWithMouse : MonoBehaviour {

    public enum rotateButton {Nothing, Right, Left, Middle };
    public rotateButton RotateButton = rotateButton.Right;

    public float rotateSpeed = 150;
    public bool invertXY = false;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool rotate = false;

        switch (RotateButton)
        {

            case rotateButton.Left:
                if (Input.GetMouseButton(0)) rotate = true;

                break;

            case rotateButton.Right:
                if (Input.GetMouseButton(1)) rotate = true;
                break;

            case rotateButton.Middle:
      
                if (Input.GetMouseButton(2)) rotate = true;
                break;

            case rotateButton.Nothing:
                rotate = true;
                break;
        }

        if (rotate)
        {
            if (invertXY)
            {
                transform.Rotate(new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * Time.deltaTime * rotateSpeed);
            }else
            {
                transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotateSpeed);
            }

        }
    }

}
