/// <summary>
/// CodeArtist.mx 2015
/// This is the main class of the project, its in charge of raycasting to a model and place brush prefabs infront of the canvas camera.
/// If you are interested in saving the painted texture you can use the method at the end and should save it to a file.
/// </summary>


using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public enum Painter_BrushMode { PAINT, DECAL, BINARY };
public class TexturePainter : MonoBehaviour {
	public GameObject brushCursor,brushContainer; //The cursor that overlaps the model and our container for the brushes painted
	public Camera sceneCamera,canvasCam;  //The camera that looks at the model, and the camera that looks at the canvas.
	public GameObject paintBrush,starBrush,binaryBrush; // Cursor for the differen functions 


	public RenderTexture canvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
	public Material baseMaterial; // The material of our base texture (Were we will save the painted texture)

   
    Painter_BrushMode mode; //Our painter mode (Paint brushes or decals)

	float brushSize=1.0f; //The size of our brush
	Color brushColor; //The selected color
	int brushCounter=0,MAX_BRUSH_COUNT=1000; //To avoid having millions of brushes
	bool saving=false; //Flag to check if we are saving the texture

    CursorImageScript cis;

    private void OnEnable()
    {
        cis = CharGameController.getOwner().GetComponent<CursorImageScript>();
        cis.resetExternalCursor();
        cis.enabled = false;
    }

    private void OnDisable()
    {
        cis.enabled = true;
    }

    private void Start()
    {
        sceneCamera = CharGameController.getCamera().GetComponent<Camera>();
    }

    void Update () {
		brushColor = ColorSelector.GetColor ();	//Updates our painted color with the selected color
		if (Input.GetMouseButton(0)) {
			DoAction();
		}
		UpdateBrushCursor ();
	}

	//The main action, instantiates a brush or decal entity at the clicked position on the UV map
	void DoAction(){	
		if (saving)
			return;
		Vector3 uvWorldPosition=Vector3.zero;		
		if(HitTestUVPosition(ref uvWorldPosition)){
			GameObject brushObj=null;

            switch (mode)
            {
                case Painter_BrushMode.PAINT:
                    brushObj = (GameObject)Instantiate(paintBrush); //Paint a brush
                    brushObj.GetComponent<SpriteRenderer>().color = brushColor; //Set the brush color
                    break;
                case Painter_BrushMode.BINARY:
                    brushObj = (GameObject)Instantiate(binaryBrush);
                    brushObj.GetComponent<SpriteRenderer>().color = brushColor;
                    break;
                case Painter_BrushMode.DECAL:
                    brushObj = (GameObject)Instantiate(starBrush);
                    brushObj.GetComponent<SpriteRenderer>().color = brushColor;
                    break;

            }

   //         if (mode==Painter_BrushMode.PAINT){

			//	brushObj=(GameObject)Instantiate(paintBrush); //Paint a brush
			//	brushObj.GetComponent<SpriteRenderer>().color=brushColor; //Set the brush color
			//}
			//else{
			//	brushObj=(GameObject)Instantiate(starBrush); //Paint a decal
			//}



			brushColor.a=brushSize*2.0f; // Brushes have alpha to have a merging effect when painted over.
			brushObj.transform.parent=brushContainer.transform; //Add the brush to our container to be wiped later
			brushObj.transform.localPosition=uvWorldPosition; //The position of the brush (in the UVMap)
			brushObj.transform.localScale=Vector3.one*brushSize;//The size of the brush
		}

		brushCounter++; //Add to the max brushes
		if (brushCounter >= MAX_BRUSH_COUNT) { //If we reach the max brushes available, flatten the texture and clear the brushes
			brushCursor.SetActive (false);
			saving=true;
			Invoke("SaveTexture",0.1f);
			
		}
	}
	//To update at realtime the painting cursor on the mesh
	void UpdateBrushCursor(){
		Vector3 uvWorldPosition=Vector3.zero;
		if (HitTestUVPosition (ref uvWorldPosition) && !saving) {
			brushCursor.SetActive(true);
			brushCursor.transform.position =uvWorldPosition+brushContainer.transform.position;									
		} else {
			brushCursor.SetActive(false);
		}		
	}
	//Returns the position on the texuremap according to a hit in the mesh collider
	bool HitTestUVPosition(ref Vector3 uvWorldPosition){
		RaycastHit hit;
		Vector3 cursorPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0.0f);
		Ray cursorRay=sceneCamera.ScreenPointToRay (cursorPos);
		if (Physics.Raycast(cursorRay,out hit,200)){
			MeshCollider meshCollider = hit.collider as MeshCollider;
			if (meshCollider == null || meshCollider.sharedMesh == null)
				return false;			
			Vector2 pixelUV  = new Vector2(hit.textureCoord.x,hit.textureCoord.y);
			uvWorldPosition.x=pixelUV.x-canvasCam.orthographicSize;//To center the UV on X
			uvWorldPosition.y=pixelUV.y-canvasCam.orthographicSize;//To center the UV on Y
			uvWorldPosition.z=0.0f;
			return true;
		}
		else{		
			return false;
		}
		
	}
	//Sets the base material with a our canvas texture, then removes all our brushes
	void SaveTexture(){		
		brushCounter=0;
		System.DateTime date = System.DateTime.Now;
		RenderTexture.active = canvasTexture;
		Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);		
		tex.ReadPixels (new Rect (0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
		tex.Apply ();
		RenderTexture.active = null;
		baseMaterial.mainTexture =tex;	//Put the painted texture as the base
		foreach (Transform child in brushContainer.transform) {//Clear brushes
			Destroy(child.gameObject);
		}
		//StartCoroutine ("SaveTextureToFile"); //Do you want to save the texture? This is your method!
		Invoke ("ShowCursor", 0.1f);
	}
	//Show again the user cursor (To avoid saving it to the texture)
	void ShowCursor(){	
		saving = false;
	}

	////////////////// PUBLIC METHODS //////////////////

	public void SetBrushMode(Painter_BrushMode brushMode){ //Sets if we are painting or placing decals
		mode = brushMode;

        Sprite brushSprite = null;

        switch (mode)
        {
            case Painter_BrushMode.PAINT:
                brushSprite = paintBrush.GetComponent<SpriteRenderer>().sprite;
                break;
            case Painter_BrushMode.BINARY:
                brushSprite = binaryBrush.GetComponent<SpriteRenderer>().sprite;
                break;
            case Painter_BrushMode.DECAL:
                brushSprite = starBrush.GetComponent<SpriteRenderer>().sprite;
                break;

        }

        brushCursor.GetComponent<SpriteRenderer> ().sprite =brushSprite;
	}
	public void SetBrushSize(float newBrushSize){ //Sets the size of the cursor brush or decal
		brushSize = newBrushSize;
		brushCursor.transform.localScale = Vector3.one * brushSize;
	}


    public void SaveTextureToFileFunc()
    {
        Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
        tex.Apply();
        StartCoroutine(SaveTextureToFile(tex));


        //Debug.Log("Started to save");

        //string directory = Application.persistentDataPath + "/Paintings";
        //if (!Directory.Exists(directory))
        //    Directory.CreateDirectory(directory);

        //string fileName = "picture_";
        //int indexOfNumber = fileName.Length;

        //string extension = ".jpg";

        ////TODO name for paintings

        //var info = new DirectoryInfo(directory);
        //var fileInfo = info.GetFiles();

        //int biggestIndex = 0;

        //foreach (var file in fileInfo)
        //{
        //    if (file.Name.Substring(0, fileName.Length) == fileName)
        //    {
        //        Debug.Log("Looking for files");

        //        int length = file.Name.IndexOf(extension[0]) - indexOfNumber;

        //        string number = file.Name.Substring(indexOfNumber, length);
        //        Debug.Log(number);
        //        int i = int.Parse(number);
        //        if (i > biggestIndex) biggestIndex = i;
        //    }
        //}


        //string fullName = (Application.persistentDataPath + "/Paintings/" + fileName + (biggestIndex + 1) + extension);

        //brushCounter = 0;
        ////string fullPath=System.IO.Directory.GetCurrentDirectory()+"\\UserCanvas\\";
        ////System.DateTime date = System.DateTime.Now;
        ////string fileName = "CanvasTexture.png";
        ////if (!System.IO.Directory.Exists(fullPath))		
        ////	System.IO.Directory.CreateDirectory(fullPath);
        //var bytes = tex.EncodeToJPG();
        //File.WriteAllBytes(fullName, bytes);
        //Debug.Log("<color=orange>Saved Successfully!</color>" + fullName);



    }

	////////////////// OPTIONAL METHODS //////////////////


		IEnumerator SaveTextureToFile(Texture2D savedTexture){

            Debug.Log("Started to save");

        //    string directory = Application.persistentDataPath + "/Paintings";
        //    if (!Directory.Exists(directory))
        //        Directory.CreateDirectory(directory);

        //    string fileName = "picture_";
        //    int indexOfNumber = fileName.Length;

        //    string extension = ".jpg";

        //    //TODO name for paintings

        //    var info = new DirectoryInfo(directory);
        //    var fileInfo = info.GetFiles();

        //    int biggestIndex = 0;

        //    foreach (var file in fileInfo)
        //    {
        //        if (file.Name.Substring(0, fileName.Length) == fileName)
        //        {
        //        Debug.Log("Looking for files");

        //            int length = file.Name.IndexOf(extension[0]) - indexOfNumber;

        //            string number = file.Name.Substring(indexOfNumber, length);
        //            Debug.Log(number);
        //            int i = int.Parse(number);
        //            if (i > biggestIndex) biggestIndex = i;
        //        }
        //    }


        //string fullName = (Application.persistentDataPath + "/Paintings/" + fileName + (biggestIndex + 1) + extension);

        brushCounter = 0;
        string fullPath = System.IO.Directory.GetCurrentDirectory() + "\\UserCanvas\\";
        System.DateTime date = System.DateTime.Now;
        string fileName = "CanvasTexture.png";

        string fullName = fullPath + fileName;

        if (!System.IO.Directory.Exists(fullPath))
            System.IO.Directory.CreateDirectory(fullPath);
        var bytes = savedTexture.EncodeToJPG();
			File.WriteAllBytes(fullName, bytes);
			Debug.Log ("<color=orange>Saved Successfully!</color>"+fullName);
			yield return null;
		}


    public void saveTextureAsJPG()
    {
        SaveTexture();

        //Debug.Log("Extracting texture");

        //Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);
        //tex.ReadPixels(new Rect(0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
        //tex.Apply();

        //Debug.Log("Extracting finished");

        Texture2D tex = baseMaterial.mainTexture as Texture2D;

        string directory = Application.persistentDataPath + "/Paintings";
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        string fileName = "picture_";
        int indexOfNumber = fileName.Length;

        string extension = ".jpg";

        //TODO name for paintings

        var info = new DirectoryInfo(directory);
        var fileInfo = info.GetFiles();

        int biggestIndex = 0;

        foreach (var file in fileInfo)
        {
            if (file.Name.Substring(0, fileName.Length) == fileName)
            {

                int length = file.Name.IndexOf(extension[0]) - indexOfNumber;

                string number = file.Name.Substring(indexOfNumber, length);
                Debug.Log(number);
                int i = int.Parse(number);
                if (i > biggestIndex) biggestIndex = i;
            }
        }




        File.WriteAllBytes(Application.persistentDataPath + "/Paintings/" + fileName + (biggestIndex + 1) + extension, tex.EncodeToJPG());

    }


}
