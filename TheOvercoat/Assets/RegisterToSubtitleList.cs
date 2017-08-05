using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using MovementEffects;


//This scripts registers object to subtitle list in whoistalking script
//If gameobject has multiple names for different languages it sepeartes it and add gameobject with selected language preferences.

public class RegisterToSubtitleList : MonoBehaviour {

    public char seperator = '_';
    public GlobalController.Language[] nameOrderInLanguages= new GlobalController.Language[] {GlobalController.Language.ENG,GlobalController.Language.TR};
    string key;

	// Use this for initialization
	void Start () {
        
        register();

	}

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= register;

        if (key == null) return;

        if(WhoIsTalking.self!=null)
            WhoIsTalking.self.removeCharacter(key, gameObject);
                
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += register;
        register();
    }

    

    void register(Scene scene, LoadSceneMode mode)
    {
        register();
        Debug.Log("New scene is load so registering to whoistalking again");
    }


    void register()
    {
        Debug.Log("Registering " + Vckrs.nameTagLayer(gameObject));
        Timing.RunCoroutine(_register());
    }

    IEnumerator<float> _register()
    {
        while (WhoIsTalking.self == null || GlobalController.Instance==null) yield return 0;

        key = getNameForLanguage();

        //if (WhoIsTalking.self == null)
        //{
        //    Debug.Log("Null who is talking " + Vckrs.nameTagLayer(gameObject));
        //    return;    }


        WhoIsTalking.self.addCharacterToDict(gameObject, key);
    }

    string getNameForLanguage()
    {
        string name = gameObject.name;
        string[] names = name.Split(seperator);

        if (nameOrderInLanguages.Length == names.Length)
        {
            int index = Array.IndexOf(nameOrderInLanguages, GlobalController.Instance.languageSetting);

            if (index != -1 && index < names.Length)
            {

                return names[index];
                
            }

        }

        return gameObject.name;
    }


    public void refreshName()
    {
        if (key!= getNameForLanguage() && WhoIsTalking.self.characters.ContainsKey(key))
        {
            WhoIsTalking.self.characters.Remove(key);


            key = getNameForLanguage();
            WhoIsTalking.self.addCharacterToDict(gameObject, key);

        }
    }
	

}
