using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using MovementEffects;


//Handles music assignments of the scenes
//[RequireComponent(typeof(AudioSource))]
public class LevelMusicController : MonoBehaviour {

    [System.Serializable]
    public struct sceneAndMusic
    {
        
        public GlobalController.Scenes scene;
        public AudioClip dayMusic;
        public AudioClip nightMusic;
    }


    //Muisc audio clips according to their scene index
    //public AudioClip[] levelMusics;

    public sceneAndMusic[] SceneAndMusic;

    AudioSource musicSource;
        

	// Use this for initialization
	void Start () {
                
        updateMusic();

	}


    sceneAndMusic getSceneAndMusicElementByScene(GlobalController.Scenes scene)
    {
        foreach(sceneAndMusic sam in SceneAndMusic)
        {
            if (sam.scene == scene) return sam;
        }

        sceneAndMusic nullSceneAndMusic = new sceneAndMusic();
        nullSceneAndMusic.scene = GlobalController.Scenes.None;
        return nullSceneAndMusic;
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
        if (musicSource == null)
        {
            Debug.Log("Music source is null");
            return;
        }

        GlobalController.Scenes currentScene =(GlobalController.Scenes) SceneManager.GetActiveScene().buildIndex;

        sceneAndMusic currentStruct = getSceneAndMusicElementByScene(currentScene);

        if ((currentStruct.scene == GlobalController.Scenes.None) || (currentStruct.dayMusic == null && currentStruct.nightMusic == null)) return;     

        if (currentStruct.dayMusic==null && currentStruct.nightMusic != null)
        {
            musicSource.clip = currentStruct.nightMusic;
            
        }else if (currentStruct.dayMusic != null && currentStruct.nightMusic == null)
        {
            musicSource.clip = currentStruct.dayMusic;
            
        }else if (currentStruct.dayMusic != null && currentStruct.nightMusic != null)
        {
            if (CharGameController.getSun() != null && CharGameController.getSun().GetComponent<DayAndNightCycle>().isNight)
            {
                musicSource.clip = currentStruct.nightMusic;
            }
            else
            {
                musicSource.clip = currentStruct.dayMusic;
            }
        }

        //int sceneIndex=SceneManager.GetActiveScene().buildIndex;

        //if (sceneIndex >= levelMusics.Length) return;

        //musicSource.clip = levelMusics[sceneIndex];

        
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

    //Play a sound affect
    public static void playSoundEffect(AudioClip clip)
    {
        Timing.RunCoroutine(_playSoundEffect(clip));
    }
    public static void playSoundEffect(AudioClip clip, float duration)
    {
        Timing.RunCoroutine(_playSoundEffect(clip,true,duration));
    }

    public static void playSoundEffect(AudioClip clip, float start, float end)
    {
        Timing.RunCoroutine(_playSoundEffect(clip, true, end-start,start));
    }



    static IEnumerator<float> _playSoundEffect(AudioClip clip, bool volueDownMusicWhilePlaying=true, float duration=0,float start=0){

        AudioSource source = GlobalController.Instance.afxSource;
        source.clip = clip;

        if (source.clip.length > start) source.time = start;

        source.loop = false;

        AudioSource musicSouce = GlobalController.Instance.musicSouce;

        float dimmedVolume = 0.3f;

        if (musicSouce == null || musicSouce.volume<dimmedVolume) volueDownMusicWhilePlaying = false;
        

        IEnumerator<float> musicDimmer=null;
        float originalVolume=0;

        

        if (volueDownMusicWhilePlaying) {
            originalVolume = musicSouce.volume;
            musicDimmer = Timing.RunCoroutine(Vckrs.smoothVolumeChange(musicSouce, dimmedVolume, 1f));
         }
        
        source.Play();

        if (duration == 0 || (start+ duration)> source.clip.length)
        {
            while (source.isPlaying) yield return 0;
        }
        else
        {
            while (duration > 0)
            {
                duration -= Time.deltaTime;
                yield return 0;
            }
        }
        source.Stop();
        source.clip = null;
        source.loop = false;

        if (volueDownMusicWhilePlaying)
        {

            if (musicDimmer != null)
                yield return Timing.WaitUntilDone(musicDimmer);

            Timing.RunCoroutine(Vckrs.smoothVolumeChange(musicSouce, originalVolume, 1f));

        }

        yield break;
    }

    public static void clearSoundEffectFromSource()
    {
        if (GlobalController.Instance.afxSource != null) GlobalController.Instance.afxSource.clip = null;
    }



}
