using UnityEngine;
using System.Collections;

public class MouseDrag : MonoBehaviour
{

    bool touched = false;
    public float speed = 10f;
    public bool invertMouse=false;
    //public Vector3 offset ;


    //for child scripts


    // Use this for initialization
    protected void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (invertMouse)
        {
            transform.position += new Vector3(0, Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X")) * Time.deltaTime * speed;
        }
        else
        {
            transform.position += new Vector3(0, Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * Time.deltaTime * speed;
        }
    }




    public virtual void dragObject()
    {

        //transform.position += new Vector3 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"), 0) * Time.deltaTime * speed;

        transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * speed);

        //		if (Input.GetAxis ("Mouse X") !=0|| Input.GetAxis ("Mouse Y")!=0) {
        //			transform.position += (transform.up/3-transform.forward)* Time.deltaTime*speed;
        //		}
    }

}
