using UnityEngine;
using System.Collections;

//Deals with mumbling sounds
public class ConversationAudio : MonoBehaviour {

    public AudioClip audioClip;

    //[HideInInspector]
    static AudioSource source;

    bool isPlaying;

	// Use this for initialization
	void Start () {
	    if(CharGameController.cgc==null || audioClip==null)
        {
            enabled = false;
            return;
        }

        ///Second audio source is for conversation.
        if(!source)  source=CharGameController.getOwner().GetComponents<AudioSource>()[1];

        if (!source)
        {
            enabled = false;
            return;
        }


	}
	
    public void activateAudioConv()
    {
        if (audioClip==null || (source.clip == audioClip && source.isPlaying)) return;

        source.clip = audioClip;
        source.time = Random.Range(0, source.clip.length);
        source.Play();
        isPlaying = true;
    }


    public static void deactivateAudioConv()
    {
        //Debug.Log("Deactivating");
        if (!source || source.clip==null) return;
        
                
        source.Stop();
        source.clip = null;
        
    }
}
