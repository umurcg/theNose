using UnityEngine;
using System.Collections;

//This script deals with self destroy of rocks
//Also it holds creater, so creater can't shoot itself.

public class RockScript : MonoBehaviour {

    public GameObject creator;
    public float selfDestructionTimer = 5f;
    float timer;

    string message;
    GameObject reciever;
    int messageParameter;

    // Use this for initialization
    void Start () {
        timer = selfDestructionTimer;
	}
	
	// Update is called once per frame
	void Update () {
        if (timer <= 0)
        {
            Destroy(gameObject);
            return;
        }else
        {
            timer -= Time.deltaTime;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            reciever.SendMessage(message,messageParameter);

        }
    }

    public void setMessage(GameObject reciever, string message, int parameter)
    {
        this.reciever = reciever;
        this.message = message;
        this.messageParameter = parameter;
    }
}
