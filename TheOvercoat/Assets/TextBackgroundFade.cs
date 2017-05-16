using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//This is fades in when text is not "" and fades out when text is ""
public class TextBackgroundFade : MonoBehaviour {

    public GameObject textObject;
    Text text;
    Image image;

    public float t = 0;
    public float fadeSpeed = 3;

    // Use this for initialization
    void Start () {
        text = textObject.GetComponent<Text>();
        image = GetComponent<Image>();

	}
	
	// Update is called once per frame
	void Update () {

        Color color = image.color;

        if (text.text != "" && color.a != 1)
        {
            if (color.a > 1)
            {
                color.a = 1;
                t = 1;
            }
            else
            {
                t += fadeSpeed * Time.deltaTime;
                color.a = Mathf.Lerp(0, 1, t);
            }

        }
        else if (text.text == "" && color.a != 0)
        {
            if (color.a < 0)
            {
                color.a = 0;
                t = 0;
            }
            else
            {
                t -= fadeSpeed * Time.deltaTime;
                color.a = Mathf.Lerp(0, 1, t);
                //     print(color.a);
            }

        }

        image.color = color;


    

    }
}
