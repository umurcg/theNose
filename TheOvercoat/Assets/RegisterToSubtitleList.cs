using UnityEngine;
using System.Collections;
using System;


//This scripts registers object to subtitle list in whoistalking script
//If gameobject has multiple names for different languages it sepeartes it and add gameobject with selected language preferences.

public class RegisterToSubtitleList : MonoBehaviour {

    public char seperator = '_';
    public GlobalController.Language[] nameOrderInLanguages= new GlobalController.Language[] {GlobalController.Language.ENG,GlobalController.Language.TR};
    string key;

	// Use this for initialization
	void Start () {

        key = getNameForLanguage();
        WhoIsTalking.self.addCharacterToDict(gameObject, key);
      

	}

    private void OnDestroy()
    {
        if (key == null) return;

        WhoIsTalking.self.removeCharacter(key, gameObject);
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
