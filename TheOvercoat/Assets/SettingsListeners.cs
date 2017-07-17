using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//This script adds listeners to settings ui elements
public class SettingsListeners : MonoBehaviour {

    public GameObject AudioSliderGO;
    public GameObject MusicSliderGO;
    public GameObject LangDropdownGO;
    public GameObject CamDropdownGO;
    //public GameObject InputDropDownGO;

    Slider audioSlider, musicSlider;
    Dropdown dropDown, camdropDown/*, inputDropDown*/;

    public MuteButton muteButton;

    //float previousMusicLevelBeforeMute;

	// Use this for initialization
	void Start () {

        audioSlider=AudioSliderGO.GetComponent<Slider>();
        musicSlider = MusicSliderGO.GetComponent<Slider>();
        dropDown = LangDropdownGO.GetComponent<Dropdown>();
        camdropDown = CamDropdownGO.GetComponent<Dropdown>();
        //if(InputDropDownGO!=null) inputDropDown = InputDropDownGO.GetComponent<Dropdown>();

        audioSlider.value = GlobalController.Instance.getAudioLevel();
        musicSlider.value = GlobalController.Instance.getMusicLevel();
        dropDown.value=(int)(GlobalController.Instance.getLangueSetting());
        //if (inputDropDown) inputDropDown.value = (int)GlobalController.Instance.getInputType();



        if (CharGameController.cgc != null)
        {
            camdropDown.value = (int)(CharGameController.getCameraType());
        }

        audioSlider.onValueChanged.AddListener(delegate { GlobalController.Instance.setAudioLevel(audioSlider.value); });
        musicSlider.onValueChanged.AddListener(delegate { GlobalController.Instance.setMusicLevel(musicSlider.value); });
        //musicSlider.onValueChanged.AddListener(delegate { muteButton.updateButton(); });
        dropDown.onValueChanged.AddListener(delegate { GlobalController.Instance.setLanguageSetting(dropDown.value); });

        //if (inputDropDown)  inputDropDown.onValueChanged.AddListener(delegate { GlobalController.Instance.setInputType(inputDropDown.value); });

        //Debug.Log("hİİİİİİİİİİİİİİİİİİ");

        if (CharGameController.cgc != null)
        {
            //Debug.Log("Adding listener");
            camdropDown.onValueChanged.AddListener(delegate {CharGameController.setCameraType((CharGameController.cameraType)camdropDown.value); });

        }else
        {
            Debug.Log("No cgc");
        }


    }

    


    public void muteMusic()
    {
        //previousMusicLevelBeforeMute = GlobalController.Instance.getMusicLevel();
        //musicSlider.value = 0;
        //GlobalController.Instance.setMusicLevel(musicSlider.value);
        GlobalController.Instance.musicSouce.mute = true;
    }
    
    public void unMuteMusic()
    {
        //if (previousMusicLevelBeforeMute == 0) previousMusicLevelBeforeMute = 1;
        //musicSlider.value = previousMusicLevelBeforeMute;
        //GlobalController.Instance.setMusicLevel(musicSlider.value);
        GlobalController.Instance.musicSouce.mute = false;
    }

}
