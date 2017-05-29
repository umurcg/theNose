using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


//Handles music of the scenes
public class LevelMusicController : MonoBehaviour {

    //Muisc audio clips according to their scene index
    public AudioClip[] levelMusics;
    AudioSource musicSource;

    

	// Use this for initialization
	void Start () {

        if (musicSource == null) musicSource = CharGameController.getOwner().GetComponent<AudioSource>();
              

        AudioSource source = CharGameController.getOwner().GetComponent<AudioSource>();
        updateMusic();

	}


    void OnEnable()
    {
        //Tell our 'registerToSceneList' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += updateMusic;
    }

    void OnDisable()
    {
        //Tell our 'registerToSceneList' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= updateMusic;
    }

    void updateMusic(Scene scene, LoadSceneMode mode)
    {
        updateMusic();
    }

    void updateMusic()
    {
        if (musicSource == null) musicSource=CharGameController.getOwner().GetComponent<AudioSource>();
        //if (musicSource == null) Debug.Log("Music source is null");

            int sceneIndex=SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex >= levelMusics.Length) return;

        musicSource.clip = levelMusics[sceneIndex];
        musicSource.Play();

    }

    public void setMusicManually(AudioClip clip)
    {
        if (musicSource == null) musicSource = CharGameController.getOwner().GetComponent<AudioSource>();
        musicSource.clip = clip;
        musicSource.Play();
    }


}
