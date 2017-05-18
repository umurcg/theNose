using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//This scripts play video from captured screen shots
public class VideoPlayer : MonoBehaviour {

    //public Sprite[] images;
    //public float fps =  60;
    //float deltaSeconds;

    //float timer = 0;

    //Image image;
    //int index = 0;

    RawImage image;
    public MovieTexture mt;

    [HideInInspector]
    public bool isPlaying = false;

	// Use this for initialization
	void Start () {
        //deltaSeconds = 1 / fps;
        //image = GetComponent<Image>();
        //image.sprite = images[index];

	}
	
	// Update is called once per frame
	public void play () {

        if(!image) image = GetComponent<RawImage>();

        if (isPlaying || !mt)
        {
            return;
        }else
        {
            isPlaying = true;
        }

        image.enabled = true;
        image.texture = mt as MovieTexture;
        mt.Play();
        enabled = true;
        

	}

    private void Update()
    {
        if (!isPlaying) enabled = false;

        if (!mt.isPlaying)
        {
            isPlaying = false;
            enabled = false;
            image.enabled = false;
        }
    }

    public void stop()
    {
        mt.Stop();
        isPlaying = false;
        enabled = false;
        image.enabled = false;
    }

    public void pause()
    {
        mt.Pause();
    }


}
