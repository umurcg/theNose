using UnityEngine;
using System.Collections;

public class BookAC : MonoBehaviour {

    public GameObject bookPlane, bigBook;

    Animation bookAnimation, planeAnimation;

	// Use this for initialization
	void Awake () {

        bookAnimation= bigBook.GetComponent<Animation>();
        planeAnimation = bookPlane.GetComponent<Animation>();
       
	}

    private void Start()
    {
        //closeBook();
    }

    
    public void openBook()
    {
        bookAnimation["Open"].speed = 1;
        planeAnimation["Open"].speed = 1;

        bookAnimation.clip = bookAnimation["Open"].clip;
        planeAnimation.clip = planeAnimation["Open"].clip;

        bookAnimation.Play();
        planeAnimation.Play();   
    }

    public void closeBook()
    {

        bookAnimation["Close"].speed = 1;
        planeAnimation["Close"].speed = 1;

        bookAnimation.clip = bookAnimation["Close"].clip;
        planeAnimation.clip = planeAnimation["Close"].clip;

        bookAnimation.Play();
        planeAnimation.Play();

    }

    public void bakeClose()
    {
        bookAnimation["Close"].speed = 100;
        planeAnimation["Close"].speed = 100;

        bookAnimation.clip = bookAnimation["Close"].clip;
        planeAnimation.clip = planeAnimation["Close"].clip;

        bookAnimation.Play();
        planeAnimation.Play();
    }

    public void bakeOpen()
    {
        bookAnimation["Open"].speed = 100;
        planeAnimation["Open"].speed = 100;

        bookAnimation.clip = bookAnimation["Open"].clip;
        planeAnimation.clip = planeAnimation["Open"].clip;


        bookAnimation.Play();
        planeAnimation.Play();
    }

}
