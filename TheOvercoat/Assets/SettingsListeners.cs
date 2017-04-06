using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//This script adds listeners to settings ui elements
public class SettingsListeners : MonoBehaviour {

    public GameObject AudioSliderGO;
    public GameObject MusicSliderGO;
    public GameObject LangDropdownGO;

    Slider audioSlider, musicSlider;
    Dropdown dropDown;


	// Use this for initialization
	void Start () {

        audioSlider=AudioSliderGO.GetComponent<Slider>();
        musicSlider = MusicSliderGO.GetComponent<Slider>();
        dropDown = LangDropdownGO.GetComponent<Dropdown>();

        audioSlider.value = GlobalController.Instance.getAudioLevel();
        musicSlider.value = GlobalController.Instance.getMusicLevel();
        dropDown.value=(int)(GlobalController.Instance.getLangueSetting());

        audioSlider.onValueChanged.AddListener(delegate { GlobalController.Instance.setAudioLevel(audioSlider.value); });
        musicSlider.onValueChanged.AddListener(delegate { GlobalController.Instance.setMusicLevel(musicSlider.value); });
        dropDown.onValueChanged.AddListener(delegate { GlobalController.Instance.setLanguageSetting(dropDown.value); });
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
