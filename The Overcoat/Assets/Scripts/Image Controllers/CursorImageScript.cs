using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CursorImageScript : MonoBehaviour {

    //public float xOffset, yOffset = 10;
    public Texture2D defaultTexture;

    RaycastHit lastHit;

	// Use this for initialization
	void Awake () {
        
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
           

 
           if(hit.transform.tag=="ActiveObject")
            {
                Cursor.SetCursor(hit.transform.GetComponent<MouseTexture>().texture, Vector2.zero, CursorMode.Auto);

            }
            else
            {
                                
                Cursor.SetCursor(defaultTexture, Vector2.zero, CursorMode.Auto);

            }


        }


    }
}
