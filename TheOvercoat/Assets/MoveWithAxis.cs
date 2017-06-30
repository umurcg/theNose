using UnityEngine;
using System.Collections;

//Moves owner obect with axis input in 2d

public class MoveWithAxis : MonoBehaviour {


    public enum axes {x,y,z };
    public axes Axis1=axes.x;
    public axes Axis2 = axes.y;

    public bool invertAxis1=false;
    public bool invertAxis2 = false;

    public string AxisInput1;
    public string AxisInput2;

    public enum space {World, Local, Object };
    public space mySpace = space.World;

    public GameObject spaceObject;


    public float speed=3f;

	// Use this for initialization
	void Start () {
	
        if(mySpace==space.Object && spaceObject == null)
        {
            Debug.Log("yOU DİDNTT ASSİGN SAPCE OBJECT");
            enabled = false;
            return;
        } 
	}
	
	// Update is called once per frame
	void Update () {

        float axis1Value = Input.GetAxis(AxisInput1);
        float axis2Value = Input.GetAxis(AxisInput2);

        Vector3 axis1Dir=Vector3.zero;
        Vector3 axis2Dir=Vector3.zero;



        if (mySpace == space.World)
        {
            switch (Axis1)
            {
                case axes.x:
                    axis1Dir = Vector3.right;
                    break;
                case axes.y:
                    axis1Dir = Vector3.up;
                    break;
                case axes.z:
                    axis1Dir = Vector3.forward;
                    break;

            }
            switch (Axis2)
            {
                case axes.x:
                    axis2Dir = Vector3.right;
                    break;
                case axes.y:
                    axis2Dir = Vector3.up;
                    break;
                case axes.z:
                    axis2Dir = Vector3.forward;
                    break;

            }        
            
        }else if(mySpace==space.Local)
        {

            switch (Axis1)
            {
                case axes.x:
                    axis1Dir = transform.right;
                    break;
                case axes.y:
                    axis1Dir = transform.up;
                    break;
                case axes.z:
                    axis1Dir = transform.forward;
                    break;

            }
            switch (Axis2)
            {
                case axes.x:
                    axis2Dir = transform.right;
                    break;
                case axes.y:
                    axis2Dir = transform.up;
                    break;
                case axes.z:
                    axis2Dir = transform.forward;
                    break;

            }

        }else if (mySpace == space.Object)
        {
            //Debug.Log("OBJECT SPACE");
            switch (Axis1)
            {
                case axes.x:
                    axis1Dir = spaceObject.transform.right;
                    break;
                case axes.y:
                    axis1Dir = spaceObject.transform.up;
                    break;
                case axes.z:
                    axis1Dir = spaceObject.transform.forward;
                    break;

            }
            switch (Axis2)
            {
                case axes.x:
                    axis2Dir = spaceObject.transform.right;
                    break;
                case axes.y:
                    axis2Dir = spaceObject.transform.up;
                    break;
                case axes.z:
                    axis2Dir = spaceObject.transform.forward;
                    break;

            }
        }

        if (invertAxis1) axis1Dir = axis1Dir * -1;
        if (invertAxis2) axis2Dir = axis2Dir * -1;


        Debug.DrawLine(transform.position, transform.position + axis1Dir);
        Debug.DrawLine(transform.position, transform.position + axis2Dir);

        transform.Translate(axis1Dir * speed * axis1Value,Space.World);
        transform.Translate(axis2Dir * speed * axis2Value,Space.World);


    }

    

}
