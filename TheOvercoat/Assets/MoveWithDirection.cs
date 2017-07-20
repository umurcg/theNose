using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithDirection : MonoBehaviour {

    public enum direction { up,right,forward};
    public direction Direction;
    public float moveSpeed=1;

    public Space space = Space.World;
    public bool negDir=false;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 dirVector = Vector3.zero;

        switch (Direction)
        {
            case (direction.up):
                dirVector = ((space == Space.World) ? Vector3.up : transform.up);
                break;
            case (direction.right):
                dirVector = ((space == Space.World) ? Vector3.right : transform.right);
                break;
            case (direction.forward):
                dirVector = ((space == Space.World) ? Vector3.forward : transform.forward);
                break;


        }

        if (negDir) dirVector = -1 * dirVector;

        transform.Translate(dirVector * moveSpeed,Space.World);
    }
}
