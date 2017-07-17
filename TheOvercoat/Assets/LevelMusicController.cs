using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


//Handles music assignments of the scenes
//[RequireComponent(typeof(AudioSource))]
public class LevelMusicController : MonoBehaviour {

    //Muisc audio clips according to their scene index
    public AudioClip[] levelMusics;
    AudioSource musicSource;
        

	// Use this for initialization
	void Start () {
                
        updateMusic();

	}


    void OnEnable()
    {
        //Tell our 'registerToSceneList' function to start listening for a scene change as soon as this script is enabled.
        assignMusicSource();

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
        assignMusicSource();
        //if (musicSource == null) Debug.Log("Music source is null");

        int sceneIndex=SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex >= levelMusics.Length) return;

        musicSource.clip = levelMusics[sceneIndex];
        musicSource.Play();

    }

    void assignMusicSource()
    {
        if (musicSource == null && GlobalController.Instance != null) musicSource = GlobalController.Instance.musicSouce;
    }

    public void setMusicManually(AudioClip clip)
    {
        assignMusicSource();
        musicSource.clip = clip;
        musicSource.Play();
    }


}
