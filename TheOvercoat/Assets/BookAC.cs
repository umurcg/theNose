using UnityEngine;
using System.Collections;
using MovementEffects;

public class BookAC : MonoBehaviour {

    public GameObject bookPlane, bigBook;

    //public float planeSpeed = 0.3f;

    //public float openPlaneHeight=55f;

    //Close plane height is first height of the plane.
    float closePlaneHeight;

    Animation bookAnimation, planeAnimation;



	// Use this for initialization
	void Awake () {

        //closePlaneHeight = transform.position.y;

        bookAnimation= bigBook.GetComponent<Animation>();
        planeAnimation = bookPlane.GetComponent<Animation>();

    }

    private void Start()
    {
        //closeBook();
    }

    
    public void openBook()
    {

        //Vector3 planeOpenPosition = new Vector3(bookPlane.transform.position.x, bookPlane.transform.position.y, openPlaneHeight);
        //Timing.RunCoroutine(Vckrs._Tween(bookPlane, planeOpenPosition, planeSpeed));

        bookAnimation["Open"].speed = 1;
        planeAnimation["Open"].speed = 1;

        bookAnimation.clip = bookAnimation["Open"].clip;
        planeAnimation.clip = planeAnimation["Open"].clip;

        bookAnimation.Play();
        planeAnimation.Play();
    }

    public void closeBook()
    {
        //Vector3 planeClosePosition = new Vector3(bookPlane.transform.position.x, bookPlane.transform.position.y, closePlaneHeight);
        //Timing.RunCoroutine(Vckrs._Tween(bookPlane, planeClosePosition, planeSpeed));

        bookAnimation["Close"].speed = 1;
        planeAnimation["Close"].speed = 1;

        bookAnimation.clip = bookAnimation["Close"].clip;
        planeAnimation.clip = planeAnimation["Close"].clip;

        bookAnimation.Play();
        planeAnimation.Play();

    }

    public void bakeClose()
    {
        //bookPlane.transform.position += Vector3.up * closePlaneHeight;

        bookAnimation["Close"].speed = 100;
        planeAnimation["Close"].speed = 100;

        bookAnimation.clip = bookAnimation["Close"].clip;
        planeAnimation.clip = planeAnimation["Close"].clip;

        bookAnimation.Play();
        planeAnimation.Play();
    }

    public void bakeOpen()
    {

        //bookPlane.transform.position += Vector3.up * openPlaneHeight;

        bookAnimation["Open"].speed = 100;
        planeAnimation["Open"].speed = 100;

        bookAnimation.clip = bookAnimation["Open"].clip;
        planeAnimation.clip = planeAnimation["Open"].clip;


        bookAnimation.Play();
        planeAnimation.Play();
    }

}
