 using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MouseTexture : MonoBehaviour {
    public Texture2D texture;
	public float factor=5;
	static GameObject oi;
	RawImage ri;
	RectTransform rt;
	bool active=false;
	public Vector2 size=new Vector2(10,10);

	public int way = 1;

	//for debug
	public bool debugShow=false;

	// Use this for initialization
	void Start () {
		if (oi == null) {
            this.enabled = false;
            //oi = GameObject.FindGameObjectWithTag ("ObjectIcon");
            
		}
        if (oi != null)
        {
            ri = oi.GetComponent<RawImage>();
            rt = oi.GetComponent<RectTransform>();
            oi.SetActive(false);
        }

	}
	
	// Update is called once per frame
	void Update () {
	
		if (active)
			UpdatePosition ();

		if (debugShow) {
			showIcon (debugShow);
		}

		

	}

	void UpdatePosition(){
		
		oi.transform.position=Camera.main.WorldToScreenPoint (transform.position+direction());
	}

	Vector3 direction(){
		switch (way) {
		case 1:
			return transform.up*factor;
		case 2:
			return transform.right * factor;
		case 3:
			return transform.forward * factor;
		case 4:
			return -transform.up*factor;
		case 5:
			return -transform.right * factor;
		case 6:
			return -transform.forward * factor;
	    default:
			return transform.up*factor;
		}
	}

	void showIcon(bool b){

		if (transform.tag=="ActiveObject"&&oi!=null) {
			
			//print (transform.name);	
			oi.SetActive (b);
			rt.sizeDelta =size;
			ri.texture = texture;

			active = b;

        }else
        {
            if(oi!=null)
            oi.SetActive(false);
        }


	}
     
    public void checkTag()
    {
        if (this.tag != "ActiveObject")
        {
  
            showIcon(false);
        }
    }

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		showIcon (true);
	}

	void OnTriggerExit(Collider col){
		if(col.tag == "Player")
		showIcon (false);
	}
}
