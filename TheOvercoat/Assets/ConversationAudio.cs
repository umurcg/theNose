using UnityEngine;
using System.Collections;

//Deals with mumbling sounds
public class ConversationAudio : MonoBehaviour {

    public static ConversationAudio activeScript;

    public AudioClip audioClip;

    //[HideInInspector]
    static AudioSource source;

    //bool isPlaying;

	// Use this for initialization
	void Start () {
	    if(CharGameController.cgc==null || audioClip==null)
        {
            enabled = false;
            return;
        }

        ///Second audio source is for conversation.
        if (!source) source = GlobalController.Instance.afxSource;

        if (!source)
        {
            enabled = false;
            return;
        }


	}
	
    public void activateAudioConv()
    {
        if (activeScript==this)
        {
            //Debug.Log("is playing ");
            return;
        }

        Debug.Log("Activating audio conv");
        if (audioClip==null || (source.clip == audioClip && source.isPlaying)) return;

        source.clip = audioClip;
        source.time = Random.Range(0, source.clip.length);
        source.Play();
        
        activeScript = this;
    }


    public void deactivateAudioConv()
    {
        Debug.Log("deactiating audio conv");

        activeScript = null;


        //Debug.Log("Deactivating");
        if (!source || source.clip==null) return;
        
                
        source.Stop();
        source.clip = null;
        
    }
}
