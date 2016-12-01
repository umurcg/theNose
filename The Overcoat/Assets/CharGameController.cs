using UnityEngine;
using System.Collections;

public class CharGameController : MonoBehaviour {
    static CharGameController cgc;

	// Use this for initialization
	void Awake () {
        if (cgc == null)
        {
            cgc = this;
            Object.DontDestroyOnLoad(gameObject);
        }
        else{
            Object.DontDestroyOnLoad(cgc.gameObject);
        }

    }
	



    public static void setCharacter(string characterName)
    {
        

        int childCount = cgc.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = cgc.transform.GetChild(i);
            if (child.name != "Main Camera")
            {
                for(int j = 0; j < child.childCount; j++)
                {
                    Transform grandChild = child.GetChild(j);
                    print(grandChild.name);
                    if (grandChild.name == characterName)
                    {
                        grandChild.gameObject.SetActive(true);
                    }else
                    {
                        grandChild.gameObject.SetActive(false);

                    }
                }

            }
                
         
        }
    }
}
