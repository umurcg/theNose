using UnityEngine;
using System.Collections;

public class RecordVideo : MonoBehaviour {

    public int resWidth = 1920;
    public int resHeight = 1080;

    public string folder;
    public int frameRate = 25;

    public GameObject cameraObj;
    Camera cam;

    private void Awake()
    {
        cam =cameraObj.GetComponent<Camera>();
    }

    public string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/{1:D04} shot.png", folder, Time.frameCount);
        //return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
        //                     Application.dataPath,
        //                     width, height,
        //                     System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }



    //// The folder to contain our screenshots.
    //// If the folder exists we will append numbers to create an empty folder.
    //public string folder = "EnterScene";

    void Start()
    {
        // Set the playback framerate (real time will not relate to game time after this).
        Time.captureFramerate = frameRate;

        // Create the folder
        System.IO.Directory.CreateDirectory(folder);
    }


    void Update()
    {

        takeScreenShot();
        //// Append filename to folder name (format is '0005 shot.png"')
        //string name = string.Format("{0}/{1:D04} shot.png", folder, Time.frameCount);

        //// Capture the screenshot to the specified file.
        //Application.CaptureScreenshot(name);
    }

    void takeScreenShot()
    {
  
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            cam.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            cam.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            cam.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));

        
    }



}
