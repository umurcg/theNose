using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour {

    public Sprite muteImage, speakerImage;
    public SettingsListeners settingListener;
    Image buttonImage;

    bool muted = false;

	// Use this for initialization
	void Awake () {
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = speakerImage;

        

	}

    private void Start()
    {
        muted = GlobalController.Instance.musicSouce.mute==true;
        updateButton();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void mute()
    {
        settingListener.muteMusic();
        buttonImage.sprite = muteImage;
        muted = true;
    }

    void unMute()
    {
        settingListener.unMuteMusic();
        buttonImage.sprite = speakerImage;
        muted = false;
    }

    public void toggle()
    {
        if (muted)
        {
            unMute();
        }
        else
        {
            mute();
        }
    }

    public void updateButton()
    {
        if (GlobalController.Instance.musicSouce.mute)
        {
            muted = true;
            buttonImage.sprite = muteImage;
        }
        else
        {
            muted = false;
            buttonImage.sprite = speakerImage;
        }
    }

}
