using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IconController : MonoBehaviour {

    //Singleton
    public static IconController ico;
    //Ratios from ten of screen
    public Vector2 offset;
    public float followSpeed=3f;
  

    CursorImageScript cis;
    Texture2D text;
    RawImage ri;

    GameObject mainPlayer;
    Camera cam;


    private void OnEnable()
    {
        if (ico == this) return;

        if (ico != null)
        {
            Destroy(this);

        }else
        {
            ico = this;
        }
    }

    private void OnDestory()
    {
        ico = null;
    }

    // Use this for initialization
    void Start () {

        mainPlayer = CharGameController.getActiveCharacter();
        cam = CharGameController.getCamera().GetComponent<Camera>();

        cis = CharGameController.getOwner().GetComponent<CursorImageScript>();
        ri = GetComponent<RawImage>();


        //For now use only one texture which is activeObject texture
        text = cis.activeObject;
        ri.texture = text;

        gameObject.SetActive(false);
            
     }
	
	// Update is called once per frame
	void Update () {
        Vector3 aim= (Vector2)cam.WorldToScreenPoint(mainPlayer.transform.position) + new Vector2(Screen.width * offset.x / 10, Screen.height * offset.y / 10);
        transform.position = Vector3.Lerp(transform.position, aim, Time.deltaTime * followSpeed);
	}
    
    //public void activate()
    //{

    //}

    //public void deactivate()
    //{

    //}

    //Vector2 getPosition()
    //{
    //    return Vector2.zero;
    //}

}
