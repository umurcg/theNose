using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLanguagePrompt : MonoBehaviour {

    public GameObject mainMenu;


    public void turkish()
    {
        GlobalController.Instance.setLanguageSetting((int)GlobalController.Language.TR);
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void english()
    {
        GlobalController.Instance.setLanguageSetting((int)GlobalController.Language.ENG);
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
