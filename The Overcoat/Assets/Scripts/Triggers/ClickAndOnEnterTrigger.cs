using UnityEngine;
using System.Collections;

//Dependent to: CursorImageScript, CollidingObjects
//This script calls Action function from IClickTrigger when trigger conditions meeted.
//Trigger condition is clicking owner object collider when player collider inside of child object's collider.
//Child object must have CollidingObjects.cs!
//If mouse texture is not null it will change CursorImageScript's default texture.

public class ClickAndOnEnterTrigger : MonoBehaviour {

    //Cursor Texture
    public Texture2D cursorTexture;

    //Child object's script
    CollidingObjects co;
    IClickAction ic;



    CursorImageScript mt;
    void Awake()
    {
      co=  GetComponentInChildren<CollidingObjects>();
      ic = GetComponentInChildren<IClickAction>();

    }
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnMouseOver()
    {




    }

    void OnMouseExit()
    {

    }


        
    void OnMouseDown()
    {
        if (co != null&&ic!=null)
        {
            foreach(GameObject go in co.colObjs)
            {
                if (go.tag == "Player")
                {
                    ic.Action();
                    //this.enabled = false;
                    Destroy(this.gameObject);

                }

            }

        }


    }
}
