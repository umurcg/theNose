// Write black pixels onto the GameObject that is located
// by the script. The script is attached to the camera.
// Determine where the collider hits and modify the texture at that point.
//
// Note that the MeshCollider on the GameObject must have Convex turned off. This allows
// concave GameObjects to be included in collision in this example.
//
// Also to allow the texture to be updated by mouse button clicks it must have the Read/Write
// Enabled option set to true in its Advanced import settings.

using UnityEngine;
using System.Collections;
using System.IO;

public class PaintGameController3D : MonoBehaviour
{
    public Camera cam;
    bool applyAtFixedUpdate = false;
    Texture2D tex;
    Texture2D paintedText;
    Renderer rend;
    MeshCollider meshCollider;

    public Texture2D star;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        meshCollider = GetComponent<MeshCollider>();
        tex = rend.material.mainTexture as Texture2D;

        //Create new texture;
        paintedText = new Texture2D(tex.width, tex.height);
        Color[] colors = tex.GetPixels(0, 0, tex.width, tex.height);
        paintedText.SetPixels(colors);

        rend.material.mainTexture = paintedText;
        paintedText.Apply();
    }

    private void OnDestroy()
    {
        //Recover original texture
        rend.material.mainTexture = tex;
        saveTextureAsJPG();
    }

    public void saveTextureAsJPG()
    {
        string directory = Application.persistentDataPath + "/Paintings";
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        string fileName = "picture_";
        int indexOfNumber = fileName.Length;

        string extension = ".jpg";

        //TODO name for paintings

        var info = new DirectoryInfo(directory);
        var fileInfo = info.GetFiles();

        int biggestIndex=0;

        foreach (var file in fileInfo)
        {
            if (file.Name.Substring(0, fileName.Length) == fileName)
            {
           
               int length=file.Name.IndexOf(extension[0]) - indexOfNumber;
       
               string number = file.Name.Substring(indexOfNumber,length);
               Debug.Log(number);
               int i= int.Parse(number);
               if (i>biggestIndex) biggestIndex=i;
            }
        }

        


        File.WriteAllBytes(Application.persistentDataPath+"/Paintings/"+fileName+(biggestIndex+1)+extension, paintedText.EncodeToJPG());
        
    }

    void OnMouseOver()
    {
        if (!Input.GetMouseButton(0))
            return;

        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;
                
        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
            return;

       
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;
        
                
        paintedText.SetPixels32((int)pixelUV.x, (int)pixelUV.y, star.width, star.height, star.GetPixels32());

        //int indexOfSelectedPixel=pixelUV.x

        float brushWidth = star.width;
        float brushHeight = star.height;

        

        float startIndex = pixelUV.x - (brushWidth / 2) - (brushHeight/2)*star.width;
        //float finishIndex = p


        //paintedText.SetPixels32()
        

        //paintedText.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
        applyAtFixedUpdate = true;
    }

    private void FixedUpdate()
    {
        if (applyAtFixedUpdate)
        {
            Debug.Log("Applying");
            paintedText.Apply();
            applyAtFixedUpdate = false;
        }
    }

}