using UnityEngine;
using System.Collections;

//Adds music to camera when scene is load
public class LevelMusicController : MonoBehaviour {

    public AudioClip levelMusicClip;
    public bool preserveLastSceneMusic = false;

	// Use this for initialization
	void Start () {


        if (levelMusicClip == null)
        {
            if (preserveLastSceneMusic) return;

            AudioSource currentSource=CharGameController.getOwner().GetComponent<AudioSource>();

            if (currentSource != null)
            {
                Destroy(currentSource);
            }

            return;
        }

        //Check if audioSource exists
        //If exists add current source to camera else cre<ate a new source and add to camera
        AudioSource source = CharGameController.getOwner().GetComponent<AudioSource>();

        if (source != null)
        {
            source.clip = levelMusicClip;
            source.loop = true;
            source.playOnAwake = true;
            source.Play();
        }
        //else
        //{
        //    //Add audio source to camera
        //    source = camObj.AddComponent<AudioSource>();
        //    source.clip = levelMusicClip;
        //    source.loop = true;
        //    source.playOnAwake = true;
        //    source.Play();
        //}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
