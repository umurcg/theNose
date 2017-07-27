using UnityEngine;
using System.Collections;

public class KovalevNoseAttachGame : MonoBehaviour {

    public GameObject nose;
    Rigidbody rb;


    public int numberOfAttachTrial = 3;
    public GameObject messageReciever;
    public string message;

    float fallingTimer;

    CursorImageScript cis;
    float cameraForwardDistance = 50;

    Quaternion originalRot;

	// Use this for initialization
	void Start () {
        rb = nose.GetComponent<Rigidbody>();
        rb.useGravity = false;
        originalRot = nose.transform.rotation;
	}

    void OnEnable()
    {
        GameObject player=CharGameController.getActiveCharacter();
        PlayerComponentController pcc=player.GetComponent<PlayerComponentController>();
        if (pcc) pcc.StopToWalk();

        cis = CharGameController.getOwner().GetComponent<CursorImageScript>();
        cis.externalTexture = cis.frontierObject;

    }


    void OnDisable()
    {
        GameObject player = CharGameController.getActiveCharacter();
        PlayerComponentController pcc = player.GetComponent<PlayerComponentController>();
        if (pcc) pcc.ContinueToWalk();
        cis.resetExternalCursor();

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
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            nose.transform.rotation = originalRot;



        }

        nose.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition+Vector3.forward* cameraForwardDistance);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition+ Vector3.forward * cameraForwardDistance);

     
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
        rb.AddTorque(Vector3.one * 50);
        numberOfAttachTrial--;
        
    }

    void finish()
    {
        messageReciever.SendMessage(message);

    }

        
}
