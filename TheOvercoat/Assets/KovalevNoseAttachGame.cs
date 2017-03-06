using UnityEngine;
using System.Collections;

public class KovalevNoseAttachGame : MonoBehaviour {

    public GameObject nose;
    Rigidbody rb;


    public int numberOfAttachTrial = 3;
    public GameObject messageReciever;
    public string message;

    float fallingTimer;

	// Use this for initialization
	void Start () {
        rb = nose.GetComponent<Rigidbody>();
        rb.useGravity = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (numberOfAttachTrial <= 0 && fallingTimer<=0)
        {
            finish();
            this.enabled = false;
            return;
        }

        if (fallingTimer > 0)
        {
            fallingTimer -= Time.deltaTime;
            return;
        }

        if (fallingTimer < 0)
        {
            fallingTimer = 0;
            
        }

        nose.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

     
        if(Physics.Raycast(ray,out hit)){
            if (hit.transform.gameObject == gameObject)
            {
                //TODO change cursor
                if (Input.GetMouseButtonDown(0))
                {
                    releaseNose();
                }
            }
        }

    }


    void releaseNose()
    {
        Debug.Log("Release");
        fallingTimer = 5f;
        rb.AddRelativeForce(-8*Vector3.forward, ForceMode.Impulse);
        numberOfAttachTrial--;
        
    }

    void finish()
    {
        messageReciever.SendMessage(message);

    }

        
}
