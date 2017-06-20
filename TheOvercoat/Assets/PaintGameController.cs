using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PaintGameController : MonoBehaviour {

    Texture2D texture;
    RectTransform rect;

    public int resWidth = 1920;
    public int resHeight = 1080;

    bool pointerOnCanvas = false;

    bool applyAtFixedUpdate = false;

    int originX,originY;

    float widthRatio, heightRatio;

    public GameObject cam;

	// Use this for initialization
	void Awake () {

        rect = GetComponent<RectTransform>();

  

        originX = (int)rect.position.x;
        originY = (int)rect.position.y;
  

        texture = new Texture2D(resWidth, resHeight);
        //paintCanvas.GetComponent<Renderer>().material.mainTexture = texture;
        GetComponent<RawImage>().texture = texture;



        clearCanvas();

        texture.SetPixel( 0,0, Color.red);
        texture.Apply();
        //for (int y = 0; y < texture.height; y++)
        //{
        //    for (int x = 0; x < texture.width; x++)
        //    {
        //        Color color = ((x & y) != 0 ? Color.white : Color.gray);
        //        texture.SetPixel(x, y, color);
        //    }
        //}


    }
	
    public void OnPointerExit()
    {
        pointerOnCanvas = false;
    }

    public void OnPointerEnter()
    {
        pointerOnCanvas = true;
    }

    private void Update()
    {
        Debug.Log("Origint is "+ originX+" "+originY+" Mouse position is " + Input.mousePosition.x + " " + Input.mousePosition.y);

        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit;
            //Ray ray = new Ray(cam.transform.position, Input.mousePosition);

            hit = Physics2D.Raycast(cam.transform.position,Input.mousePosition);

            Debug.Log("Drawing");
            //float x = hit.textu;
            float y = Input.mousePosition.y-originY;

      
            //texture.SetPixel((int)(x*widthRatio), (int)(y*heightRatio), Color.black);

            applyAtFixedUpdate = true;


        }
    }

    private void FixedUpdate()
    {
        if (applyAtFixedUpdate)
        {
            Debug.Log("Applying");
            texture.Apply();
            applyAtFixedUpdate = false;
        }
    }

    void clearCanvas()
    {
        Color32 resetColor = new Color32(255, 255, 255, 255);
        Color32[] resetColorArray = texture.GetPixels32();

        for (int i = 0; i < resetColorArray.Length; i++)
        {
            resetColorArray[i] = resetColor;
        }

        texture.SetPixels32(resetColorArray);
        texture.Apply();
    }

}
