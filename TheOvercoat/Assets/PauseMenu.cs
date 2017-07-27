using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using MovementEffects;


//TODO FADE FUNCTIONALITY
public class PauseMenu : MonoBehaviour {

    //public GameObject continueBut, restartBut, settingsBut, exitBut;
    public GameObject mainButtons, settingsSubMenu;
    public Texture2D pauseMenuText;

    EnableDisableUI MBedUI;
    EnableDisableUI SBMedUI;

    CursorImageScript cis;
    

    bool paused = false;

    float musicLevel, audioLevel;

	// Use this for initialization
	void Start () {
        MBedUI = mainButtons.GetComponent<EnableDisableUI>();
        SBMedUI = settingsSubMenu.GetComponent<EnableDisableUI>();
        cis =CharGameController.getOwner().GetComponent<CursorImageScript>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Cancel"))
        {
            switchMenu();
        }
	}

    void switchMenu()
    {
        if (paused)
        {
            pause(false);
        }else
        {
            pause(true);
        }
        paused = !paused;
    }
        
    void pause(bool b)
    {
        Timing.RunCoroutine(_pause(b));

    }

    IEnumerator <float> _pause(bool b)
    {

        //Open pause menu
        if (b)
        {
            cis.externalTexture = pauseMenuText;
            Vckrs.enableAllChildren(mainButtons.transform);
            Time.timeScale = 0;

            //musicLevel = GlobalController.Instance.getMusicLevel();
            //audioLevel = GlobalController.Instance.getAudioLevel();

            ////Mute all sounds while game is paused
            //GlobalController.Instance.setMusicLevel(0);
            //GlobalController.Instance.setAudioLevel(0);

            //Debug.Log("AFX source is placed as " + (GlobalController.Instance.gameObject.GetComponents<AudioSource>()[0] == GlobalController.Instance.afxSource));

            GlobalController.Instance.musicSouce.Pause();
            GlobalController.Instance.afxSource.Pause();


        }
        else
        {
            cis.resetExternalCursor();
            Vckrs.disableAllChildren(mainButtons.transform);
            Vckrs.disableAllChildren(settingsSubMenu.transform);
            Time.timeScale = 1;

            //GlobalController.Instance.setMusicLevel(musicLevel);
            //GlobalController.Instance.setAudioLevel(audioLevel);

            GlobalController.Instance.musicSouce.UnPause();
            GlobalController.Instance.afxSource.UnPause();

        }

        GetComponent<RawImage>().enabled = b;

        //if (b) Time.timeScale = 1;

        //IEnumerator<float> handler = null;

        //if (b)
        //{
        //   handler=Timing.RunCoroutine( MBedUI._deactivate());
        //}
        //else
        //{
        //    handler = Timing.RunCoroutine(MBedUI._activate());
        //}

        //if(!b)
        //{
        //    yield return Timing.WaitUntilDone(handler);
        //    Time.timeScale = 0;

        //}     

        //GetComponent<RawImage>().enabled = b;

        ////Time.timeScale = b ? 0 : 1;

        yield break;

    }

    public void continueFunc() {

        switchMenu();

    }

    //TODO think about game controllers
    public void restartFunc()
    {

        GlobalController.Instance.removeLastScene();
                
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        GameObject player = CharGameController.getActiveCharacter();
        if (player != null) player.GetComponent<PlayerComponentController>().ContinueToWalk();

        CharGameController.getSun().GetComponent<DayAndNightCycle>().makeDay();
    }

    public void settingsFunc()
    {
        Vckrs.enableAllChildren(settingsSubMenu.transform);
        Vckrs.disableAllChildren(mainButtons.transform);
        //SBMedUI.activate();
        //MBedUI.deactivate();

    }

    public void returnToMenu()
    {
        Vckrs.disableAllChildren(settingsSubMenu.transform);
        Vckrs.enableAllChildren(mainButtons.transform);
    }

    public void exitFunc()
    {
        SceneManager.LoadScene(0);
    }


}
